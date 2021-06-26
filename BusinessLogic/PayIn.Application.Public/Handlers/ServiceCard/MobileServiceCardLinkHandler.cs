using Microsoft.Practices.Unity;
using PayIn.Application.Dto.Arguments;
using PayIn.Application.Dto.Arguments.Notification;
using PayIn.BusinessLogic.Common;
using PayIn.Common;
using PayIn.Domain.Payments;
using PayIn.Domain.Public;
using System;
using System.Linq;
using System.Threading.Tasks;
using Xp.Application.Dto;
using Xp.Domain;

namespace PayIn.Application.Public.Handlers
{
    public class MobileServiceCardLinkHandler : IServiceBaseHandler<MobileServiceCardLinkArguments>
	{
		[Dependency] public ISessionData SessionData { get; set; }
		[Dependency] public IEntityRepository<ServiceCard> Repository { get; set; }
		[Dependency] public IEntityRepository<PaymentConcession> PaymentConcessionRepository { get; set; }
		[Dependency] public ServiceNotificationCreateHandler ServiceNotificationCreate { get; set; }

        private string hexArray = "0123456789ABCDEF";

		#region ExecuteAsync
		public async Task<dynamic> ExecuteAsync(MobileServiceCardLinkArguments arguments)
		{
			if (arguments.UidText.IsNullOrEmpty())
				throw new ArgumentNullException(nameof(MobileServiceCardLinkArguments.UidText));

			var paymentConcessions = (await PaymentConcessionRepository.GetAsync())
				.Where(x => x.Id == arguments.PaymentConcessionId);
			var cards = (await Repository.GetAsync(nameof(ServiceCard.LinkedUsers), nameof(ServiceCard.OwnerUser)))
				.Where(x =>
                    (
						(x.SystemCard.ClientId == null) ||
						(x.SystemCard.ClientId == SessionData.ClientId) ||
						(SessionData.ClientId == "PayInWebApp")
					) &&
 					(x.UidText == arguments.UidText) &&
					(paymentConcessions
						.Any(y => y.ConcessionId == x.ConcessionId)
					)
				)
                .ToList();
            if (!cards.Any())
                throw new ArgumentNullException(nameof(MobileServiceCardLinkArguments.UidText));
            
			foreach(var card in cards)
				await ExecuteInternalAsync(card);

			return cards.FirstOrDefault();
		}
        #endregion ExecuteAsync

        #region ExecuteInternalAsync
        public async Task<ServiceCard> ExecuteInternalAsync(ServiceCard card)
        {
            if ((card.State != ServiceCardState.Emited) && (card.State != ServiceCardState.Active))
                throw new ApplicationException("Tarjeta no emitida");
            if (card.LinkedUsers.Any(x => x.Login == SessionData.Login))
                return card;
            card.LinkedUsers.Add(new ServiceUserLink { Login = SessionData.Login });

			// Existe un caso que no tiene sentido en fallas que es el caso de que se vincule una pulsera anónima
			// En este asco debería asignasele al vinculador como propietario vinculado
			// Actualizar el owner si se vincula este usuario
			//if (card.OwnerLogin.IsNullOrEmpty())
			//	throw new ApplicationException("Tarjeta no asignada");

			return card;
        }
		#endregion ExecuteInternalAsync
	}
}
