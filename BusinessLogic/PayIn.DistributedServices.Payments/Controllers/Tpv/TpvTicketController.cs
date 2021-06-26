using PayIn.Application.Dto.Payments.Arguments;
using PayIn.Application.Dto.Payments.Results;
using PayIn.Domain.Security;
using PayIn.Web.Security;
using System.Threading.Tasks;
using System.Web.Http;
using Xp.Application.Dto;
using Xp.DistributedServices.Filters;
using Xp.DistributedServices.ModelBinder;

namespace PayIn.DistributedServices.Payments.Controllers
{
	[HideSwagger]
	[RoutePrefix("Tpv/Ticket")]
	[XpAuthorize(
		ClientIds = AccountClientId.Tpv + "," + AccountClientId.PaymentApi,
		Roles = AccountRoles.PaymentWorker + "," + AccountRoles.CommercePayment
	)]
	public class TpvTicketController : ApiController
	{
		#region GET /v1
		[HttpGet]
		[Route("v1")]
		public async Task<ResultBase<TicketTpvGetAllResult>> Get(
			[FromUri] TicketTpvGetAllArguments command,
			[Injection] IQueryBaseHandler<TicketTpvGetAllArguments, TicketTpvGetAllResult> handler
		)
		{
			var result = await handler.ExecuteAsync(command);
			return result;
		}
		#endregion GET /v1

		#region POST /v1
		/// <summary>
		/// Método para crear un ticket
		/// </summary>
		/// <returns></returns>
		[HttpPost]
		[Route("v1")]
		public async Task<dynamic> Post(
			TicketTpvCreateArguments arguments,
			[Injection] IServiceBaseHandler<TicketTpvCreateArguments> handler
		)
		{
			var item = await handler.ExecuteAsync(arguments);
			return new { item.Id };
		}
		#endregion POST /v1
	}
}
