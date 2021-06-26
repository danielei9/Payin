using PayIn.Application.Dto.Transport.Arguments.TransportSystem;
using PayIn.Application.Dto.Transport.Results.TransportSystem;
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
	[RoutePrefix("Api/TransportSystem")]
	[XpAuthorize(
	ClientIds = AccountClientId.Web,
	Roles = AccountRoles.Superadministrator)]
	public class ApiTransportSystemController : ApiController
	{
		#region GET /
		[HttpGet]
		[Route("")]
		public async Task<ResultBase<TransportSystemGetAllResult>> GetAll(
			[FromUri] TransportSystemGetAllArguments arguments,
			[Injection] IQueryBaseHandler<TransportSystemGetAllArguments, TransportSystemGetAllResult> handler
		)
		{
			var result = await handler.ExecuteAsync(arguments);
			return result;
		}
		#endregion GET /

		#region POST /
		[HttpPost]
		[Route]
		public async Task<dynamic> Post(
			TransportSystemCreateArguments arguments,
			[Injection] IServiceBaseHandler<TransportSystemCreateArguments> handler
		)
		{
			var item = await handler.ExecuteAsync(arguments);
			return new { item.Id };
		}
		#endregion POST /

		#region DELETE /
		[HttpDelete]
		[Route("{id:int}")]
		public async Task<dynamic> Delete(
			[FromUri] TransportSystemDeleteArguments arguments,
			[Injection] IServiceBaseHandler<TransportSystemDeleteArguments> handler
		)
		{
			var result = await handler.ExecuteAsync(arguments);
			return result;
		}
		#endregion DELETE /
	}
}
