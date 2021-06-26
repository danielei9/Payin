using System.Threading.Tasks;
using System.Web.Http;
using PayIn.Web.Security;
using PayIn.Domain.Security;
using Xp.Application.Dto;
using Xp.DistributedServices.ModelBinder;
using PayIn.Application.Dto.Arguments;
using Xp.DistributedServices.Filters;
using PayIn.Application.Dto.Results.SystemCard;
using PayIn.Application.Dto.Arguments.SystemCard;

namespace PayIn.DistributedServices.Controllers.Api
{
	[HideSwagger]
	[RoutePrefix("Api/SystemCard")]
	[XpAuthorize(
		ClientIds = AccountClientId.Web,
		Roles = AccountRoles.Superadministrator + "," + AccountRoles.Commerce + "," + AccountRoles.CommercePayment
	)]
	public class SystemCardController : ApiController
	{
		#region GET /
		[HttpGet]
		[Route]
		[XpAuthorize(Roles = AccountRoles.Superadministrator + "," + AccountRoles.SystemCardOwner + "," + AccountRoles.CommercePayment)]
		public async Task<ResultBase<SystemCardGetAllResult>> GetAll(
		[FromUri] SystemCardGetAllArguments arguments,
		[Injection] IQueryBaseHandler<SystemCardGetAllArguments, SystemCardGetAllResult> handler
		)
		{
			var result = await handler.ExecuteAsync(arguments);
			return result;
		}
		#endregion GET /

		#region GET /{id:int}
		[HttpGet]
		[Route("{id:int}")]
		public async Task<ResultBase<SystemCardGetResult>> Get(
			int id,
			[FromUri] SystemCardGetArguments arguments,
			[Injection] IQueryBaseHandler<SystemCardGetArguments, SystemCardGetResult> handler)
		{
			arguments.Id = id;
			var result = await handler.ExecuteAsync(arguments);
			return result;
		}
		#endregion GET /{id:int}

		#region DELETE /{id:int}
		[HttpDelete]
		[Route("{id:int}")]
		[XpAuthorize(Roles = AccountRoles.Superadministrator)]
		public async Task<dynamic> Delete(
			[FromUri] SystemCardDeleteArguments arguments,
			[Injection] IServiceBaseHandler<SystemCardDeleteArguments> handler
			)
		{
			var result = await handler.ExecuteAsync(arguments);
			return result;
		}
		#endregion DELETE /{id:int}

		#region GET /RetrieveSelector/{filter?}
		[HttpGet]
		[Route("RetrieveSelector/{filter?}")]
		public async Task<ResultBase<SelectorResult>> RetrieveSelector(
			[FromUri] ApiSystemCardGetSelectorArguments command,
			[Injection] IQueryBaseHandler<ApiSystemCardGetSelectorArguments, SelectorResult> handler
		)
		{
			var result = await handler.ExecuteAsync(command);
			return result;
		}
		#endregion GET /RetrieveSelector/{filter?}

		#region POST /
		[HttpPost]
		[Route("")]
		[XpAuthorize(
			ClientIds = AccountClientId.Web,
			Roles = AccountRoles.Superadministrator
		)]
		public async Task<dynamic> Post(
			SystemCardCreateArguments command,
			[Injection] IServiceBaseHandler<SystemCardCreateArguments> handler
		)
		{
			var item = await handler.ExecuteAsync(command);
			return new { item.Id };
		}
		#endregion POST /

		#region PUT /
		[HttpPut]
		[Route("{id:int}")]
		[XpAuthorize(
			ClientIds = AccountClientId.Web,
			Roles = AccountRoles.Superadministrator
		)]
		public async Task<dynamic> Put(
			[FromUri] int id,
			SystemCardUpdateArguments command,
			[Injection] IServiceBaseHandler<SystemCardUpdateArguments> handler
		)
		{
			//command.Id = id;
			var item = await handler.ExecuteAsync(command);
			return new { item.Id };
		}
		#endregion PUT /

		#region POST /ImageCrop/{id:int}
		[HttpPut]
		[Route("ImageCrop/{id:int}")]
		public async Task<dynamic> SystemCardImage(
			int id,
			[FromBody] SystemCardUpdatePhotoArguments arguments,
			[Injection] IServiceBaseHandler<SystemCardUpdatePhotoArguments> handler)
		{
			arguments.Id = id;
			var item = await handler.ExecuteAsync(arguments);
			return new { item.Id };
		}
		#endregion POST /ImageCrop/{id:int}
	}
}
