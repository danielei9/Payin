using Microsoft.Practices.Unity;
using PayIn.Application.Dto.Arguments;
using PayIn.Application.Dto.Arguments.Notification;
using PayIn.BusinessLogic.Common;
using PayIn.Common;
using PayIn.Domain.Public;
using System;
using System.Linq;
using System.Threading.Tasks;
using Xp.Application.Dto;
using Xp.Domain;

namespace PayIn.Application.Public.Handlers
{
    public class MobileServiceCardUnlinkHandler : IServiceBaseHandler<MobileServiceCardUnlinkArguments>
	{
		[Dependency] public ISessionData SessionData { get; set; }
		[Dependency] public IEntityRepository<ServiceCard> Repository { get; set; }
		[Dependency] public IEntityRepository<ServiceUser> ServiceUserRepository { get; set; }
		[Dependency] public ServiceNotificationCreateHandler ServiceNotificationCreate { get; set; }

        #region ExecuteAsync
        public async Task<dynamic> ExecuteAsync(MobileServiceCardUnlinkArguments arguments)
        {
            var card = (await Repository.GetAsync(arguments.Id, nameof(ServiceCard.LinkedUsers)));
            if (card == null)
                throw new ArgumentNullException(nameof(MobileServiceCardUnlinkArguments.Id));

            return ExecuteInternalAsync(card, arguments.LinkedToLogin);
        }
        #endregion ExecuteAsync

        #region ExecuteInternalAsynct
        public async Task<ServiceCard> ExecuteInternalAsync(ServiceCard card, string linkedToLogin)
		{
			if ((card.State != ServiceCardState.Emited) && (card.State != ServiceCardState.Active))
				throw new ApplicationException("Tarjeta no emitida");
					   
			var linkedLogin = SessionData.Login;

			if (linkedToLogin != null && linkedToLogin != "")
				linkedLogin = linkedToLogin;

			var linkedUsers = card.LinkedUsers
				.Where(x => x.Login == linkedLogin)
				.ToList();

			if (linkedUsers.Count == 0)
				throw new ApplicationException("No existe relación entre las tarjetas a desvincular");

			var cuenta = linkedUsers.Count;
			foreach (var user in linkedUsers)
            {
                card.LinkedUsers.Remove(user);

				// Existe un caso que no tiene sentido en fallas que es el caso de que se vincule una pulsera anónima
				// En este asco debería eliminarse al vinculador como propietario vinculado
				//if (card.OwnerLogin == SessionData.Login)
				//    card.OwnerLogin = card.OwnerUser?.Login ?? "";
			}
			if (card.LinkedUsers.Count == cuenta)
				throw new ApplicationException("No se ha desvinculado ninguna tarjeta");

			return card;
        }
        #endregion ExecuteInternalAsync
    }
}
