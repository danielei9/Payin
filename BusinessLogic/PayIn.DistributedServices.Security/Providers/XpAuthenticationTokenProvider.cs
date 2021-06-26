using Microsoft.Owin.Security.Infrastructure;
using PayIn.Domain.Security;
using PayIn.Infrastructure.Security;
using System;
using System.Threading.Tasks;

namespace PayIn.DistributedServices.Security.Providers
{
	public class XpAuthenticationTokenProvider : IAuthenticationTokenProvider
	{
		#region CreateAsync
		public async Task CreateAsync(AuthenticationTokenCreateContext context)
		{
			var clientid = context.Ticket.Properties.Dictionary["as:client_id"];
			if (string.IsNullOrEmpty(clientid))
				return;

			var refreshTokenId = Guid.NewGuid().ToString("n");

			using (var repository = new SecurityRepository())
			{
				var refreshTokenLifeTime = context.OwinContext.Get<string>("as:clientRefreshTokenLifeTime");

				var token = new RefreshToken()
				{
					Id = refreshTokenId.ToHash(),
					ClientId = clientid,
					Subject = context.Ticket.Identity.Name,
					IssuedUtc = DateTime.UtcNow,
					ExpiresUtc = DateTime.UtcNow.AddMinutes(Convert.ToDouble(refreshTokenLifeTime))
				};

				context.Ticket.Properties.IssuedUtc = token.IssuedUtc;
				context.Ticket.Properties.ExpiresUtc = token.ExpiresUtc;

				token.ProtectedTicket = context.SerializeTicket();

				var result = await repository.UpdateRefreshTokenAsync(token);

				if (result)
					context.SetToken(refreshTokenId);
			}
		}
		#endregion CreateAsync

		#region ReceiveAsync
		public async Task ReceiveAsync(AuthenticationTokenReceiveContext context)
		{
			var allowedOrigin = context.OwinContext.Get<string>("as:clientAllowedOrigin");
			if (!string.IsNullOrEmpty(allowedOrigin))
				context.OwinContext.Response.Headers.Add("Access-Control-Allow-Origin", new[] { allowedOrigin });

			string hashedTokenId = context.Token.ToHash();

			using (var repository = new SecurityRepository())
			{
				var refreshToken = await repository.GetRefreshTokenAsync(hashedTokenId);

				if (refreshToken != null)
				{
					//Get protectedTicket from refreshToken class
					context.DeserializeTicket(refreshToken.ProtectedTicket);
					var result = await repository.DeleteRefreshTokenAsync(hashedTokenId);
				}
			}
		}
		#endregion ReceiveAsync

		#region Create
		public void Create(AuthenticationTokenCreateContext context)
		{
			throw new NotImplementedException();
		}
		#endregion Create

		#region Receive
		public void Receive(AuthenticationTokenReceiveContext context)
		{
			throw new NotImplementedException();
		}
		#endregion Receive
	}
}
