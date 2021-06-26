using PayIn.Application.Dto.Arguments.ControlPlanning;
using PayIn.Application.Dto.Results;
using PayIn.Domain.Public;
using PayIn.Domain.Security;
using PayIn.Web.Security;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Http;
using Xp.Application.Dto;
using Xp.DistributedServices.Filters;
using Xp.DistributedServices.ModelBinder;

namespace PayIn.DistributedServices.Controllers
{
	[HideSwagger]
	[RoutePrefix("Api/ControlPlanning")]
	[XpAuthorize(
		ClientIds = AccountClientId.Web,
		Roles = AccountRoles.Operator
	)]
	public class ControlPlanningController : ApiController
	{
		#region GET /
		[HttpGet]
		[Route("")]
		public async Task<ResultBase<ControlPlanningGetAllResult>> Get(
			[FromUri] ControlPlanningGetAllArguments arguments,
			[Injection] IQueryBaseHandler<ControlPlanningGetAllArguments, ControlPlanningGetAllResult> handler
		)
		{
			var result = await handler.ExecuteAsync(arguments);
			return result;
		}
		#endregion GET /

		#region GET /{id:int}
		[HttpGet]
		[Route("{id:int}")]
		public async Task<ResultBase<ControlPlanningGetResult>> Get(
			[FromUri] ControlPlanningGetArguments arguments,
			[Injection] IQueryBaseHandler<ControlPlanningGetArguments, ControlPlanningGetResult> handler
		)
		{
			var result = await handler.ExecuteAsync(arguments);
			return result;
		}
		#endregion GET /{id:int}

		#region POST /
		[HttpPost]
		[Route("")]
		public async Task<dynamic> Post(
			ControlPlanningCreateArguments arguments,
			[Injection] IServiceBaseHandler<ControlPlanningCreateArguments> handler
		)
		{
			var result = await handler.ExecuteAsync(arguments);
			return result;
		}
		#endregion POST /

		#region POST /Clear
		[HttpPost]
		[Route("Clear")]
		public async Task<dynamic> Clear(
			ControlPlanningClearArguments arguments,
			[Injection] IServiceBaseHandler<ControlPlanningClearArguments> handler
		)
		{
			var result = await handler.ExecuteAsync(arguments);
			return result;
		}
		#endregion POST /Clear

		#region POST /Template
		[HttpPost]
		[Route("Template")]
		public async Task<dynamic> Template(
			ControlPlanningCreateTemplateArguments arguments,
			[Injection] IServiceBaseHandler<ControlPlanningCreateTemplateArguments> handler
		)
		{
			List<ControlFormAssign> result = await handler.ExecuteAsync(arguments);

			return new { Ids = result.Select(x => x.Id.ToString()).JoinString(",") };
		}
		#endregion POST /Template

		#region PUT /{id:int}
		[HttpPut]
		[Route("{id:int}")]
		public async Task<dynamic> Put(
			ControlPlanningUpdateArguments arguments,
			[Injection] IServiceBaseHandler<ControlPlanningUpdateArguments> handler
		)
		{
			var item = await handler.ExecuteAsync(arguments);
			return new { item.Id };
		}
		#endregion PUT /{id:int}

		#region DELETE /{id:int}
		[HttpDelete]
		[Route("{id:int}")]
		public async Task<dynamic> Delete(
			[FromUri] ControlPlanningDeleteArguments arguments,
			[Injection] IServiceBaseHandler<ControlPlanningDeleteArguments> handler
		)
		{
			var result = await handler.ExecuteAsync(arguments);
			return result;
		}
		#endregion DELETE /{id:int}
	}
}
