using PayIn.Application.Dto.SmartCity.Arguments;
using PayIn.Application.Dto.SmartCity.Results;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Http;
using Xp.Application.Dto;
using Xp.DistributedServices.Filters;
using Xp.DistributedServices.ModelBinder;

namespace PayIn.DistributedServices.SmartCity.Controllers.Sentilo
{
	[HideSwagger]
	[RoutePrefix("Sentilo/Alert")]
    //[XpAuthorize(
    //       ClientIds = AccountClientId.Web,
    //       Roles = AccountRoles.Superadministrator + "," + AccountRoles.Operator + "," + AccountRoles.Commerce + "," + AccountRoles.CommercePayment
    //)]
    public class SentiloAlertController : ApiController
	{
		#region GET /
		[HttpGet]
		[Route()]
		public async Task<SentiloAlertGetAllResult> GetAll(
			int id,
			[FromUri] SentiloAlertGetAllArguments arguments,
			[Injection] IQueryBaseHandler<SentiloAlertGetAllArguments, SentiloAlertGetAllResult> handler
		)
		{
			var items = await handler.ExecuteAsync(arguments);
			return items.Data.FirstOrDefault();
		}
		#endregion GET /

		#region PUT /{id:int}
		[HttpPut]
		[Route("{alertId:int}")]
		public async Task<dynamic> Put(
			int alertId,
			[FromBody] SentiloAlertUpdateArguments arguments,
			[Injection] IServiceBaseHandler<SentiloAlertUpdateArguments> handler
		)
		{
			arguments.Id = alertId;
			var item = await handler.ExecuteAsync(arguments);
			return new { item.Id };
		}
		#endregion PUT /{id:int}
	}
}
