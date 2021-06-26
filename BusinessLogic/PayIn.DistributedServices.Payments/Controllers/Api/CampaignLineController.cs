using PayIn.Application.Dto.Payments.Arguments;
using PayIn.Application.Dto.Payments.Arguments.CampaignLine;
using PayIn.Application.Dto.Payments.Results;
using PayIn.Application.Dto.Payments.Results.CampaignLine;
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
	[RoutePrefix("Api/CampaignLine")]
	[XpAuthorize(
		ClientIds = AccountClientId.Web,
		Roles = AccountRoles.CommercePayment + "," + AccountRoles.PaymentWorker + "," + AccountRoles.User
	)]
	public class CampaignLineController : ApiController
	{
		#region GET /{id:int}
		[HttpGet]
		[Route("{id:int}")]
		public async Task<ResultBase<ApiCampaignLineGetAllResult>> GetAll(
			[FromUri] ApiCampaignLineGetAllArguments arguments,
			[Injection] IQueryBaseHandler<ApiCampaignLineGetAllArguments, ApiCampaignLineGetAllResult> handler
		)
		{
			var result = await handler.ExecuteAsync(arguments);
			return result;
		}
        #endregion GET /{id:int}

        #region GET /View/{id:int}
        [HttpGet]
		[Route("View/{id:int}")]
		public async Task<ResultBase<CampaignLineGetResult>> Get(
			[FromUri] CampaignLineGetArguments query,
	    	[Injection] IQueryBaseHandler<CampaignLineGetArguments, CampaignLineGetResult> handler
		)
		{
			var result = await handler.ExecuteAsync(query);
			return result;
		}
		#endregion  GET /View/{id:int}

		#region PUT /{id:int}
		[HttpPut]
		[Route("{id:int}")]
		public async Task<dynamic> Put(
			int Id,
			CampaignLineUpdateArguments command,
			[Injection] IServiceBaseHandler<CampaignLineUpdateArguments> handler
		)
		{
			command.Id = Id;
			var item = await handler.ExecuteAsync(command);
			return new { item.Id };
		}
		#endregion PUT /{id:int}
		
		#region POST /
		[HttpPost]
		[Route("{id:int}")]
		public async Task<dynamic> Post(
			CampaignLineCreateArguments arguments,
			[Injection] IServiceBaseHandler<CampaignLineCreateArguments> handler
		)
		{
			var item = await handler.ExecuteAsync(arguments);
			return new { item.Id };
		}
		#endregion POST /

		#region Delete /{id:int}
		[HttpDelete]
		[Route("{id:int}")]
		public async Task<dynamic> Delete(
			[FromUri] CampaignLineDeleteArguments arguments,
			[Injection] IServiceBaseHandler<CampaignLineDeleteArguments> handler
		)
		{
			var result = await handler.ExecuteAsync(arguments);
			return result;
		}
		#endregion Delete /{id:int}

		#region GET /Selector/{filter?}
		[HttpGet]		
		[Route("RetrieveSelector/{filter?}")]
		public async Task<ResultBase<CampaignLineGetSelectorResult>> Selector(
			string filter,
			[FromUri] CampaignLineGetSelectorArguments arguments,
			[Injection] IQueryBaseHandler<CampaignLineGetSelectorArguments, CampaignLineGetSelectorResult> handler
		)
		{
			var result = await handler.ExecuteAsync(arguments);
			return result;
		}
		#endregion GET /Selector/{filter?}

		#region GET /ProductRetrieveSelector/{filter?}
		[HttpGet]
		[Route("ProductRetrieveSelector/{filter?}")]
		public async Task<ResultBase<CampaignLineProductGetSelectorResult>> Selector(
			string filter,
			[FromUri] CampaignLineProductGetSelectorArguments arguments,
			[Injection] IQueryBaseHandler<CampaignLineProductGetSelectorArguments, CampaignLineProductGetSelectorResult> handler
		)
		{
			var result = await handler.ExecuteAsync(arguments);
			return result;
		}
		#endregion GET /ProductRetrieveSelector/{filter?}

		#region GET /ProductFamilyRetrieveSelector/{filter?}
		[HttpGet]
		[Route("ProductFamilyRetrieveSelector/{filter?}")]
		public async Task<ResultBase<CampaignLineProductFamilyGetSelectorResult>> Selector(
			string filter,
			[FromUri] CampaignLineProductFamilyGetSelectorArguments arguments,
			[Injection] IQueryBaseHandler<CampaignLineProductFamilyGetSelectorArguments, CampaignLineProductFamilyGetSelectorResult> handler
		)
		{
			var result = await handler.ExecuteAsync(arguments);
			return result;
		}
		#endregion GET /ProductFamilyRetrieveSelector/{filter?}
		
		#region GET /Product/{id:int}
		[HttpGet]
		[Route("Product/{id:int}")]
		[XpAuthorize(
			ClientIds = AccountClientId.Web,
			Roles = AccountRoles.Commerce + "," + AccountRoles.CommercePayment
		)]
		public async Task<ResultBase<CampaignLineGetByProductResult>> Product(
			int id,
			[FromUri] CampaignLineGetByProductArguments arguments,
			[Injection] IQueryBaseHandler<CampaignLineGetByProductArguments, CampaignLineGetByProductResult> handler
		)
		{
			arguments.Id = id;
			var result = await handler.ExecuteAsync(arguments);
			return result;
		}
		#endregion GET /Product/{id:int}

		#region POST /Product/{id:int}
		[HttpPost]
		[Route("Product/{id:int}")]
		public async Task<dynamic> AddProduct(
			int id,
			CampaignLineAddProductArguments arguments,
			[Injection] IServiceBaseHandler<CampaignLineAddProductArguments> handler = null
		)
		{
			arguments.Id = id;
			var item = await handler.ExecuteAsync(arguments);
			return new { item.Id };
		}
		#endregion POST /Product/{id:int}

		#region DELETE /Product/{id:int}
		[HttpDelete]
		[Route("Product/{id:int}")]
		[XpAuthorize(
			ClientIds = AccountClientId.Web,
			Roles = AccountRoles.Commerce + "," + AccountRoles.CommercePayment
		)]
		public async Task<dynamic> Remove(
			[FromBody] CampaignLineRemoveProductArguments arguments,
			[Injection] IServiceBaseHandler<CampaignLineRemoveProductArguments> handler,
			int id
		)
		{
			arguments.Id = id;
			var result = await handler.ExecuteAsync(arguments);
			return result;
		}
		#endregion DELETE /Product/{id:int}

		#region GET /ProductFamily/{id:int}
		[HttpGet]
		[Route("ProductFamily/{id:int}")]
		[XpAuthorize(
			ClientIds = AccountClientId.Web,
			Roles = AccountRoles.Commerce + "," + AccountRoles.CommercePayment
		)]
		public async Task<ResultBase<CampaignLineGetByProductFamilyResult>> Product(
			int id,
			[FromUri] CampaignLineGetByProductFamilyArguments arguments,
			[Injection] IQueryBaseHandler<CampaignLineGetByProductFamilyArguments, CampaignLineGetByProductFamilyResult> handler
		)
		{
			arguments.Id = id;
			var result = await handler.ExecuteAsync(arguments);
			return result;
		}
		#endregion GET /ProductFamily/{id:int}

		#region POST /ProductFamily/{id:int}
		[HttpPost]
		[Route("ProductFamily/{id:int}")]
		public async Task<dynamic> AddProduct(
			int id,
			CampaignLineAddProductFamilyArguments arguments,
			[Injection] IServiceBaseHandler<CampaignLineAddProductFamilyArguments> handler = null
		)
		{
			arguments.Id = id;
			var item = await handler.ExecuteAsync(arguments);
			return new { item.Id };
		}
		#endregion POST /ProductFamily/{id:int}

		#region DELETE /ProductFamily/{id:int}
		[HttpDelete]
		[Route("ProductFamily/{id:int}")]
		[XpAuthorize(
			ClientIds = AccountClientId.Web,
			Roles = AccountRoles.Commerce + "," + AccountRoles.CommercePayment
		)]
		public async Task<dynamic> Remove(
			[FromBody] CampaignLineRemoveProductFamilyArguments arguments,
			[Injection] IServiceBaseHandler<CampaignLineRemoveProductFamilyArguments> handler,
			int id
		)
		{
			arguments.Id = id;
			var result = await handler.ExecuteAsync(arguments);
			return result;
		}
		#endregion DELETE /ProductFamily/{id:int}

		#region GET /EntranceType/{id:int}
		[HttpGet]
		[Route("EntranceType/{id:int}")]
		[XpAuthorize(
			ClientIds = AccountClientId.Web,
			Roles = AccountRoles.Commerce + "," + AccountRoles.CommercePayment
		)]
		public async Task<ResultBase<ApiCampaignLineGetByEntranceTypeResult>> EntranceType(
			int id,
			[FromUri] ApiCampaignLineGetByEntranceTypeArguments arguments,
			[Injection] IQueryBaseHandler<ApiCampaignLineGetByEntranceTypeArguments, ApiCampaignLineGetByEntranceTypeResult> handler
		)
		{
			arguments.CampaignLineId = id;
			var result = await handler.ExecuteAsync(arguments);
			return result;
		}
		#endregion GET /EntranceType/{id:int}

		#region POST /EntranceType/{id:int}
		[HttpPost]
		[Route("EntranceType/{id:int}")]
		public async Task<dynamic> AddEntranceType(
			int id,
			CampaignLineAddEntranceTypeArguments arguments,
			[Injection] IServiceBaseHandler<CampaignLineAddEntranceTypeArguments> handler = null
		)
		{
			arguments.Id = id;
			var item = await handler.ExecuteAsync(arguments);
			return new { item.Id };
		}
		#endregion POST /EntranceType/{id:int}

		#region DELETE /EntranceType/{id:int}
		[HttpDelete]
		[Route("EntranceType/{id:int}")]
		[XpAuthorize(
			ClientIds = AccountClientId.Web,
			Roles = AccountRoles.Commerce + "," + AccountRoles.CommercePayment
		)]
		public async Task<dynamic> RemoveEntranceType(
			[FromBody] CampaignLineRemoveEntranceTypeArguments arguments,
			[Injection] IServiceBaseHandler<CampaignLineRemoveEntranceTypeArguments> handler,
			int id
		)
		{
			arguments.Id = id;
			var result = await handler.ExecuteAsync(arguments);
			return result;
		}
		#endregion DELETE /EntranceType/{id:int}

		#region GET /ServiceUserRetrieveSelector/{filter?}
		[HttpGet]
		[Route("ServiceUserRetrieveSelector/{filter?}")]
		public async Task<ResultBase<CampaignLineServiceUserGetSelectorResult>> SelectorServiceUse(
			string filter,
			[FromUri] CampaignLineServiceUserGetSelectorArguments arguments,
			[Injection] IQueryBaseHandler<CampaignLineServiceUserGetSelectorArguments, CampaignLineServiceUserGetSelectorResult> handler
		)
		{
			var result = await handler.ExecuteAsync(arguments);
			return result;
		}
		#endregion GET /ServiceUserRetrieveSelector/{filter?}

		#region GET /ServiceUser/{id:int}
		[HttpGet]
		[Route("ServiceUser/{id:int}")]
		[XpAuthorize(
			ClientIds = AccountClientId.Web,
			Roles = AccountRoles.Commerce + "," + AccountRoles.CommercePayment
		)]
		public async Task<ResultBase<CampaignLineGetByServiceUserResult>> ServiceUser(
			int id,
			[FromUri] CampaignLineGetByServiceUserArguments arguments,
			[Injection] IQueryBaseHandler<CampaignLineGetByServiceUserArguments, CampaignLineGetByServiceUserResult> handler
		)
		{
			arguments.Id = id;
			var result = await handler.ExecuteAsync(arguments);
			return result;
		}
		#endregion GET /ServiceUser/{id:int}

		#region POST /ServiceUser/{id:int}
		[HttpPost]
		[Route("ServiceUser/{id:int}")]
		public async Task<dynamic> AddServiceUser(
			[FromUri] int id,
			[FromBody] CampaignLineAddServiceUserArguments arguments,
			[Injection] IServiceBaseHandler<CampaignLineAddServiceUserArguments> handler = null
		)
		{
			arguments.Id = id;
			var item = await handler.ExecuteAsync(arguments);
			return new { item.Id };
		}
		#endregion POST /ServiceUser/{id:int}

		#region PUT /RemoveServiceUser/{id:int}
		[HttpPut]
		[Route("RemoveServiceUser/{id:int}")]
		[XpAuthorize(
			ClientIds = AccountClientId.Web,
			Roles = AccountRoles.Commerce + "," + AccountRoles.CommercePayment
		)]
		public async Task<dynamic> Remove(
			[FromUri] int id,
			[FromBody] CampaignLineRemoveServiceUserArguments arguments,
			[Injection] IServiceBaseHandler<CampaignLineRemoveServiceUserArguments> handler
		)
		{
			arguments.Id = id;
			var result = await handler.ExecuteAsync(arguments);
			return result;
		}
		#endregion PUT /RemoveServiceUser/{id:int}

		#region GET /ServiceGroupRetrieveSelector/{filter?}
		[HttpGet]
		[Route("ServiceGroupRetrieveSelector/{filter?}")]
		public async Task<ResultBase<CampaignLineServiceGroupGetSelectorResult>> SelectorServiceGroup(
			string filter,
			[FromUri] CampaignLineServiceGroupGetSelectorArguments arguments,
			[Injection] IQueryBaseHandler<CampaignLineServiceGroupGetSelectorArguments, CampaignLineServiceGroupGetSelectorResult> handler
		)
		{
			var result = await handler.ExecuteAsync(arguments);
			return result;
		}
		#endregion GET /ServiceGroupRetrieveSelector/{filter?}

		#region GET /ServiceGroup/{id:int}
		[HttpGet]
		[Route("ServiceGroup/{id:int}")]
		[XpAuthorize(
			ClientIds = AccountClientId.Web,
			Roles = AccountRoles.Commerce + "," + AccountRoles.CommercePayment
		)]
		public async Task<ResultBase<CampaignLineGetByServiceGroupResult>> ServiceGroup(
			int id,
			[FromUri] CampaignLineGetByServiceGroupArguments arguments,
			[Injection] IQueryBaseHandler<CampaignLineGetByServiceGroupArguments, CampaignLineGetByServiceGroupResult> handler
		)
		{
			arguments.Id = id;
			var result = await handler.ExecuteAsync(arguments);
			return result;
		}
		#endregion GET /ServiceGroup/{id:int}

		#region POST /ServiceGroup/{id:int}
		[HttpPost]
		[Route("ServiceGroup/{id:int}")]
		public async Task<dynamic> AddServiceGroup(
			int id,
			CampaignLineAddServiceGroupArguments arguments,
			[Injection] IServiceBaseHandler<CampaignLineAddServiceGroupArguments> handler = null
		)
		{
			arguments.Id = id;
			var item = await handler.ExecuteAsync(arguments);
			return new { item.Id };
		}
		#endregion POST /ServiceGroup/{id:int}

		#region PUT /RemoveServiceGroup/{id:int}
		[HttpPut]
		[Route("ServiceGroup/{id:int}")]
		[XpAuthorize(
			ClientIds = AccountClientId.Web,
			Roles = AccountRoles.Commerce + "," + AccountRoles.CommercePayment
		)]
		public async Task<dynamic> Remove(
			[FromUri] int id,
			[FromBody] CampaignLineRemoveServiceGroupArguments arguments,
			[Injection] IServiceBaseHandler<CampaignLineRemoveServiceGroupArguments> handler
		)
		{
			arguments.Id = id;
			var result = await handler.ExecuteAsync(arguments);
			return result;
		}
		#endregion PUT /RemoveServiceGroup/{id:int}

	}

}
