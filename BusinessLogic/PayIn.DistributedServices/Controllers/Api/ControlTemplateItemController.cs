using PayIn.Application.Dto.Arguments.ControlTemplateItem;
using PayIn.Application.Dto.Results.ControlTemplateItem;
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
	[RoutePrefix("Api/ControlTemplateItem")]
	[XpAuthorize(
		ClientIds = AccountClientId.Web,
		Roles = AccountRoles.Operator
	)]
	public class ControlTemplateItemController : ApiController
	{
		#region GET /Template
		[HttpGet]
		[Route("Template")]
		public async Task<ResultBase<ControlTemplateItemGetTemplateResult>> Get(
			[FromUri] ControlTemplateItemGetTemplateArguments arguments,
			[Injection] IQueryBaseHandler<ControlTemplateItemGetTemplateArguments, ControlTemplateItemGetTemplateResult> handler
		)
		{
			var result = await handler.ExecuteAsync(arguments);
			return result;
		}
		#endregion GET /Template

		#region GET /{id:int}
		[HttpGet]
		[Route("{id:int}")]
		public async Task<ResultBase<ControlTemplateItemGetResult>> Get(
			[FromUri] ControlTemplateItemGetArguments arguments,
			[Injection] IQueryBaseHandler<ControlTemplateItemGetArguments, ControlTemplateItemGetResult> handler
		)
		{
			var result = await handler.ExecuteAsync(arguments);
			return result;
		}
		#endregion GET /{id:int}

		#region POST /
		[HttpPost]
		[Route]
		public async Task<dynamic> Post(
			ControlTemplateItemCreateArguments arguments,
			[Injection] IServiceBaseHandler<ControlTemplateItemCreateArguments> handler
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
			ControlTemplateItemUpdateArguments arguments,
			[Injection] IServiceBaseHandler<ControlTemplateItemUpdateArguments> handler
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
			[FromUri] ControlTemplateItemDeleteArguments arguments,
			[Injection] IServiceBaseHandler<ControlTemplateItemDeleteArguments> handler
		)
		{
			var result = await handler.ExecuteAsync(arguments);
			return result;
		}
		#endregion DELETE /
	}
}
