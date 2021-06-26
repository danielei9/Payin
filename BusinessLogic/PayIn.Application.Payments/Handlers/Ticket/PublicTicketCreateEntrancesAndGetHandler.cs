using Microsoft.Practices.Unity;
using PayIn.Application.Dto.Payments.Arguments;
using PayIn.Application.Dto.Payments.Results;
using PayIn.Common;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xp.Application.Attributes;
using Xp.Application.Dto;

namespace PayIn.Application.Payments.Handlers
{
	[XpLog("Ticket", "CreateEntrancesAndGet")]
	[XpAnalytics("Ticket", "CreateEntrancesAndGet")]
	public class PublicTicketCreateEntrancesAndGetHandler : IServiceBaseHandler<PublicTicketCreateEntrancesAndGetArguments>
	{
		[Dependency] public MobileTicketCreateAndGetHandler MobileTicketCreateAndGetHandler { get; set; }
		[Dependency] public PublicPaymentMediaGetByUserHandler PublicPaymentMediaGetByUserHandler { get; set; }

		#region ExecuteAsync
		public async Task<dynamic> ExecuteAsync(PublicTicketCreateEntrancesAndGetArguments arguments)
        {
            var lines = arguments.Lines
				.Select(x => new MobileTicketCreateAndGetArguments_TicketLine
				{
					EntranceTypeId = x.EntranceTypeId,
					Quantity = x.Quantity
				});
			var payments = new List<MobileTicketCreateAndGetArguments_Payment>();

			var ticket = await MobileTicketCreateAndGetHandler.CreateTicketAsync(arguments.Reference, null, arguments.Now, arguments.ConcessionId, arguments.EventId, lines, payments, null, null, null, arguments.Type, arguments.Email, arguments.Login, "", null, arguments.Now, true, false);
			var paymentMedias = (await PublicPaymentMediaGetByUserHandler.ExecuteInternalAsync(arguments.Now, arguments.Login))
					.Where(x => x.State == PaymentMediaState.Active);
			var result = await MobileTicketCreateAndGetHandler.GetTicketAsync(ticket);

			return new PublicTicketCreateAndGetResultBase {
				Data = new[] { result },
				PaymentMedias = paymentMedias
					.ToList()
			};
		}
        #endregion ExecuteAsync
    }
}
