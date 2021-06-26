using PayIn.Application.Dto.Transport.Arguments.GreyList;
using PayIn.Application.Dto.Transport.Results.GreyList;
using PayIn.Domain.Security;
using PayIn.Web.Security;
using System.Threading.Tasks;
using System.Web.Http;
using Xp.Application.Dto;
using Xp.DistributedServices.Filters;
using Xp.DistributedServices.ModelBinder;

namespace PayIn.DistributedServices.Transport.Controllers.Api
{
	[HideSwagger]
	[RoutePrefix("Api/GreyList")]
	[XpAuthorize(
		ClientIds = AccountClientId.Web,
		Roles = AccountRoles.Transport + "," + AccountRoles.TransportOperator + "," + AccountRoles.Superadministrator
	)]
	public class ApiGreyListController : ApiController
	{
		#region GET /
		[HttpGet]
		[Route("")]
		public async Task<ResultBase<GreyListGetAllResult>> GetAll(
			[FromUri] GreyListGetAllArguments arguments,
			[Injection] IQueryBaseHandler<GreyListGetAllArguments, GreyListGetAllResult> handler
		)
		{
			var result = await handler.ExecuteAsync(arguments);
			return result;
		}
		#endregion GET /

		#region POST Create/{id:long}
		[HttpPost]
		[Route("Create/{id:long}")]
		public async Task<dynamic> Post(
			GreyListCreateArguments arguments,
			[Injection] IServiceBaseHandler<GreyListCreateArguments> handler
		)
		{
			var item = await handler.ExecuteAsync(arguments);
			return new { item.Id };
			
		}
		#endregion POST Create/{id:long}

		#region POST ReadAll/{id:long}
		[HttpPost]
		[Route("ReadAll/{id:long}")]
		public async Task<dynamic> Post(
			GreyListReadAllArguments arguments,
			[Injection] IServiceBaseHandler<GreyListReadAllArguments> handler
		)
		{
			var item = await handler.ExecuteAsync(arguments);
			//return new { item.Id };
			return 1;
		}
		#endregion POST ReadAll/{id:long}

		#region POST ModifyBlock/{id:long}
		[HttpPost]
		[Route("ModifyBlock/{id:long}")]
		public async Task<dynamic> Post(
			GreyListModifyBlockArguments arguments,
			[Injection] IServiceBaseHandler<GreyListModifyBlockArguments> handler
		)
		{
			var item = await handler.ExecuteAsync(arguments);
			//return new { item.Id };
			return 1;
		}
		#endregion POST ModifyBlock/{id:long}

		#region POST ModifyField/{id:long}
		[HttpPost]
		[Route("ModifyField/{id:long}")]
		public async Task<dynamic> Post2(
			GreyListModifyFieldArguments arguments,
			[Injection] IServiceBaseHandler<GreyListModifyFieldArguments> handler			
		)
		{			
			var item = await handler.ExecuteAsync(arguments);
			return new { Count = item };
		}
		#endregion POST ModifyField/{id:long}

		#region POST ModifyField - Test
		[HttpPost]
		[Route("ModifyField")]
		public async Task<dynamic> Post3(
			GreyListModifyFieldArguments arguments,
			[Injection] IServiceBaseHandler<GreyListModifyFieldArguments> handler
		)
		{
			var item = await handler.ExecuteAsync(arguments);
			return new { Count = item };
		}
		#endregion POST ModifyField - Test

		#region DELETE /
		[HttpDelete]
		[Route("{id:int}")]
		public async Task<dynamic> Delete(
			[FromUri] GreyListDeleteArguments arguments,
			[Injection] IServiceBaseHandler<GreyListDeleteArguments> handler
		)
		{
			var result = await handler.ExecuteAsync(arguments);
			return result;
		}
		#endregion DELETE /
	}
}
