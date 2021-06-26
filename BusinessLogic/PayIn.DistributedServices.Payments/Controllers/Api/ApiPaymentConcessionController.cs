using PayIn.Application.Dto.Payments.Arguments;
using PayIn.Application.Dto.Payments.Results;
using PayIn.Domain.Security;
using PayIn.Web.Security;
using System.Threading.Tasks;
using System.Web.Http;
using Xp.Application.Dto;
using Xp.DistributedServices.Filters;
using Xp.DistributedServices.ModelBinder;

namespace PayIn.DistributedServices.Payments.Controllers.Api
{
	[HideSwagger]
	[RoutePrefix("Api/PaymentConcession")]
	[XpAuthorize(
		ClientIds = AccountClientId.Web,
		Roles = AccountRoles.User + "," + AccountRoles.CommercePayment + "," + AccountRoles.Superadministrator
	)]
	public class ApiPaymentConcessionController : ApiController
	{
		#region GET /
		[HttpGet]
		[Route]
		[XpAuthorize(
			ClientIds = AccountClientId.Web,
			Roles = AccountRoles.Commerce + "," + AccountRoles.CommercePayment + "," + AccountRoles.Superadministrator
		)]
		public async Task<ResultBase<PaymentConcessionGetAllResult>> GetAll(
			[FromUri] PaymentConcessionGetAllArguments arguments,
			[Injection] IQueryBaseHandler<PaymentConcessionGetAllArguments, PaymentConcessionGetAllResult> handler
		)
		{
			var result = await handler.ExecuteAsync(arguments);
			return result;
		}
		#endregion GET /

		#region POST /Create
		[HttpPost]
		[Route("Create")]
		public async Task<dynamic> Create(
			PaymentConcessionCreateArguments arguments,
			[Injection] IServiceBaseHandler<PaymentConcessionCreateArguments> handler
		)
		{
			var result = (await handler.ExecuteAsync(arguments));
			return new {
				Id = result.Id,
				ServiceConcessionId = result.ConcessionId
			};
		}
		#endregion POST /Create

		#region GET /{id:int}
		[HttpGet]
		[Route("{id:int}")]		
		public async Task<ResultBase<PaymentConcessionGetResult>> Get(
			[FromUri] PaymentConcessionGetArguments argument,
			[Injection] IQueryBaseHandler<PaymentConcessionGetArguments, PaymentConcessionGetResult> handler)
		{
			var result = await handler.ExecuteAsync(argument);
			return result;
		}
		#endregion GET /{id:int}

		#region PUT /{id:int}
		[HttpPut]
		[Route("{id:int}")]
		[XpAuthorize(Roles = AccountRoles.Superadministrator)]
		public async Task<dynamic> Put(
			PaymentConcessionUpdateArguments arguments,
		[Injection] IServiceBaseHandler<PaymentConcessionUpdateArguments> handler
		)
		{
			var item = await handler.ExecuteAsync(arguments);
			return new { item.Id };

		}
		#endregion PUT /{id:int}

		#region PUT /UpdateCommerce/{id:int}
		[HttpPut]
		[Route("UpdateCommerce/{id:int}")]
		[XpAuthorize(Roles = AccountRoles.CommercePayment)]
		public async Task<dynamic> Put(
			PaymentConcessionUpdateCommerceArguments command,
		[Injection] IServiceBaseHandler<PaymentConcessionUpdateCommerceArguments> handler
		)
		{
			var item = await handler.ExecuteAsync(command);
			return new { item.Id };

		}
		#endregion PUT /UpdateCommerce/{id:int}

		#region GET /GetCommerce/{id:int}
		[HttpGet]
		[Route("GetCommerce/{id:int}")]
		[XpAuthorize(
			ClientIds = AccountClientId.Web,
			Roles = AccountRoles.CommercePayment
		)]
		public async Task<ResultBase<PaymentConcessionGetCommerceResult>> Get(
			[FromUri] PaymentConcessionGetCommerceArguments argument,
			[Injection] IQueryBaseHandler<PaymentConcessionGetCommerceArguments, PaymentConcessionGetCommerceResult> handler)
		{
			var result = await handler.ExecuteAsync(argument);
			return result;
		}
		#endregion GET /GetCommerce/{id:int}

		#region GET /Selector/{filter?}
		[HttpGet]
		[Route("Selector/{filter?}")]
		public async Task<ResultBase<ApiPaymentConcessionGetSelectorResult>> RetrieveSelector(
			[FromUri] ApiPaymentConcessionGetSelectorArguments arguments,
			[Injection] IQueryBaseHandler<ApiPaymentConcessionGetSelectorArguments, ApiPaymentConcessionGetSelectorResult> handler,
            string filter = ""
        )
		{
            arguments.Filter = filter;
			var result = await handler.ExecuteAsync(arguments);
			return result;
		}
		#endregion GET /Selector/{filter?}

		#region GET /SelectorConcession/{filter?}
		[HttpGet]
		[Route("SelectorConcession/{filter?}")]
		public async Task<ResultBase<PromotionGetPaymentConcessionSelectorResult>> RetrieveSelector2(
			string filter,
			[FromUri]  PromotionGetPaymentConcessionSelectorArguments command,
			[Injection] IQueryBaseHandler<PromotionGetPaymentConcessionSelectorArguments, PromotionGetPaymentConcessionSelectorResult> handler
		)
		{
			var result = await handler.ExecuteAsync(command);
			return result;
		}
		#endregion GET /SelectorConcession/{filter?}

		#region PUT /BannerImageCrop
		[HttpPut]
		[Route("BannerImageCrop/{id:int}")]
		public async Task<dynamic> BannerImageCrop(
			int id,
			PaymentConcessionCreatePhotoUrlArguments arguments,
			[Injection] IServiceBaseHandler<PaymentConcessionCreatePhotoUrlArguments> handler)
		{
			arguments.Id = id;
			var item = await handler.ExecuteAsync(arguments);
			return new { item.Id };
		}
		#endregion POST /BannerImageCrop
	}
}
