using PayIn.Application.Dto.Arguments.ControlFormAssignTemplate;
using PayIn.Application.Dto.Results.ControlFormAssignTemplate;
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
	[RoutePrefix("Api/ControlFormAssignTemplate")]
	[XpAuthorize(
		ClientIds = AccountClientId.Web,
		Roles = AccountRoles.Operator
	)]
	public class ControlFormAssignTemplateController : ApiController
	{
		#region GET /{id:int}
		[HttpGet]
		[Route("{id:int}")]
		public async Task<ResultBase<ControlFormAssignTemplateGetResult>> Get(
			[FromUri] ControlFormAssignTemplateGetArguments arguments,
			[Injection] IQueryBaseHandler<ControlFormAssignTemplateGetArguments, ControlFormAssignTemplateGetResult> handler
		)
		{
			var result = await handler.ExecuteAsync(arguments);
			return result;
		}
		#endregion GET /{id:int}

		#region GET /{filter?}
		[HttpGet]
		[Route("{filter?}")]
		public async Task<ResultBase<ControlFormAssignTemplateGetCheckResult>> Get(
			[FromUri] ControlFormAssignTemplateGetCheckArguments arguments,
			[Injection] IQueryBaseHandler<ControlFormAssignTemplateGetCheckArguments, ControlFormAssignTemplateGetCheckResult> handler
		)
		{
			var result = await handler.ExecuteAsync(arguments);
			return result;
		}
		#endregion GET /{filter?}

		#region POST /
		[HttpPost]
		[Route("")]
		public async Task<dynamic> Post(
			ControlFormAssignTemplateCreateArguments command,
			[Injection] IServiceBaseHandler<ControlFormAssignTemplateCreateArguments> handler
		)
		{
			var item = await handler.ExecuteAsync(command);
			return new { item.Id };
		}
		#endregion POST /

		#region PUT /{id:int}
		[HttpPut]
		[Route("{id:int}")]
		public async Task<dynamic> Put(
			int id,
			ControlFormAssignTemplateUpdateArguments arguments,
			[Injection] IServiceBaseHandler<ControlFormAssignTemplateUpdateArguments> handler
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
			[FromUri] ControlFormAssignTemplateDeleteArguments arguments,
			[Injection] IServiceBaseHandler<ControlFormAssignTemplateDeleteArguments> handler
		)
		{
			var result = await handler.ExecuteAsync(arguments);
			return result;
		}
		#endregion DELETE /{id:int}
	}
}
