using Microsoft.Practices.Unity;
using PayIn.Application.Dto.Payments.Arguments;
using PayIn.BusinessLogic.Common;
using PayIn.Common;
using PayIn.Domain.Payments;
using PayIn.Domain.Public;
using PayIn.Domain.Transport;
using System;
using System.Linq;
using System.Threading.Tasks;
using Xp.Application.Attributes;
using Xp.Application.Dto;
using Xp.Domain;

namespace PayIn.Application.Payments.Handlers
{
    [XpLog("Ticket", "BuyManyEntrances")]
    [XpAnalytics("Ticket", "BuyManyEntrances")]
    public class TicketBuyManyEntrancesHandler :
		IServiceBaseHandler<TicketBuyManyEntrancesArguments>
	{
        [Dependency] public ISessionData SessionData { get; set; }
        [Dependency] public IEntityRepository<ServiceCard> ServiceCardRepository { get; set; }
        [Dependency] public IEntityRepository<EntranceType> EntranceTypeRepository { get; set; }
        [Dependency] public IEntityRepository<PaymentConcession> PaymentConcessionRepository { get; set; }
		[Dependency] public IEntityRepository<BlackList> BlackListRepository { get; set; }
		[Dependency] public MobileTicketCreateAndGetHandler CreateTicket { get; set; }

        #region ExecuteAsync
        public async Task<dynamic> ExecuteAsync(
			TicketBuyManyEntrancesArguments arguments
        )
        {
			if (arguments.ConcessionId<=0)
				throw new ArgumentNullException("concessionId");

			var now = DateTime.Now;

            var serviceCard = (await ServiceCardRepository.GetAsync())
                .Where(x =>
                    (x.Id == arguments.ServiceCardId)
                )
                .FirstOrDefault();
            if (serviceCard == null)
                throw new ArgumentNullException("cardId");

			var inBlackLists = (await BlackListRepository.GetAsync())
				.Where(y =>
					(y.Uid == serviceCard.Uid) &&
					(y.SystemCardId == serviceCard.SystemCardId) &&
					(y.State == BlackList.BlackListStateType.Active) &&
					(!y.Resolved)
				)
				.Any();
			if (inBlackLists)
				throw new ApplicationException("No se puede operar con una tarjeta bloqueada");

			var entranceTypeIds = arguments.EntrancesType
                .Select(x => x.EntranceTypeId)
                .ToList();
            var paymentConcessionsCount = (await EntranceTypeRepository.GetAsync())
                .Where(x =>
                    (entranceTypeIds.Contains(x.Id))
                )
                .GroupBy(x => new { Id = x.Event.PaymentConcessionId })
                .Count();
			if (paymentConcessionsCount > 1)
				throw new ApplicationException("No se pueden comprar entradas de empresas distintas");

			var lines = arguments.EntrancesType
				.Select(x => new MobileTicketCreateAndGetArguments_TicketLine
				{
					EntranceTypeId = x.EntranceTypeId,
					Type = TicketLineType.Entrance,
					Quantity = x.Quantity
				})
#if DEBUG
                .ToList()
#endif // DEBUG
                ;

			//var ticket = await CreateTicket.CreateTicketAsync("", serviceCard.Uid, now, paymentConcession.Key.Id , null, lines, null, null, null, null, TicketType.Ticket, "", SessionData.Login, "", null, now, true, false);
			var ticket = await CreateTicket.CreateTicketAsync("", serviceCard.Uid, now, arguments.ConcessionId, null, lines, null, null, null, null, TicketType.Ticket, "", SessionData.Login, "", null, now, true, false, false);
			return ticket;
        }
        #endregion ExecuteAsync
    }
}
