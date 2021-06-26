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
	[RoutePrefix("Api/ProductFamily")]
	[XpAuthorize(
		ClientIds = AccountClientId.Web,
		Roles = AccountRoles.Superadministrator + "," + AccountRoles.Operator + "," +AccountRoles.Commerce + "," + AccountRoles.CommercePayment
		
	)]
	public class ProductFamilyController : ApiController
	{
        #region GET /
        [HttpGet]
		[Route]
		[XpAuthorize(
			ClientIds = AccountClientId.Web,
			Roles = AccountRoles.Commerce + "," + AccountRoles.CommercePayment
		)]
		public async Task<ResultBase<ProductFamilyGetAllResult>> GetAll(
			[FromUri] ProductFamilyGetAllArguments arguments,
			[Injection] IQueryBaseHandler<ProductFamilyGetAllArguments, ProductFamilyGetAllResult> handler
		)
		{
			var result = await handler.ExecuteAsync(arguments);
			return result;
		}
		#endregion GET /

		#region GET /{id:int}
		[HttpGet]
		[Route("{id:int}")]
		public async Task<ResultBase<ProductFamilyGetResult>> Get(
			[FromUri] ProductFamilyGetArguments argument,
			[Injection] IQueryBaseHandler<ProductFamilyGetArguments, ProductFamilyGetResult> handler)
		{
			var result = await handler.ExecuteAsync(argument);
			return result;
		}
		#endregion GET /{id:int}

		#region GET /RetrieveSelector/{filter?}
		[HttpGet]
        [Route("RetrieveSelector/{filter?}")]
        public async Task<ResultBase<ProductFamilyGetSelectorResult>> RetrieveSelector(
			string filter,
            [FromUri] ProductFamilyGetSelectorArguments argument,
            [Injection] IQueryBaseHandler<ProductFamilyGetSelectorArguments, ProductFamilyGetSelectorResult> handler
        )
        {
            var result = await handler.ExecuteAsync(argument);
            return result;
        }
		#endregion GET /RetrieveSelector/{filter?}

		#region PUT /{id:int}
		[HttpPut]
		[Route("{id:int}")]
		[XpAuthorize(Roles = AccountRoles.Superadministrator + "," + AccountRoles.Commerce + "," + AccountRoles.CommercePayment)]
		public async Task<dynamic> Put(
			int id,
            ProductFamilyUpdateArguments arguments,
			[Injection] IServiceBaseHandler<ProductFamilyUpdateArguments> handler)
		{
			var item = await handler.ExecuteAsync(arguments);
			return new { item.Id };

		}
		#endregion PUT /{id:int}

        #region POST
        [HttpPost]
        [Route("Create/{id:int?}")]
        public async Task<dynamic> Post(
            ProductFamilyCreateArguments arguments,
            [FromUri] int? id = 0,
            [Injection] IServiceBaseHandler<ProductFamilyCreateArguments> handler = null
        )
        {
            arguments.Id = id == 0 ? null : id;
            var item = await handler.ExecuteAsync(arguments);
            return new { item.Id };
        }
        #endregion POST

        #region POST /ImageCrop		
        [HttpPut]
		[Route("ImageCrop/{id:int}")]
		public async Task<dynamic> PurseImage(
			int id,
			ProductFamilyUpdatePhotoArguments arguments,
			[Injection] IServiceBaseHandler<ProductFamilyUpdatePhotoArguments> handler)
		{
			arguments.Id = id;
			var item = await handler.ExecuteAsync(arguments);
			return new { item.Id };
		}
		#endregion POST api/Account/ImageCrop

		#region DELETE /{id:int}
		[HttpDelete]
        [Route("{id:int}")]
        [XpAuthorize(
        ClientIds = AccountClientId.Web,
        Roles = AccountRoles.Commerce + "," + AccountRoles.CommercePayment
        )]
        public async Task<dynamic> Delete(
            int id,
            [FromUri] ProductFamilyDeleteArguments command,
            [Injection] IServiceBaseHandler<ProductFamilyDeleteArguments> handler
            )
        {
            var result = await handler.ExecuteAsync(command);
            return result;
        }
		#endregion DELETE /{id:int}

		#region PUT /IsVisible/{id:int}
		[HttpPut]
		[Route("IsVisible/{id:int}")]
		[XpAuthorize(Roles = AccountRoles.Superadministrator + "," + AccountRoles.CommercePayment + "," + AccountRoles.PaymentWorker)]
		public async Task<dynamic> IsVisible(
			int id,
			ProductFamilyIsVisibleArguments arguments,
			[Injection] IServiceBaseHandler<ProductFamilyIsVisibleArguments> handler)
		{
			arguments.Id = id;
			var result = await handler.ExecuteAsync(arguments);
			return result;
		}
		#endregion PUT /IsVisible/{id:int}
	}
}

