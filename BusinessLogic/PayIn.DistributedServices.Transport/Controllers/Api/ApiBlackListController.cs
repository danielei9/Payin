using PayIn.Application.Dto.Transport.Arguments.BlackList;
using PayIn.Application.Dto.Transport.Results.BlackList;
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
	[RoutePrefix("Api/BlackList")]
	[XpAuthorize(
		ClientIds = AccountClientId.Web,
		Roles = AccountRoles.Transport + "," + AccountRoles.TransportOperator + "," + AccountRoles.Superadministrator
	)]
	public class ApiBlackListController : ApiController
	{
		#region GET /
		[HttpGet]
		[Route("")]
		public async Task<ResultBase<BlackListGetAllResult>> GetAll(
			[FromUri] BlackListGetAllArguments arguments,
			[Injection] IQueryBaseHandler<BlackListGetAllArguments, BlackListGetAllResult> handler
		)
		{
			var result = await handler.ExecuteAsync(arguments);
			return result;
		}
		#endregion GET /

		#region DELETE /{id:int}
		[HttpDelete]
		[Route("{id:long}")]
		public async Task<dynamic> Delete(
			[FromUri] BlackListCreateArguments arguments,
			[Injection] IServiceBaseHandler<BlackListCreateArguments> handler
		)
		{
			var result = await handler.ExecuteAsync(arguments);
			return result;
		}
		#endregion DELETE /{id:int}

		#region DELETE /Delete/{id:int}
		[HttpDelete]
		[Route("Delete/{id:long}")]
		public async Task<dynamic> Delete2(
			[FromUri] BlackListDeleteArguments arguments,
			[Injection] IServiceBaseHandler<BlackListDeleteArguments> handler
		)
		{
			var result = await handler.ExecuteAsync(arguments);
			return result;
		}
		#endregion DELETE /Delete/{id:int}
	}
}
