using PayIn.Application.Dto.Arguments.ControlItem;
using PayIn.Application.Dto.Results.ControlItem;
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
	[RoutePrefix("Api/ControlItem")]
	[XpAuthorize(
		ClientIds = AccountClientId.Web,
		Roles = AccountRoles.Operator
	)]
	public class ControlItemController : ApiController
	{
		#region GET /{id:int}
		[HttpGet]
		[Route("{id:int}")]
		public async Task<ResultBase<ControlItemGetResult>> Get(
			[FromUri] ControlItemGetArguments arguments,
			[Injection] IQueryBaseHandler<ControlItemGetArguments, ControlItemGetResult> handler
		)
		{
			var result = await handler.ExecuteAsync(arguments);
			return result;
		}
		#endregion GET /{id:int}

		#region GET /{filter?}
		[HttpGet]
		[Route("{filter?}")]
		public async Task<ResultBase<ControlItemGetAllResult>> Get(
			[FromUri] ControlItemGetAllArguments arguments,
			[Injection] IQueryBaseHandler<ControlItemGetAllArguments, ControlItemGetAllResult> handler
		)
		{
			var result = await handler.ExecuteAsync(arguments);
			return result;
		}
		#endregion GET /{filter?}

		#region GET /Selector/{filter?}
		[HttpGet]
		[XpAuthorize(Roles = AccountRoles.Operator)]
		[Route("Selector/{filter?}")]
		public async Task<ResultBase<ControlItemGetSelectorResult>> Selector(
			string filter,
			[Injection] IQueryBaseHandler<ControlItemGetSelectorArguments, ControlItemGetSelectorResult> handler
		)
		{
			var result = await handler.ExecuteAsync(new ControlItemGetSelectorArguments(filter));
			return result;
		}
		#endregion GET /Selector/{filter?}

		#region POST /
		[HttpPost]
		[Route("")]
		public async Task<dynamic> Post(
			ControlItemCreateArguments command,
			[Injection] IServiceBaseHandler<ControlItemCreateArguments> handler
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
			ControlItemUpdateArguments arguments,
			[Injection] IServiceBaseHandler<ControlItemUpdateArguments> handler
		)
		{
			arguments.Id = id;
			var item = await handler.ExecuteAsync(arguments);
			return new { item.Id };
		}
		#endregion PUT /{id:int}

		#region GET /Name/{id:int}
		[HttpGet]
		[Route("Name/{id:int}")]
		public async Task<ResultBase<ControlItemGetNameResult>> GetName(
			[FromUri] ControlItemGetNameArguments arguments,
			[Injection] IQueryBaseHandler<ControlItemGetNameArguments, ControlItemGetNameResult> handler
		)
		{
			var result = await handler.ExecuteAsync(arguments);
			return result;
		}
		#endregion GET /Name/{id:int}

		#region PUT /AddTag/{id:int}
		[HttpPut]
		[Route("AddTag/{id:int}")]
		public async Task<dynamic> AddTag(
				ControlItemAddTagArguments arguments,
				[Injection] IServiceBaseHandler<ControlItemAddTagArguments> handler
		)
		{
			var item = await handler.ExecuteAsync(arguments);
			return new { item.Id };
		}
		#endregion PUT /AddTag/{id:int}

		#region PUT /RemoveTag/{id:int}
		[HttpPut]
		[Route("RemoveTag/{id:int}")]
		public async Task<dynamic> RemoveTag(
			int id,
			ControlItemRemoveTagArguments arguments,
			[Injection] IServiceBaseHandler<ControlItemRemoveTagArguments> handler
		)
		{
			var item = await handler.ExecuteAsync(arguments);
			return new { item.Id };
		}
		#endregion PUT /RemoveTag/{id:int}

		#region DELETE /{id:int}
		[HttpDelete]
		[Route("{id:int}")]
		public async Task<dynamic> Delete(
			[FromUri] ControlItemDeleteArguments arguments,
			[Injection] IServiceBaseHandler<ControlItemDeleteArguments> handler
		)
		{
			var result = await handler.ExecuteAsync(arguments);
			return result;
		}
		#endregion DELETE /{id:int}
	}
}
