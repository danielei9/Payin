using Microsoft.Practices.Unity;
using PayIn.Application.Dto.Payments.Arguments;
using PayIn.Application.Dto.Payments.Results;
using PayIn.Common;
using System;
using System.Linq;
using System.Threading.Tasks;
using Xp.Application.Attributes;
using Xp.Application.Dto;

namespace PayIn.Application.Payments.Handlers
{
	[XpLog("Ticket", "CreateAndGet")]
	[XpAnalytics("Ticket", "CreateAndGet")]
	public class PublicTicketCreateAndGetHandler : IServiceBaseHandler<PublicTicketCreateAndGetArguments>
	{
		[Dependency] public MobileTicketCreateAndGetHandler MobileTicketCreateAndGetHandler { get; set; }
		[Dependency] public PublicPaymentMediaGetByUserHandler PublicPaymentMediaGetByUserHandler { get; set; }

		#region ExecuteAsync
		public async Task<dynamic> ExecuteAsync(PublicTicketCreateAndGetArguments arguments)
		{
			var now = DateTime.Now;

			var ticket = await MobileTicketCreateAndGetHandler.CreateTicketAsync(arguments.Reference, null, arguments.Date, arguments.ConcessionId, arguments.EventId, arguments.Lines, null, new TicketAnswerFormsArguments_Form[0], null, null, arguments.Type, arguments.Email, arguments.Login, arguments.ExternalLogin, null, now, true, false);
			var paymentMedias = (await PublicPaymentMediaGetByUserHandler.ExecuteInternalAsync(now, arguments.Login))
					.Where(x => x.State == PaymentMediaState.Active);
			var result = await MobileTicketCreateAndGetHandler.GetTicketAsync(ticket);

			return new PublicTicketCreateAndGetResultBase {
				Data = new MobileTicketCreateAndGetResult[] { result },
				PaymentMedias = paymentMedias
					.ToList()
			};
		}
        #endregion ExecuteAsync
    }
}
