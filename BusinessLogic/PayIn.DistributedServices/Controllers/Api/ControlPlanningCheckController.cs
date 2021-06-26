using PayIn.Application.Dto.Arguments.ControlPlanningCheck;
using PayIn.Application.Dto.Results.ControlPlanningCheck;
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
	[RoutePrefix("Api/ControlPlanningCheck")]
	[XpAuthorize(
		ClientIds = AccountClientId.Web,
		Roles = AccountRoles.Operator
	)]
	public class ControlPlanningCheckController : ApiController
	{
		#region POST /
		[HttpPost]
		[Route("")]
		public async Task<dynamic> Template(
			ControlPlanningCheckCreateArguments arguments,
			[Injection] IServiceBaseHandler<ControlPlanningCheckCreateArguments> handler
		)
		{
			var result = await handler.ExecuteAsync(arguments);
			return result;
		}
		#endregion POST /

		#region GET /{id:int}
		[HttpGet]
		[Route("{id:int}")]
		public async Task<ResultBase<ControlPlanningCheckGetResult>> Get(
			[FromUri] ControlPlanningCheckGetArguments command,
			[Injection] IQueryBaseHandler<ControlPlanningCheckGetArguments, ControlPlanningCheckGetResult> handler
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
			ControlPlanningCheckUpdateArguments command,
			[Injection] IServiceBaseHandler<ControlPlanningCheckUpdateArguments> handler
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
			[FromUri] ControlPlanningCheckDeleteArguments arguments,
			[Injection] IServiceBaseHandler<ControlPlanningCheckDeleteArguments> handler
		)
		{
			var result = await handler.ExecuteAsync(arguments);
			return result;
		}
		#endregion DELETE /{id:int}
	}
}
