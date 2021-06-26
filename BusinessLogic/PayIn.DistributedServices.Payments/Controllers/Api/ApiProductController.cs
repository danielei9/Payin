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
	[RoutePrefix("Api/Product")]
	[XpAuthorize(
		ClientIds = AccountClientId.Web,
		Roles = AccountRoles.Superadministrator + "," + AccountRoles.Operator + "," +AccountRoles.Commerce + "," + AccountRoles.CommercePayment
		
	)]
	public class ApiProductController : ApiController
	{
		#region GET /
		[HttpGet]
		[Route]
		[XpAuthorize(
			ClientIds = AccountClientId.Web,
			Roles = AccountRoles.Commerce + "," + AccountRoles.CommercePayment
		)]
		public async Task<ResultBase<ProductGetAllResult>> GetAll(
			[FromUri] ProductGetAllArguments arguments,
			[Injection] IQueryBaseHandler<ProductGetAllArguments, ProductGetAllResult> handler
		)
		{
			var result = await handler.ExecuteAsync(arguments);
			return result;
		}
		#endregion GET /

		#region GET /{id:int}
		[HttpGet]
		[Route("{id:int}")]
		public async Task<ResultBase<ProductGetResult>> Get(
			[FromUri] ProductGetArguments argument,
			[Injection] IQueryBaseHandler<ProductGetArguments, ProductGetResult> handler)
		{
			var result = await handler.ExecuteAsync(argument);
			return result;
		}
		#endregion GET /{id:int}

		#region POST /AddGroup/{id:int}	
		[HttpPost]
		[Route("AddGroup/{id:int}")]
		public async Task<dynamic> AddGroup(
			int id,
			ProductAddGroupArguments arguments,
			[Injection] IServiceBaseHandler<ProductAddGroupArguments> handler)
		{
			arguments.Id = id;
			var item = await handler.ExecuteAsync(arguments);
			return new { item.Id };
		}
		#endregion POST /AddGroup/{id:int}

		#region PUT /RemoveGroup/{id:int}	
		[HttpPut]
		[Route("RemoveGroup/{id:int}")]
		public async Task<dynamic> RemoveGroup(
			int id,
			ProductRemoveGroupArguments arguments,
			[Injection] IServiceBaseHandler<ProductRemoveGroupArguments> handler)
		{
			arguments.Id = id;
			var item = await handler.ExecuteAsync(arguments);
			return null;
		}
		#endregion PUT /RemoveGroup/{id:int}

		#region GET /Groups/{id:int}
		[HttpGet]
		[Route("Groups/{id:int}")]
		public async Task<ResultBase<ProductGroupsGetAllResult>> GetGroups(
			int id,
			[FromUri] ProductGroupsGetAllArguments argument,
			[Injection] IQueryBaseHandler<ProductGroupsGetAllArguments, ProductGroupsGetAllResult> handler)
		{
			argument.Id = id;
			var result = await handler.ExecuteAsync(argument);
			return result;
		}
		#endregion GET /Groups/{id:int}

		#region GET /ProductDashboard/
		[HttpGet]
		[Route("ProductDashboard")]
		[XpAuthorize(
			ClientIds = AccountClientId.Web,
			Roles = AccountRoles.Commerce + "," + AccountRoles.CommercePayment
		)]
		public async Task<ResultBase<ProductGetAllByDashboardResult>> ProductDashboard(
			[FromUri] ProductGetAllByDashboardArguments arguments,
			[Injection] IQueryBaseHandler<ProductGetAllByDashboardArguments, ProductGetAllByDashboardResult> handler
		)
		{
			var result = await handler.ExecuteAsync(arguments);
			return result;
		}
		#endregion GET /ProductDashboard/{filter?}

		#region PUT /{id:int}
		[HttpPut]
		[Route("{id:int}")]
		[XpAuthorize(Roles = AccountRoles.Superadministrator + "," + AccountRoles.Commerce + "," + AccountRoles.CommercePayment)]
		public async Task<dynamic> Put(
			int id,
			ProductUpdateArguments arguments,
			[Injection] IServiceBaseHandler<ProductUpdateArguments> handler)
		{
			arguments.Id = id;
			var item = await handler.ExecuteAsync(arguments);
			return new { item.Id };
		}
        #endregion PUT /{id:int}

        #region POST
        [HttpPost]
        [Route("Create/{id:int?}")]
        public async Task<dynamic> Post(
            ProductCreateArguments arguments,
            [FromUri] int? id = 0,
            [Injection] IServiceBaseHandler<ProductCreateArguments> handler = null
        )
        {
            arguments.FamilyId = id == 0 ? null : id;
            var item = await handler.ExecuteAsync(arguments);
            return new { item.Id };
        }
		#endregion POST

		#region PUT /ImageCrop/{id:int}	
		[HttpPut]
		[Route("ImageCrop/{id:int}")]
		public async Task<dynamic> ProductImage(
			int id,
			ProductUpdatePhotoArguments arguments, 
			[Injection] IServiceBaseHandler<ProductUpdatePhotoArguments> handler)
		{
			arguments.Id = id;
			var item = await handler.ExecuteAsync(arguments);
			return new { item.Id };
		}
		#endregion PUT /ImageCrop/{id:int}

		#region GET /RetrieveSelector/{filter?}
		[HttpGet]
        [Route("RetrieveSelector/{filter?}")]
        public async Task<ResultBase<ProductGetSelectorResult>> RetrieveSelector(
			string filter,
            [FromUri] ProductGetSelectorArguments arguments,
            [Injection] IQueryBaseHandler<ProductGetSelectorArguments, ProductGetSelectorResult> handler
        )
        {
            var result = await handler.ExecuteAsync(arguments);
            return result;
        }
		#endregion GET /RetrieveSelector/{filter?}

		#region DELETE /{id:int}
		[HttpDelete]
		[Route("{id:int}")]
		[XpAuthorize(
		ClientIds = AccountClientId.Web,
		Roles = AccountRoles.Commerce + "," + AccountRoles.CommercePayment
		)]
		public async Task<dynamic> Delete(
			int id,
			[FromUri] ProductDeleteArguments command,
			[Injection] IServiceBaseHandler<ProductDeleteArguments> handler
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
			ProductIsVisibleArguments arguments,
			[Injection] IServiceBaseHandler<ProductIsVisibleArguments> handler)
		{
			arguments.Id = id;
			var result = await handler.ExecuteAsync(arguments);
			return result;
		}
		#endregion PUT /IsVisible/{id:int}

		#region GET /Visibility/{id:int}
		[HttpGet]
		[Route("Visibility/{id:int}")]
		public async Task<ResultBase<ProductGetVisibilityResult>> GetVisibility(
			int id,
			[FromUri] ProductGetVisibilityArguments arguments,
			[Injection] IQueryBaseHandler<ProductGetVisibilityArguments, ProductGetVisibilityResult> handler)
		{
			arguments.Id = id;
			var result = await handler.ExecuteAsync(arguments);
			return result;
		}
		#endregion GET /Visibility/{id:int}

		#region PUT /Visibility/{id:int}
		[HttpPut]
		[Route("Visibility/{id:int}")]
		public async Task<dynamic> Visibility(
			int id,
			[FromUri]ProductVisibilityArguments arguments,
			[Injection] IServiceBaseHandler<ProductVisibilityArguments> handler
		)
		{
			arguments.Id = id;
			var result = await handler.ExecuteAsync(arguments);
			return result;
		}
		#endregion PUT /Visibility/{id:int}
	}
}
