using PayIn.Application.Dto.Arguments.ServiceTag;
using PayIn.Application.Dto.Results.ServiceTag;
using PayIn.Domain.Security;
using PayIn.Web.Security;
using System.Threading.Tasks;
using System.Web.Http;
using Xp.Application.Dto;
using Xp.DistributedServices.Filters;
using Xp.DistributedServices.ModelBinder;

namespace PayIn.DistributedServices.Controllers.Mobile
{
	[HideSwagger]
	[RoutePrefix("Mobile/ServiceTag")]
	[XpAuthorize(
		ClientIds = AccountClientId.AndroidNative,
		Roles = AccountRoles.User
	)]
	public class MobileServiceTagController : ApiController
	{
		#region GET /v1/Tag/{reference:string}
		[HttpGet]
		[Route("v1/Tag/{reference}")]
		public async Task<ResultBase<ServiceTagMobileGetResult>> GetTag(
			[FromUri] ServiceTagMobileGetArguments arguments,
			[Injection] IQueryBaseHandler<ServiceTagMobileGetArguments, ServiceTagMobileGetResult> handler
		)
		{
			var result = await handler.ExecuteAsync(arguments);
			return result;
		}
		#endregion GET /v1/Tag/{reference:string}
	}
}
