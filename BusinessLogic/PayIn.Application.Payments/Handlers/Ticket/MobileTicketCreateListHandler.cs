using Microsoft.Practices.Unity;
using PayIn.Application.Dto.Payments.Arguments;
using PayIn.BusinessLogic.Common;
using PayIn.Domain.Payments;
using System;
using System.Linq;
using System.Threading.Tasks;
using Xp.Application.Dto;
using Xp.Domain;

namespace PayIn.Application.Payments.Handlers
{
    public class MobileTicketCreateListHandler : IServiceBaseHandler<MobileTicketCreateListArguments>
	{
        [Dependency] public ISessionData SessionData { get; set; }
        [Dependency] public MobileTicketCreateAndGetHandler MobileTicketCreateAndGetHandler { get; set; }
        [Dependency] public IEntityRepository<Ticket> TicketRepository { get; set; }
        [Dependency] public IEntityRepository<PaymentWorker> PaymentWorkerRepository { get; set; }

        #region ExecuteAsync
        public async Task<dynamic> ExecuteAsync(MobileTicketCreateListArguments arguments)
		{
			var now = DateTime.UtcNow;

            foreach (var ticket in arguments.List)
                await MobileTicketCreateAndGetHandler.CreateTicketAsync(ticket.Reference, null, ticket.Date, arguments.ConcessionId, ticket.EventId, ticket.Lines, ticket.Payments, ticket.Forms, arguments.LiquidationConcession, ticket.TransportPrice, ticket.Type, SessionData.Email, SessionData.Login, ticket.ExternalLogin, null, now, false, true);

            var eventId = arguments.List
                .Where(x => x.EventId != null)
                .Select(x => x.EventId)
                .FirstOrDefault();

            var sum = arguments.List
                .Select(x => x.Lines.Sum(y => y.Amount))
                .GroupBy(x => x >= 0)
                .ToList();
            var total = (await TicketRepository.GetAsync())
                .Where(x => x.PaymentWorker.Login == SessionData.Login)
                .Select(x => x.Amount)
                .GroupBy(x => x >= 0)
                .ToList();

            return new
            {
                SumRecharged = -1 * sum
                    .Where(x => !x.Key)
                    .Sum(x => x
                        .Sum(y => (decimal?)y)
                    ) ?? 0m,
                SumPayed = sum
                    .Where(x => x.Key)
                    .Sum(x => x
                        .Sum(y => (decimal?)y)
                    ) ?? 0m,
                TotalRecharged = -1 * total
                    .Where(x => !x.Key)
                    .Sum(x => x
                        .Sum(y => (decimal?)y)
                    ) ?? 0m,
                TotalPayed = total
                    .Where(x => x.Key)
                    .Sum(x => x
                        .Sum(y => (decimal?)y)
                    ) ?? 0m,
            };
		}
        #endregion ExecuteAsync
    }
}
