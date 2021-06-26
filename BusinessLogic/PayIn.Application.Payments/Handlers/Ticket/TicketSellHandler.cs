using Microsoft.Practices.Unity;
using PayIn.Application.Dto.Payments.Arguments;
using PayIn.BusinessLogic.Common;
using PayIn.Common;
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
	[XpLog("Ticket", "Sell")]
	[XpAnalytics("Ticket", "Sell")]
	public class TicketSellHandler :
		IServiceBaseHandler<TicketSellArguments>
	{
		[Dependency] public ISessionData SessionData { get; set; }
		[Dependency] public IEntityRepository<EntranceType> EntranceTypeRepository { get; set; }
		[Dependency] public MobileTicketCreateAndGetHandler CreateTicket { get; set; }
		[Dependency] public ApiEntranceCreateHandler CreateEntrance { get; set; }
		[Dependency] public IEntityRepository<ServiceCard> ServiceCardRepository { get; set; }
		[Dependency] public IEntityRepository<BlackList> BlackListRepository { get; set; }

		#region ExecuteAsync
		public async Task<dynamic> ExecuteAsync(
			TicketSellArguments arguments
		)
		{
			var now = DateTime.Now;

			var serviceCard = (await ServiceCardRepository.GetAsync())
				.Where(x =>
					(x.Id == arguments.CardId)
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

			var entranceTypeIds = arguments.EntranceTypes
				.Where(x => x.Selected == true)
				.Select(x => x.Id)
				.ToList();
			if (entranceTypeIds.Count() == 0)
				throw new ApplicationException("No ha seleccionado ninguna entrada");
			
			var paymentConcessions = (await EntranceTypeRepository.GetAsync())
				.Where(x =>
					(entranceTypeIds.Contains(x.Id))
				)
				.GroupBy(x => new {
					Id = x.Event.PaymentConcessionId,
					TaxNumber = x.Event.PaymentConcession.Concession.Supplier.TaxNumber,
					TaxName = x.Event.PaymentConcession.Concession.Supplier.TaxName,
					Price = x.Price
				})
				.ToList();
			if (paymentConcessions.Count() > 1)
				throw new ApplicationException("No se pueden comprar entradas de empresas distintas");
			var paymentConcession = paymentConcessions.FirstOrDefault();
			if (paymentConcession == null)
				throw new ArgumentNullException("concessionId");

			var lines = paymentConcession
				.Select(x => new MobileTicketCreateAndGetArguments_TicketLine
				{
					EntranceTypeId = x.Id,
					Type = TicketLineType.Entrance,
					Quantity = 1,
					Amount = x.Price
				})
#if DEBUG
				.ToList()
#endif // DEBUG
				;

			// XAVI: No creo que esto esté bien ya que no siempre una venta se paga con una tarjeta (el uid puede ser donde se carge una entrada
			// Este caso es el que una falla en el portal venda una entrada desde la ficha de la pulsera.
			// Quito la inicialización del Uid
			var payments = new List<MobileTicketCreateAndGetArguments_Payment>();
			var payment = new MobileTicketCreateAndGetArguments_Payment();
			payment.Amount = lines
				.Sum(x => x.Amount);
			//payment.Uid = serviceCard.Uid;
			payment.Date = now;
			payments.Add(payment);

			var ticket = await CreateTicket.CreateTicketAsync("", serviceCard.Uid, now, paymentConcession.Key.Id, null, lines, payments, null, null, null, TicketType.Ticket, "", SessionData.Login, "", null, now, true, false);
			return ticket;
		}
		#endregion ExecuteAsync
	}
}
