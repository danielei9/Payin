using PayIn.Application.Dto.Arguments.ControlTemplate;
using PayIn.Application.Dto.Results.ControlTemplate;
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
	[RoutePrefix("Api/ControlTemplate")]
	[XpAuthorize(
		ClientIds = AccountClientId.Web,
		Roles = AccountRoles.Operator
	)]
	public class ControlTemplateController : ApiController
	{
		#region GET /{filter?}
		[HttpGet]
		[Route("{filter?}")]
		public async Task<ResultBase<ControlTemplateGetAllResult>> Get(
			[FromUri] ControlTemplateGetAllArguments arguments,
			[Injection] IQueryBaseHandler<ControlTemplateGetAllArguments, ControlTemplateGetAllResult> handler
		)
		{
			var result = await handler.ExecuteAsync(arguments);
			return result;
		}
		#endregion GET /{filter?}

		#region GET /{id:int}
		[HttpGet]
		[Route("{id:int}")]
		public async Task<ResultBase<ControlTemplateGetResult>> Get(
			[FromUri] ControlTemplateGetArguments arguments,
			[Injection] IQueryBaseHandler<ControlTemplateGetArguments, ControlTemplateGetResult> handler
		)
		{
			var result = await handler.ExecuteAsync(arguments);
			return result;
		}
		#endregion GET /{id:int}

		#region GET /Selector/{filter?}
		[HttpGet]
		[Route("Selector/{filter?}")]
		public async Task<ResultBase<ControlTemplateGetSelectorResult>> Selector(
			string filter,
			[FromUri] ControlTemplateGetSelectorArguments arguments,
			[Injection] IQueryBaseHandler<ControlTemplateGetSelectorArguments, ControlTemplateGetSelectorResult> handler
		)
		{
			var result = await handler.ExecuteAsync(arguments);
			return result;
		}
		#endregion GET /Selector/{filter?}

		#region POST /
		[HttpPost]
		[Route]
		public async Task<dynamic> Post(
			ControlTemplateCreateArguments arguments,
			[Injection] IServiceBaseHandler<ControlTemplateCreateArguments> handler
		)
		{
			var item = await handler.ExecuteAsync(arguments);
			return new { item.Id };
		}
		#endregion POST /

		#region PUT /
		[HttpPut]
		[Route("{id:int}")]
		public async Task<dynamic> Put(
			ControlTemplateUpdateArguments arguments,
			[Injection] IServiceBaseHandler<ControlTemplateUpdateArguments> handler
		)
		{
			var item = await handler.ExecuteAsync(arguments);
			return new { item.Id };
		}
		#endregion PUT /

		#region DELETE /
		[HttpDelete]
		[Route("{id:int}")]
		public async Task<dynamic> Delete(
			[FromUri] ControlTemplateDeleteArguments arguments,
			[Injection] IServiceBaseHandler<ControlTemplateDeleteArguments> handler
		)
		{
			var result = await handler.ExecuteAsync(arguments);
			return result;
		}
		#endregion DELETE /
	}
}
