using PayIn.Application.Dto.Arguments.ControlTemplateCheck;
using PayIn.Application.Dto.Results;
using PayIn.Application.Dto.Results.ControlTemplateCheck;
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
	[RoutePrefix("Api/ControlTemplateCheck")]
	[XpAuthorize(
		ClientIds = AccountClientId.Web,
		Roles = AccountRoles.Operator
	)]
	public class ControlTemplateCheckController : ApiController
	{
		#region GET /Template
		[HttpGet]
		[Route("Template")]
		public async Task<ResultBase<ControlTemplateCheckGetTemplateResult>> Get(
			[FromUri] ControlTemplateCheckGetTemplateArguments arguments,
			[Injection] IQueryBaseHandler<ControlTemplateCheckGetTemplateArguments, ControlTemplateCheckGetTemplateResult> handler
		)
		{
			var result = await handler.ExecuteAsync(arguments);
			return result;
		}
		#endregion GET /Template

		#region GET /{id:int}
		[HttpGet]
		[Route("{id:int}")]
		public async Task<ResultBase<ControlTemplateCheckGetResult>> Get(
			[FromUri] ControlTemplateCheckGetArguments arguments,
			[Injection] IQueryBaseHandler<ControlTemplateCheckGetArguments, ControlTemplateCheckGetResult> handler
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
			ControlTemplateCheckCreateArguments command,
			[Injection] IServiceBaseHandler<ControlTemplateCheckCreateArguments> handler
		)
		{
			var item = await handler.ExecuteAsync(command);
			return new { item.Id };
		}
		#endregion POST /

		#region PUT /
		[HttpPut]
		[Route("{id:int}")]
		public async Task<dynamic> Put(
			ControlTemplateCheckUpdateArguments arguments,
			[Injection] IServiceBaseHandler<ControlTemplateCheckUpdateArguments> handler
		)
		{
			var item = await handler.ExecuteAsync(arguments);
			return new { item.Id };
		}
		#endregion PUT /

		#region DELETE /{id:int}
		[HttpDelete]
		[Route("{id:int}")]
		public async Task<dynamic> Delete(
			[FromUri] ControlTemplateCheckDeleteArguments arguments,
			[Injection] IServiceBaseHandler<ControlTemplateCheckDeleteArguments> handler
		)
		{
			var result = await handler.ExecuteAsync(arguments);
			return result;
		}
		#endregion DELETE /{id:int}
	}
}
