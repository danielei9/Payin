using Microsoft.Owin.Security;
using Microsoft.Owin.Security.OAuth;
using PayIn.Domain.Security;
using PayIn.Infrastructure.Security;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Xp.Common;
using Xp.Common.Resources;

namespace PayIn.DistributedServices.Security.Providers
{
	public class XpOAuthAuthorizationServerProvider : OAuthAuthorizationServerProvider
	{
		#region ValidateClientAuthentication
		public async override Task ValidateClientAuthentication(OAuthValidateClientAuthenticationContext context)
		{
			var clientId = string.Empty;
			var clientSecret = string.Empty;

			if (!context.TryGetBasicCredentials(out clientId, out clientSecret))
				context.TryGetFormCredentials(out clientId, out clientSecret);

			if (context.ClientId == null)
			{
				//Remove the comments from the below line context.SetError, and invalidate context 
				//if you want to force sending clientId/secrects once obtain access tokens. 
				//context.Validated();
				context.SetError("invalid_clientId", "ClientId should be sent.");
				context.Rejected();
				return;
			}

			using (var repository = new SecurityRepository())
			{
				var client = await repository.GetClientAsync(context.ClientId);
				if (client == null)
				{
					context.SetError("invalid_clientId", string.Format("Client '{0}' is not registered in the system.", context.ClientId));
					return;
				}

				if (client.ApplicationType == ApplicationTypes.NativeConfidential)
				{
					if (string.IsNullOrWhiteSpace(clientSecret))
					{
						context.SetError("invalid_clientId", "Client secret should be sent.");
						return;
					}

					if (client.Secret != clientSecret.ToHash())
					{
						context.SetError("invalid_clientId", "Client secret is invalid.");
						return;
					}
				}

				if (!client.Active)
				{
					context.SetError("invalid_clientId", "Client is inactive.");
					return;
				}

				context.OwinContext.Set<string>("as:clientAllowedOrigin", client.AllowedOrigin);
				context.OwinContext.Set<string>("as:clientRefreshTokenLifeTime", client.RefreshTokenLifeTime.ToString());

				context.Validated();
			}
		}
		#endregion ValidateClientAuthentication

		#region GrantResourceOwnerCredentials
		public override async Task GrantResourceOwnerCredentials(OAuthGrantResourceOwnerCredentialsContext context)
		{
			var allowedOrigin = context.OwinContext.Get<string>("as:clientAllowedOrigin");
			var name = context.OwinContext.Get<string>("as:name");

			//if (allowedOrigin == null)
			//	allowedOrigin = "*";
			if (!string.IsNullOrEmpty(allowedOrigin))
				context.OwinContext.Response.Headers.Add("Access-Control-Allow-Origin", new[] { allowedOrigin });

			using (var repository = new SecurityRepository())
			{
				var user = await repository.AutenticateUserAsync(context.UserName, context.Password);
				if (user == null)
				{
					context.SetError("invalid_grant", AccountResources.InvalidUserOrPasswordException);
					return;
				}
				if (!user.EmailConfirmed)
				{
					context.SetError("invalid_grant", AccountResources.EmailNotConfirmedException);
					return;
				}
				if (user.AccessFailedCount > 3)
				{
					context.SetError("invalid_grant", AccountResources.UserLockedException);
					return;
				}

				var roles = await repository.GetRoleAsync(user.Roles.Select(x => x.RoleId));
				if (roles == null)
				{
					context.SetError("invalid_grant", AccountResources.IncorrectRoleException);
					return;
				}

				var clientId = (context.ClientId == null) ? string.Empty : context.ClientId;
				var rolesString = roles.Select(x => x.Name).JoinString(",");

				var identity = new ClaimsIdentity(context.Options.AuthenticationType);
				foreach(var role in roles)
					SetClaim(identity, ClaimTypes.Role, role.Name);
				SetClaim(identity, ClaimTypes.Name, user.UserName);
				SetClaim(identity, XpClaimTypes.Name, user.Name);
				SetClaim(identity, XpClaimTypes.Email, user.Email);
				SetClaim(identity, XpClaimTypes.TaxNumber, user.TaxNumber);
				SetClaim(identity, XpClaimTypes.TaxName, user.TaxName);
				SetClaim(identity, XpClaimTypes.TaxAddress, user.TaxAddress);
				SetClaim(identity, XpClaimTypes.ClientId, clientId);

				var props = new AuthenticationProperties(new Dictionary<string, string> {
					{ "as:client_id", clientId},
					//{ "userName", context.UserName },
					{ "as:name", user.Name },
					{ "as:roles", rolesString }
				});

				var ticket = new AuthenticationTicket(identity, props);
				context.Validated(ticket);
			}
		}
		#endregion GrantResourceOwnerCredentials

		#region GrantRefreshToken
		public async override Task GrantRefreshToken(OAuthGrantRefreshTokenContext context)
		{
			var clientId = context.Ticket.Properties.Dictionary["as:client_id"];
			var currentClient = context.ClientId;

			if (clientId != currentClient)
			{
				context.SetError("invalid_clientId", "Refresh token is issued to a different clientId.");
				return;
			}

			using (var repository = new SecurityRepository())
			{
				var user = await repository.GetUserAsync(context.Ticket.Identity.Name);
				if (user == null)
				{
					context.SetError("invalid_grant", "The user name or password is incorrect.");
					return;
				}

				var roles = await repository.GetRoleAsync(user.Roles.Select(x => x.RoleId));
				if (roles == null)
				{
					context.SetError("invalid_grant", "The role is incorrect.");
					return;
				}

				// Change auth ticket for refresh token requests
				var identity = new ClaimsIdentity(context.Ticket.Identity);
				var rolesString = roles.Select(x => x.Name).JoinString(",");

				foreach (var role in roles)
					SetClaim(identity, ClaimTypes.Role, role.Name);
				SetClaim(identity, ClaimTypes.Name, user.UserName);
				SetClaim(identity, XpClaimTypes.Name, user.Name);
				SetClaim(identity, XpClaimTypes.Email, user.Email);
				SetClaim(identity, XpClaimTypes.TaxNumber, user.TaxNumber);
				SetClaim(identity, XpClaimTypes.TaxName, user.TaxName);
				SetClaim(identity, XpClaimTypes.TaxAddress, user.TaxAddress);
				SetClaim(identity, XpClaimTypes.ClientId, clientId);

				var props = new AuthenticationProperties(new Dictionary<string, string> {
					{ "as:client_id", clientId},
					//{ "userName", context.UserName },
					{ "as:name", user.Name },
					{ "as:roles", rolesString }
				});

				var newTicket = new AuthenticationTicket(identity, props);
				context.Validated(newTicket);
			}
		}
		#endregion GrantRefreshToken

		#region TokenEndpoint
		public override Task TokenEndpoint(OAuthTokenEndpointContext context)
		{
			foreach (KeyValuePair<string, string> property in context.Properties.Dictionary)
				context.AdditionalResponseParameters.Add(property.Key, property.Value);

			return Task.FromResult<object>(null);
		}
		#endregion TokenEndpoint

		#region SetClaim
		private void SetClaim(ClaimsIdentity identity, string type, string value)
		{
			var newClaim = identity.Claims
				.Where(c => 
					c.Type == type && 
					c.Value == value
				)
				.FirstOrDefault();
			if (newClaim != null)
				identity.RemoveClaim(newClaim);

			identity.AddClaim(new Claim(type, value));
		}
		#endregion SetClaim
	}
}
