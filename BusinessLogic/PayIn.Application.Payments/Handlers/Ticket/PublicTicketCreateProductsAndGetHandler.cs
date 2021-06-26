using Microsoft.Practices.Unity;
using PayIn.Application.Dto.Payments.Arguments;
using PayIn.Application.Dto.Payments.Results;
using PayIn.Common;
using System;
using System.Linq;
using System.Threading.Tasks;
using Xp.Application.Dto;

namespace PayIn.Application.Payments.Handlers
{
	public class PublicTicketCreateProductsAndGetHandler : IServiceBaseHandler<PublicTicketCreateProductsAndGetArguments>
	{
		[Dependency] public MobileTicketCreateAndGetHandler MobileTicketCreateAndGetHandler { get; set; }
		[Dependency] public PublicPaymentMediaGetByUserHandler PublicPaymentMediaGetByUserHandler { get; set; }

		#region ExecuteAsync
		public async Task<dynamic> ExecuteAsync(PublicTicketCreateProductsAndGetArguments arguments)
        {
            throw new ApplicationException("Temporaly blocked");

            var lines = arguments.Lines
				.Select(x => new MobileTicketCreateAndGetArguments_TicketLine
				{
					ProductId = x.ProductId,
					Quantity = x.Quantity
				});

			var ticket = await MobileTicketCreateAndGetHandler.CreateTicketAsync(arguments.Reference, null, arguments.Now, arguments.ConcessionId, arguments.EventId, lines, null, null, null, null, arguments.Type, arguments.Email, arguments.Login, "", null, arguments.Now, true, false);
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
