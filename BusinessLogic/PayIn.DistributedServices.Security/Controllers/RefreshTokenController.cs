using PayIn.Infrastructure.Security;
using System.Threading.Tasks;
using System.Web.Http;
using Xp.DistributedServices.Filters;

namespace PayIn.DistributedServices.Security.Controllers
{
	[HideSwagger]
	[RoutePrefix("api/RefreshToken")]
	public class RefreshTokenController : ApiController
	{
		private readonly SecurityRepository repository;

		#region Constructors
		public RefreshTokenController()
		{
			repository = new SecurityRepository();
		}
		#endregion Constructors

		#region DELETE /
		[AllowAnonymous]
		[Route("")]
		public async Task<IHttpActionResult> Delete(string tokenId)
		{
			var result = await repository.DeleteRefreshTokenAsync(tokenId);
			if (result)
				return Ok();

			return BadRequest("Token Id does not exist");
		}
		#endregion DELETE /

		#region Dispose
		protected override void Dispose(bool disposing)
		{
			if (disposing)
				if (repository != null)
					repository.Dispose();

			base.Dispose(disposing);
		}
		#endregion Dispose
	}
}
