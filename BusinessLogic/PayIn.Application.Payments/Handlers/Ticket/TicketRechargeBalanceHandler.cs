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
	[XpLog("Ticket", "RechargeBalance")]
	[XpAnalytics("Ticket", "RechargeBalance")]
	public class TicketRechargeBalanceHandler :
		IServiceBaseHandler<TicketRechargeBalanceArguments>
	{
		[Dependency] public ISessionData SessionData { get; set; }
		[Dependency] public IEntityRepository<Ticket> Repository { get; set; }
		[Dependency] public IEntityRepository<PaymentConcession> PaymentConcessionRepository { get; set; }
		[Dependency] public IEntityRepository<EntranceType> EntranceTypeRepository { get; set; }
		[Dependency] public IEntityRepository<ServiceCard> ServiceCardRepository { get; set; }
		[Dependency] public IEntityRepository<BlackList> BlackListRepository { get; set; }
		[Dependency] public MobileTicketCreateAndGetHandler CreateTicket { get; set; }

		#region ExecuteAsync
		public async Task<dynamic> ExecuteAsync(
			TicketRechargeBalanceArguments arguments
		)
		{
			if (arguments.Amount <= 0)
				throw new ArgumentException("Invalid amount");

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

			var paymentConcession = (await PaymentConcessionRepository.GetAsync("Concession"))
				.Where(x => x.ConcessionId == serviceCard.ConcessionId)
				.FirstOrDefault();
			if (paymentConcession == null)
				throw new ArgumentException(TicketResources.CreateNonActiveConcessionException, "paymentConcessionId");
			if (paymentConcession.Concession.State != ConcessionState.Active)
				throw new ArgumentException(TicketResources.CreateNonActiveConcessionException, "paymentConcessionId");

			var now = DateTime.Now;
			
			var lines = new List<MobileTicketCreateAndGetArguments_TicketLine>
			{
				new MobileTicketCreateAndGetArguments_TicketLine
				{
					Type = TicketLineType.Recharge,
					Amount = arguments.Amount,
					PurseId = arguments.PurseId,
					Quantity = 1
				}
			};

			var payments = new List<MobileTicketCreateAndGetArguments_Payment> {
					new MobileTicketCreateAndGetArguments_Payment {
						Amount = arguments.Amount,
						Date = now,
						ExternalLogin = "",
						Uid = serviceCard.Uid
					}
				};

			var ticket = await CreateTicket.CreateTicketAsync("", serviceCard.Uid, now, paymentConcession.Id, null, lines, payments, null, null, null, TicketType.Recharge, "", SessionData.Login, "", arguments.Amount, now, true, false);
			var result = await CreateTicket.GetTicketAsync(ticket);
			
			return result;
		}
		#endregion ExecuteAsync
	}
}
