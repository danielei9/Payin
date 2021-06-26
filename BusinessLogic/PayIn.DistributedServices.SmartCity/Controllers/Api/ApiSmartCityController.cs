using PayIn.Application.Dto.SmartCity.Arguments;
using PayIn.Application.Dto.SmartCity.Results;
using PayIn.Domain.Security;
using PayIn.Web.Security;
using System.Threading.Tasks;
using System.Web.Http;
using Xp.Application.Dto;
using Xp.DistributedServices.Filters;
using Xp.DistributedServices.ModelBinder;

namespace PayIn.DistributedServices.SmartCity.Controllers.Api
{
	[HideSwagger]
	[RoutePrefix("SmartCity/Api/SmartCity")]
    [XpAuthorize(
           ClientIds = AccountClientId.Web,
           Roles = AccountRoles.Superadministrator + "," + AccountRoles.Operator + "," + AccountRoles.Commerce + "," + AccountRoles.CommercePayment
    )]
    public class ApiSmartCityController : ApiController
	{
		#region GET /
		[HttpGet]
		[Route()]
		public async Task<ResultBase<ApiSmartCityGetAllResult>> GetAll(
			[FromUri] ApiSmartCityGetAllArguments arguments,
			[Injection] IQueryBaseHandler<ApiSmartCityGetAllArguments, ApiSmartCityGetAllResult> handler
		)
		{
			var items = await handler.ExecuteAsync(arguments);
			return items;
		}
		#endregion GET /{componentId:int}
	}
}
