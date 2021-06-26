using Microsoft.Practices.Unity;
using PayIn.Application.Dto.Payments.Arguments;
using PayIn.BusinessLogic.Common;
using PayIn.Common;
using PayIn.Common.Resources;
using PayIn.Domain.Payments;
using PayIn.Domain.Public;
using PayIn.Domain.Transport;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xp.Application.Attributes;
using Xp.Application.Dto;
using Xp.Domain;

namespace PayIn.Application.Payments.Handlers
{
	[XpLog("Ticket", "BuyRechargeBalance")]
	[XpAnalytics("Ticket", "BuyRechargeBalance")]
	public class TicketBuyRechargeBalanceHandler :
		IServiceBaseHandler<TicketBuyRechargeBalanceArguments>
	{
		[Dependency] public ISessionData SessionData { get; set; }
		[Dependency] public IEntityRepository<Ticket> Repository { get; set; }
		[Dependency] public IEntityRepository<ServiceCard> ServiceCardRepository { get; set; }
		[Dependency] public IEntityRepository<PaymentConcession> PaymentConcessionRepository { get; set; }
		[Dependency] public IEntityRepository<EntranceType> EntranceTypeRepository { get; set; }
		[Dependency] public IEntityRepository<BlackList> BlackListRepository { get; set; }
		[Dependency] public MobileTicketCreateAndGetHandler CreateTicket { get; set; }

		#region ExecuteAsync
		public async Task<dynamic> ExecuteAsync(
			TicketBuyRechargeBalanceArguments arguments
		)
		{

			var now = DateTime.Now;

			var serviceCard = (await ServiceCardRepository.GetAsync())
				.Where(x =>
					(x.Id == arguments.ServiceCardId)
				)
				.FirstOrDefault();
			if (serviceCard == null)
				throw new ArgumentNullException("serviceCardId");

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

			var lines = new List<MobileTicketCreateAndGetArguments_TicketLine>
			{
				new MobileTicketCreateAndGetArguments_TicketLine
				{
					Type = TicketLineType.Recharge,
					Title = "Recarga",
					Amount = arguments.Amount,
					PurseId = arguments.PurseId,
					Quantity = 1
				}
			};

			var paymentConcession = (await PaymentConcessionRepository.GetAsync("Concession"))
				.Where(x => x.ConcessionId == serviceCard.ConcessionId)
				.FirstOrDefault();
			if (paymentConcession == null)
				throw new ArgumentException(TicketResources.CreateNonActiveConcessionException, "paymentConcessionId");
			if (paymentConcession.Concession.State != ConcessionState.Active)
				throw new ArgumentException(TicketResources.CreateNonActiveConcessionException, "paymentConcessionId");

			var ticket = await CreateTicket.CreateTicketAsync("", serviceCard.Uid, now, paymentConcession.Id, null, lines, null, null, null, null, TicketType.Recharge, "", SessionData.Login, "", null, now, true, false);
			return ticket;
		}
		#endregion ExecuteAsync
	}
}
