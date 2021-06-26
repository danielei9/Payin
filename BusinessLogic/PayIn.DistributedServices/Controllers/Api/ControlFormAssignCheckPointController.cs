using PayIn.Application.Dto.Arguments.ControlFormAssignCheckPoint;
using PayIn.Application.Dto.Results.ControlFormAssignCheckPoint;
using PayIn.Domain.Security;
using PayIn.Web.Security;
using System.Threading.Tasks;
using System.Web.Http;
using Xp.Application.Dto;
using Xp.DistributedServices.Filters;
using Xp.DistributedServices.ModelBinder;

namespace PayIn.DistributedServices.Controllers.Api
{
	[HideSwagger]
	[RoutePrefix("Api/ControlFormAssignCheckPoint")]
	[XpAuthorize(
		ClientIds = AccountClientId.Web,
		Roles = AccountRoles.Operator
	)]
	public class ControlFormAssignCheckPointController : ApiController
	{
		#region GET /{id:int}
		[HttpGet]
		[Route("{id:int}")]
		public async Task<ResultBase<ControlFormAssignCheckPointGetResult>> Get(
			[FromUri] ControlFormAssignCheckPointGetArguments arguments,
			[Injection] IQueryBaseHandler<ControlFormAssignCheckPointGetArguments, ControlFormAssignCheckPointGetResult> handler
		)
		{
			var result = await handler.ExecuteAsync(arguments);
			return result;
		}
		#endregion GET /{id:int}

		#region GET /Check
		[HttpGet]
		[Route("{filter?}")]
		public async Task<ResultBase<ControlFormAssignCheckPointGetCheckResult>> Get(
			[FromUri] ControlFormAssignCheckPointGetCheckArguments arguments,
			[Injection] IQueryBaseHandler<ControlFormAssignCheckPointGetCheckArguments, ControlFormAssignCheckPointGetCheckResult> handler
		)
		{
			var result = await handler.ExecuteAsync(arguments);
			return result;
		}
		#endregion GET /Check

		#region POST /
		[HttpPost]
		[Route("")]
		public async Task<dynamic> Post(
			ControlFormAssignCheckPointCreateArguments command,
			[Injection] IServiceBaseHandler<ControlFormAssignCheckPointCreateArguments> handler
		)
		{
			var item = await handler.ExecuteAsync(command);
			return new { item.Id };
		}
		#endregion POST /

		//#region PUT /{id:int}
		//[HttpPut]
		//[Route("{id:int}")]
		//public async Task<dynamic> Put(
		//	int id,
		//	ControlFormAssignTemplateUpdateArguments arguments,
		//	[Injection] IServiceBaseHandler<ControlFormAssignTemplateUpdateArguments> handler
		//)
		//{
		//	var item = await handler.ExecuteAsync(arguments);
		//	return new { item.Id };
		//}
		//#endregion PUT /{id:int}

		#region DELETE /{id:int}
		[HttpDelete]
		[Route("{id:int}")]
		public async Task<dynamic> Delete(
			[FromUri] ControlFormAssignCheckPointDeleteArguments arguments,
			[Injection] IServiceBaseHandler<ControlFormAssignCheckPointDeleteArguments> handler
		)
		{
			var result = await handler.ExecuteAsync(arguments);
			return result;
		}
		#endregion DELETE /{id:int}
	}
}
