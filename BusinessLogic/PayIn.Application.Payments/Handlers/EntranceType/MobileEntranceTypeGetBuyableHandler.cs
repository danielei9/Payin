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
    public class MobileEntranceTypeGetBuyableHandler :
        IQueryBaseHandler<MobileEntranceTypeGetBuyableArguments, MobileEntranceTypeGetBuyableResult>
    {
        [Dependency] public IEntityRepository<EntranceType> Repository { get; set; }
		[Dependency] public IEntityRepository<SystemCard> SystemCardRepository { get; set; }
		[Dependency] public IEntityRepository<ServiceCard> ServiceCardRepository { get; set; }
		[Dependency] public ISessionData SessionData { get; set; }
        [Dependency] public PaymentConcessionMobileGetHandler PaymentConcessionMobileGetHandler { get; set; }

        #region ExecuteAsync
        public async Task<ResultBase<MobileEntranceTypeGetBuyableResult>> ExecuteAsync(MobileEntranceTypeGetBuyableArguments arguments)
        {
            var now = DateTime.UtcNow;

			var systemCards = (await SystemCardRepository.GetAsync())
				.Where(x =>
					x.Cards
						.Any(y => y.Id == arguments.CardId) 
				);

            var result = (await Repository.GetAsync())
				.Where(x =>
                    (x.State == EntranceTypeState.Active) &&
                    (arguments.PaymentConcessionId == null || x.Event.PaymentConcessionId == arguments.PaymentConcessionId) &&
					(
						(
							(arguments.EventId == null) ||
							(arguments.EventId == x.EventId)
						)
					) &&
					(x.SellStart <= now) && (x.SellEnd >= now) &&									// Periodo de venta y
					(x.Event.EventEnd >= now) &&													// Cierre del evento y
					(x.IsVisible == true) &&                                                        // Evento visible y
					(systemCards
						.Any(y =>
							y.ConcessionOwnerId == x.Event.PaymentConcession.Concession.Id		// El dueño del SystemCard de la tarjeta del usuario organiza el evento y
						)
                    ) &&
					(
						(x.Visibility == EntranceTypeVisibility.Public) ||            // Que sea público o
						(x.Visibility == EntranceTypeVisibility.Internal) ||             // internal,
						(
							x.Visibility == EntranceTypeVisibility.Members &&			// o solo para miembros y
                            x.Event.PaymentConcession.Concession.ServiceUsers
                                .Any(y => y.OnwnerCards
                                    .Any(z => z.Id == arguments.CardId)          // la tarjeta que llama al handler sea miembro
								)
						)
					)
				)
				.Select(x => new MobileEntranceTypeGetBuyableResult
				{
					Id = x.Id,
					Name = x.Name,
					Price = x.Price,
					ExtraPrice = x.ExtraPrice,
					EventId = x.EventId,
					EventName = x.Event.Name,
					MaxEntrancesPerCard = x.Event.MaxEntrancesPerCard,
                    PaymentConcessionId = x.Event.PaymentConcession.Id
                });

            return new ResultBase<MobileEntranceTypeGetBuyableResult>
            {
                Data = result
            };
        }
        #endregion ExecuteAsync
    }
}
