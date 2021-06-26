using PayIn.Application.Dto.Arguments.ControlPlanningItem;
using PayIn.Application.Dto.Results.ControlPlanningItem;
using PayIn.Domain.Security;
using PayIn.Web.Security;
using System.Threading.Tasks;
using System.Web.Http;
using Xp.Application.Dto;
using Xp.DistributedServices.Filters;
using Xp.DistributedServices.ModelBinder;

namespace PayIn.DistributedServices.Controllers
{
	[HideSwagger]
	[RoutePrefix("Api/ControlPlanningItem")]
	[XpAuthorize(
		ClientIds = AccountClientId.Web,
		Roles = AccountRoles.Operator
	)]
	public class ControlPlanningItemController : ApiController
	{
		#region POST /
		[HttpPost]
		[Route("")]
		public async Task<dynamic> Post(
			ControlPlanningItemCreateArguments arguments,
			[Injection] IServiceBaseHandler<ControlPlanningItemCreateArguments> handler
		)
		{
			var result = await handler.ExecuteAsync(arguments);
			return result;
		}
		#endregion POST /

		#region GET /{id:int}
		[HttpGet]
		[Route("{id:int}")]
		public async Task<ResultBase<ControlPlanningItemGetResult>> Get(
			[FromUri] ControlPlanningItemGetArguments command,
			[Injection] IQueryBaseHandler<ControlPlanningItemGetArguments, ControlPlanningItemGetResult> handler
		)
		{
			var result = await handler.ExecuteAsync(command);
			return result;
		}
		#endregion GET /{id:int}

		#region PUT /{id:int}
		[HttpPut]
		[Route("{id:int}")]
		public async Task<dynamic> Put(
			ControlPlanningItemUpdateArguments command,
			[Injection] IServiceBaseHandler<ControlPlanningItemUpdateArguments> handler
		)
		{	
			var item = await handler.ExecuteAsync(command);
			return new { item.Id };
		}
		#endregion PUT /{id:int}

		#region DELETE /{id:int}
		[HttpDelete]
		[Route("{id:int}")]
		public async Task<dynamic> Delete(
			[FromUri] ControlPlanningItemDeleteArguments arguments,
			[Injection] IServiceBaseHandler<ControlPlanningItemDeleteArguments> handler
		)
		{
			var result = await handler.ExecuteAsync(arguments);
			return result;
		}
		#endregion DELETE /{id:int}
	}
}
