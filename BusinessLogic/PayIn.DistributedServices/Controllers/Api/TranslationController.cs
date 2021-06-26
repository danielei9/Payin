using PayIn.Application.Dto.Arguments;
using PayIn.Application.Dto.Payments.Results.Translation;
using PayIn.Application.Dto.Results;
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
	[RoutePrefix("Api/Translation")]
	public class TranslationController : ApiController
	{
		//#region GET /{filter?}
		//[HttpGet]
		//[Route]
		//[XpAuthorize(
		//	ClientIds = AccountClientId.Web,
		//	Roles = AccountRoles.User
		//)]
		//public async Task<ResultBase<TranslationGetResult>> GetAll(
		//	[FromUri] TranslationGetArguments arguments,
		//	[Injection] IQueryBaseHandler<TranslationGetArguments, TranslationGetResult> handler
		//)
		//{
		//	var result = await handler.ExecuteAsync(arguments);
		//	return result;
		//}
		//#endregion GET /{filter?}

		#region GET ?{params}
		[HttpGet]
		[Route]
		[XpAuthorize(
			ClientIds = AccountClientId.Web,
			Roles = AccountRoles.Superadministrator + "," + AccountRoles.Operator + "," + AccountRoles.Commerce + "," + AccountRoles.CommercePayment
		)]
		public async Task<ResultBase<TranslationGetResult>> Get(
			[FromUri] TranslationGetArguments arguments,
			[Injection] IQueryBaseHandler<TranslationGetArguments, TranslationGetResult> handler)
		{
			var result = await handler.ExecuteAsync(arguments);
			return result;
		}
		#endregion GET ?{params}

		#region GET /{id:int}
		[HttpGet]
		[Route("{id:int}")]
		[XpAuthorize(
			ClientIds = AccountClientId.Web,
			Roles = AccountRoles.Superadministrator + "," + AccountRoles.Operator + "," + AccountRoles.Commerce + "," + AccountRoles.CommercePayment
		)]
		public async Task<ResultBase<TranslationGetResult>> Get(
			int id,
			[FromUri] TranslationGetArguments arguments,
			[Injection] IQueryBaseHandler<TranslationGetArguments, TranslationGetResult> handler)
		{
			arguments.Id = id;
			var result = await handler.ExecuteAsync(arguments);
			return result;
		}
		#endregion GET /{id:int}

		#region PUT /{id:int}
		[HttpPut]
		[Route("{id:int}")]
		[XpAuthorize(
			ClientIds = AccountClientId.Web,
			Roles = AccountRoles.Superadministrator + "," + AccountRoles.Operator + "," + AccountRoles.Commerce + "," + AccountRoles.CommercePayment
		)]
		public async Task<dynamic> Put(
			int id,
			TranslationUpdateArguments arguments,
			[Injection] IServiceBaseHandler<TranslationUpdateArguments> handler
		)
		{
			arguments.Id = id;
			var item = await handler.ExecuteAsync(arguments);
			return new { item.Id };
		}
		#endregion PUT /{id:int}

		#region POST /{id:int}
		[HttpPost]
		[Route]
		[XpAuthorize(
			ClientIds = AccountClientId.Web,
			Roles = AccountRoles.Superadministrator + "," + AccountRoles.Operator + "," + AccountRoles.Commerce + "," + AccountRoles.CommercePayment
		)]
		public async Task<dynamic> Post(
			TranslationCreateArguments arguments,
			[Injection] IServiceBaseHandler<TranslationCreateArguments> handler
		)
		{
			var item = await handler.ExecuteAsync(arguments);
			return new { item.Id };
		}
		#endregion POST /{id:int}

		#region DELETE /{id:int}
		[HttpDelete]
		[Route("{id:int}")]
		[XpAuthorize(
			ClientIds = AccountClientId.Web,
			Roles = AccountRoles.Superadministrator + "," + AccountRoles.Operator + "," + AccountRoles.Commerce + "," + AccountRoles.CommercePayment
		)]
		public async Task<dynamic> Delete(
		//int id,
		[FromUri] TranslationDeleteArguments arguments,
		[Injection] IServiceBaseHandler<TranslationDeleteArguments> handler
		)
		{
			//arguments.Id = id;
			var result = await handler.ExecuteAsync(arguments);
			return result;
		}
		#endregion DELETE /{id:int}
	}
}
