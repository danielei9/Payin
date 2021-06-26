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
    [XpLog("Ticket", "DonateBalance")]
	[XpAnalytics("Ticket", "DonateBalance")]
	public class TicketDonateBalanceHandler :
		IServiceBaseHandler<TicketDonateBalanceArguments>
	{
		[Dependency] public ISessionData SessionData { get; set; }
		[Dependency] public MobileTicketCreateAndGetHandler CreateTicket { get; set; }
		[Dependency] public IEntityRepository<ServiceCard> ServiceCardRepository { get; set; }
		[Dependency] public IEntityRepository<BlackList> BlackListRepository { get; set; }

		#region ExecuteAsync
		public async Task<dynamic> ExecuteAsync(
			TicketDonateBalanceArguments arguments
		)
		{
			var now = DateTime.Now;

			var serviceCard = (await ServiceCardRepository.GetAsync())
				.Where(x => x.Id == arguments.CardId)
				.FirstOrDefault();

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

			var line = new TicketLine
			{
				Type = TicketLineType.Recharge,
				Amount = arguments.Amount,
				PurseId = arguments.PurseId,
				Quantity = 1
			};

			var result = await CreateTicket.ApplyRechargeAsync(null, true, line, serviceCard.Uid, System.DateTime.Now, false);
			return result;
		}
		#endregion ExecuteAsync
	}
}
