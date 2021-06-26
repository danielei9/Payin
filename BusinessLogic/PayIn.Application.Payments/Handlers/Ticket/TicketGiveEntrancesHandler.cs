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
	[XpLog("Ticket", "GiveEntrances")]
	[XpAnalytics("Ticket", "GiveEntrances")]
	public class TicketGiveEntrancesHandler :
		IServiceBaseHandler<TicketGiveEntrancesArguments>
	{
		[Dependency] public ISessionData SessionData { get; set; }
		[Dependency] public IEntityRepository<Ticket> Repository { get; set; }
		[Dependency] public IEntityRepository<PaymentConcession> PaymentConcessionRepository { get; set; }
		[Dependency] public IEntityRepository<BlackList> BlackListRepository { get; set; }
		[Dependency] public IEntityRepository<EntranceType> EntranceTypeRepository { get; set; }
		[Dependency] public ApiEntranceCreateHandler CreateEntrance { get; set; }

		[Dependency] public IEntityRepository<ServiceCard> ServiceCardRepository { get; set; }


		#region ExecuteAsync
		public async Task<dynamic> ExecuteAsync(
			TicketGiveEntrancesArguments arguments
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
					TaxName = x.Event.PaymentConcession.Concession.Supplier.TaxName
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
					Quantity = 1
				})
#if DEBUG
				.ToList()
#endif // DEBUG
				;

			var entrances = new List<Entrance>();
			foreach (MobileTicketCreateAndGetArguments_TicketLine line in lines)
			{
				var newArguments = new ApiEntranceCreateArguments
				(
					entranceTypeId: line.EntranceTypeId ?? 0,
					quantity: line.Quantity,
					amount: line.Amount ?? 0,
					taxNumber: paymentConcession.Key.TaxNumber,
					taxName: paymentConcession.Key.TaxName,
					email: serviceCard.OwnerLogin,
					login: SessionData.Login,
					uid: serviceCard.Uid,
					payed: true,
					now: System.DateTime.Now
				);
				var entrance = await CreateEntrance.ExecuteAsync(newArguments);
				entrances.AddRange(entrance);
			}

			if (entrances.Count() == 0)
				throw new ApplicationException("No se ha creado ninguna entrada");

			return entrances.FirstOrDefault();
		}
		#endregion ExecuteAsync
	}
}
