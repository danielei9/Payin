using Microsoft.Practices.Unity;
using PayIn.Application.Dto.Payments.Arguments;
using PayIn.BusinessLogic.Common;
using PayIn.Common;
using PayIn.Domain.Payments;
using System;
using System.Linq;
using System.Threading.Tasks;
using Xp.Application.Dto;
using Xp.Domain;

namespace PayIn.Application.Payments.Handlers
{
	public class TicketCreateListHandler : IServiceBaseHandler<PublicTicketCreateListArguments>
	{
        [Dependency] public ISessionData SessionData { get; set; }
        [Dependency] public IEntityRepository<PaymentConcession> PaymentConcessionRepository { get; set; }
        [Dependency] public IEntityRepository<Product> ProductRepository { get; set; }
		[Dependency] public MobileTicketCreateAndGetHandler MobileTicketCreateAndGetHandler { get; set; }

		#region ExecuteAsync
		public async Task<dynamic> ExecuteAsync(PublicTicketCreateListArguments arguments)
		{
            var now = DateTime.Now;

            var paymentConcession = (await PaymentConcessionRepository.GetAsync())
                .Where(x => 
                    x.Concession.State == ConcessionState.Active &&
                    x.Concession.Supplier.Login == SessionData.Login
                )
                .Select(x => new
                {
                    x.Id
                })
                .FirstOrDefault();

            var productCodes = arguments.Tickets
                .SelectMany(x => x.Lines
                    .Where(y =>
                        (y.ProductId == null) &&
                        (!y.ProductCode.IsNullOrEmpty())
                    )
                    .Select(y => y.ProductCode)
                )
                .ToList();
            var products = (await ProductRepository.GetAsync())
                .Where(x =>
                    productCodes.Contains(x.Code)
                )
                .Distinct()
                .ToDictionary(x => x.Code, x => x.Id);

            foreach (var ticket in arguments.Tickets)
            {
                await MobileTicketCreateAndGetHandler.CreateTicketAsync(
                    ticket.Reference,
					null,
                    ticket.Date,
                    paymentConcession.Id,
                    null, // eventId
                    ticket.Lines
                        .Select(x => new MobileTicketCreateAndGetArguments_TicketLine
                        {
                            Title = x.Title,
                            Amount = x.Amount,
                            Quantity = x.Quantity,
                            Type = TicketLineType.Buy,
                            ProductId = x.ProductId ?? products[x.ProductCode],
                            EntranceTypeId = x.EntranceTypeId,
                            CampaignLineId = x.CampaignLineId,
                            CampaignId = x.CampaignId,
                            CampaignCode = x.CampaignCode,
                        }),
                    null,
                    new TicketAnswerFormsArguments_Form[0],
                    null, // liquidationConcessionId
                    null, // transportPrice
                    TicketType.Ticket,
                    ticket.Email,
                    SessionData.Login,
                    "", // external login
                    null, // amount
                    now,
                    false,
                    true
                );
            }
            
            return null;
		}
        #endregion ExecuteAsync
    }
}
