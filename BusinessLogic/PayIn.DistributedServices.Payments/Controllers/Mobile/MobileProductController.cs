using PayIn.Application.Dto.Payments.Arguments;
using PayIn.Application.Dto.Payments.Results;
using PayIn.Domain.Security;
using PayIn.Web.Security;
using System.Threading.Tasks;
using System.Web.Http;
using Xp.Application.Dto;
using Xp.DistributedServices.Filters;
using Xp.DistributedServices.ModelBinder;

namespace PayIn.DistributedServices.Payments.Controllers.Mobile
{
	[HideSwagger]
	[RoutePrefix("Mobile/Product/v1")]
	[XpAuthorize(
		ClientIds = AccountClientId.AndroidNative + "," + AccountClientId.AndroidFallesNative,
		Roles = AccountRoles.User
	)]
	public class MobileProductController : ApiController
	{
		#region GET /{id:int}
		[HttpGet]
		[Route("{id:int}")]
		public async Task<ResultBase<ProductMobileGetResult>> Get(
			[FromUri] ProductMobileGetArguments argument,
			[Injection] IQueryBaseHandler<ProductMobileGetArguments, ProductMobileGetResult> handler)
		{
			var result = await handler.ExecuteAsync(argument);
			return result;
		}
		#endregion GET /{id:int}

		#region GET /Tpv
		[HttpGet]
		[Route("Tpv")]
		public async Task<ResultBase<ProductMobileGetTpvResult>> GetTpv(
			[FromUri] ProductMobileGetTpvArguments argument,
			[Injection] IQueryBaseHandler<ProductMobileGetTpvArguments, ProductMobileGetTpvResult> handler)
		{
			var result = await handler.ExecuteAsync(argument);
			return result;
		}
		#endregion GET /Tpv
	}
}
