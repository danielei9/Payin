using PayIn.Application.Dto.Payments.Arguments;
using PayIn.Application.Dto.Payments.Results;
using PayIn.Domain.Security;
using PayIn.Web.Security;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using Xp.Application.Dto;
using Xp.DistributedServices.Filters;
using Xp.DistributedServices.Formatters;
using Xp.DistributedServices.ModelBinder;

namespace PayIn.DistributedServices.Payments.Controllers.Api
{
    [HideSwagger]
	[RoutePrefix("Api/ServiceCategory")]
	[XpAuthorize(
		ClientIds = AccountClientId.Web,
		Roles = AccountRoles.Superadministrator + "," + AccountRoles.Operator + "," +AccountRoles.Commerce + "," + AccountRoles.CommercePayment
		
	)]
	public class ApiServiceCategoryController : ApiController
	{
		#region GET /
		[HttpGet]
		[Route]
		[XpAuthorize(
			ClientIds = AccountClientId.Web,
			Roles = AccountRoles.Commerce + "," + AccountRoles.CommercePayment
		)]
		public async Task<ResultBase<ServiceCategoryGetAllResult>> GetAll(
			[FromUri] ServiceCategoryGetAllArguments arguments,
			[Injection] IQueryBaseHandler<ServiceCategoryGetAllArguments, ServiceCategoryGetAllResult> handler
		)
		{
			var result = await handler.ExecuteAsync(arguments);
			return result;
		}

		#endregion GET /

		#region PUT /{id:int}
		[HttpPut]
		[Route("{id:int}")]
		[XpAuthorize(Roles = AccountRoles.Superadministrator + "," + AccountRoles.Commerce + "," + AccountRoles.CommercePayment)]
		public async Task<dynamic> Put(
			int id,
			ServiceCategoryUpdateArguments arguments,
			[Injection] IServiceBaseHandler<ServiceCategoryUpdateArguments> handler)
		{
			arguments.Id = id;
			var item = await handler.ExecuteAsync(arguments);
			return new { Id = item.Id };
		}
        #endregion PUT /{id:int}

        #region POST
        [HttpPost]
        [Route("Create/{id:int?}")]
        public async Task<dynamic> Post(
			ServiceCategoryCreateArguments arguments,
            [FromUri] int? id = 0,
            [Injection] IServiceBaseHandler<ServiceCategoryCreateArguments> handler = null
        )
        {
            arguments.FamilyId = id == 0 ? null : id;
            var item = await handler.ExecuteAsync(arguments);
            return new { id = item.Id };
        }
		#endregion POST

		#region GET /RetrieveSelector/{filter?}
		[HttpGet]
        [Route("RetrieveSelector/{filter?}")]
        public async Task<ResultBase<ServiceCategoryGetSelectorResult>> RetrieveSelector(
			string filter,
            [FromUri] ServiceCategoryGetSelectorArguments arguments,
            [Injection] IQueryBaseHandler<ServiceCategoryGetSelectorArguments, ServiceCategoryGetSelectorResult> handler
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
			[FromUri] ServiceCategoryDeleteArguments command,
			[Injection] IServiceBaseHandler<ServiceCategoryDeleteArguments> handler
			)
			{
				
				var result = await handler.ExecuteAsync(command);
				return result;
			}
		#endregion DELETE /{id:int}

    }
}
