using Microsoft.Practices.Unity;
using PayIn.Application.Dto.Payments.Arguments;
using PayIn.Application.Dto.Payments.Results;
using System.Threading.Tasks;
using Xp.Application.Dto;

namespace PayIn.Application.Payments.Handlers
{
    public class TicketMobileGetOrdersByPaymentConcessionHandler :
		IQueryBaseHandler<TicketMobileGetOrdersByPaymentConcessionArguments, MobileTicketGetOrdersResult>
	{
		[Dependency] public MobileTicketGetOrdersHandler MobileTicketGetOrdersHandler { get; set; }

		#region ExecuteAsync
		public async Task<ResultBase<MobileTicketGetOrdersResult>> ExecuteAsync(TicketMobileGetOrdersByPaymentConcessionArguments arguments)
		{
			var result = await MobileTicketGetOrdersHandler.ExecuteInternalAsync(arguments.PaymentConcessionId);

			return new ResultBase<MobileTicketGetOrdersResult>
			{
				Data = result
			};
		}
		#endregion ExecuteAsync
	}
}
