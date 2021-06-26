using PayIn.Application.Dto.Bus.Arguments;
using PayIn.Application.Dto.Bus.Results;
using PayIn.Domain.Security;
using PayIn.Web.Security;
using System.Threading.Tasks;
using System.Web.Http;
using Xp.Application.Dto;
using Xp.DistributedServices.Filters;
using Xp.DistributedServices.ModelBinder;

namespace PayIn.DistributedServices.Bus.Controllers.Api
{
	[HideSwagger]
	[RoutePrefix("Bus/Api/Graph")]
	[XpAuthorize(
		ClientIds = AccountClientId.Web,
		Roles = AccountRoles.User
	)]
	public class BusApiGraphController : ApiController
	{
		#region GET /
		[HttpGet]
		[Route]
		public async Task<dynamic> GetAll(
			[FromUri] BusApiLineGetAllArguments arguments,
			[Injection] IQueryBaseHandler<BusApiLineGetAllArguments, BusApiLineGetAllResult> handler
		)
		{
			var item = await handler.ExecuteAsync(arguments);
			return item;
		}
		#endregion GET /
	}
}
