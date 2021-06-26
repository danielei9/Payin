using Microsoft.Practices.Unity;
using PayIn.Application.Dto.Payments.Arguments;
using PayIn.Application.Dto.Payments.Results;
using PayIn.BusinessLogic.Common;
using PayIn.Common;
using PayIn.Domain.Payments;
using PayIn.Domain.Public;
using System;
using System.Linq;
using System.Threading.Tasks;
using Xp.Application.Dto;
using Xp.Domain;

namespace PayIn.Application.Payments.Handlers
{
	public class MobileEntranceTypeGetSellableHandler :
        IQueryBaseHandler<MobileEntranceTypeGetSellableArguments, MobileEntranceTypeGetSellableResult>
    {
        [Dependency] public IEntityRepository<EntranceType> Repository { get; set; }
		[Dependency] public IEntityRepository<SystemCard> SystemCardRepository { get; set; }
		[Dependency] public IEntityRepository<ServiceCard> ServiceCardRepository { get; set; }
		[Dependency] public IEntityRepository<PaymentConcession> PaymentConcessionRepository { get; set; }
		[Dependency] public ISessionData SessionData { get; set; }
        [Dependency] public PaymentConcessionMobileGetHandler PaymentConcessionMobileGetHandler { get; set; }

        #region ExecuteAsync
        public async Task<ResultBase<MobileEntranceTypeGetSellableResult>> ExecuteAsync(MobileEntranceTypeGetSellableArguments arguments)
        {
			if (arguments.PaymentConcessionId == null && arguments.CardId == null)
				throw new ApplicationException("Debe indicar una tarjeta o una concesión");

            var now = DateTime.UtcNow;

			var serviceCards = (await ServiceCardRepository.GetAsync());

			var systemCards = (await SystemCardRepository.GetAsync())
				.Where(x =>
					(
						(x.ConcessionOwner.Supplier.Login == SessionData.Login) ||
						(x.ConcessionOwner.Supplier.Workers
							.Any(y => y.Login == SessionData.Login)
						) ||
						(serviceCards.Any(y => y.Concession.Id == x.ConcessionOwnerId)) 
					) && (
						arguments.CardId == null ||
						x.Cards
							.Any(y => y.Id == arguments.CardId)
					)
				);

			var items = (await Repository.GetAsync())
				.Where(x =>
					(x.State == EntranceTypeState.Active) &&
					(
						(arguments.EventId == null) ||
						(arguments.EventId == x.EventId)
					) &&
					(x.SellStart <= now) &&                                                     // Iniciado el periodo de venta y
					(x.SellEnd >= now) &&                                                       // No terminado el periodo de venta y 
					(x.Event.EventEnd >= now) &&                                                // Cierre del evento y
					(x.IsVisible == true) &&                                                    // Evento visible y
					(
						systemCards
							.Any(y =>
								y.ConcessionOwnerId == x.Event.PaymentConcession.Concession.Id  // Que el dueño del SystemCard de la tarjeta organice el evento y
							) &&
						(
							(
								arguments.PaymentConcessionId == null ||
								(
									arguments.PaymentConcessionId != null &&
									x.Event.PaymentConcessionId == arguments.PaymentConcessionId
								)
							) &&
							(
								arguments.CardId == null ||
								(
									arguments.CardId != null &&
									(
										(
											x.Visibility == EntranceTypeVisibility.Public ||                // Que sea público o
											x.Visibility == EntranceTypeVisibility.Internal                 // internal,
										) ||
										(
											(x.Visibility == EntranceTypeVisibility.Members) &&             // o solo para miembros y
											systemCards
												.Any(y =>
													y.Cards.Any(z => z.Id == arguments.CardId)              // la tarjeta que llama al handler sea miembro
												)
										)
									)
								)
							)
						)
					)
				);

			var result = items
				.Select(x => new MobileEntranceTypeGetSellableResult
				{
					Id = x.Id,
					Name = x.Name,
					Price = x.Price,
					ExtraPrice = x.ExtraPrice,
					EventId = x.EventId,
					EventName = x.Event.Name,
					MaxEntrancesPerCard = (x.Event.MaxEntrancesPerCard != int.MaxValue) ? x.Event.MaxEntrancesPerCard : (int?)null,
                    PaymentConcessionId = x.Event.PaymentConcessionId,
					CheckInClosed = (x.Event.CheckInEnd == null ? false : (x.Event.CheckInEnd < now ? true : false))
				});

			return new ResultBase<MobileEntranceTypeGetSellableResult>
            {
                Data = result
            };
        }
        #endregion ExecuteAsync
    }
}
