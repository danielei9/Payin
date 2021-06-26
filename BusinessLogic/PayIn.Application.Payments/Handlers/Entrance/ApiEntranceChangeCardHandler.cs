using Microsoft.Practices.Unity;
using PayIn.Application.Dto.Payments.Arguments;
using PayIn.Common;
using PayIn.Common.Resources;
using PayIn.Domain.Payments;
using PayIn.Domain.Public;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xp.Application.Dto;
using Xp.Domain;

namespace PayIn.Application.Payments.Handlers
{
	public class ApiEntranceChangeCardHandler :
		IServiceBaseHandler<ApiEntranceChangeCardArguments>
	{
		[Dependency] public IEntityRepository<Entrance> Repository { get; set; }
        [Dependency] public IEntityRepository<ServiceCard> ServiceCardRepository { get; set; }

        #region ExecuteAsync
        public async Task<dynamic> ExecuteAsync(ApiEntranceChangeCardArguments arguments)
		{
            // Get entrance
            var entrance = (await Repository.GetAsync())
                .Where(x =>
                    x.Id == arguments.Id &&
                    x.State == EntranceState.Active
                )
                .FirstOrDefault();
            if (entrance == null)
                throw new ArgumentException(EntranceResources.EntranceNotFound);

            // Get cards
            var cards = (await ServiceCardRepository.GetAsync())
                .Where(x =>
                    x.Uid == entrance.Uid ||
                    (
                        x.UidText == arguments.UidText &&
                        x.State == ServiceCardState.Active
                    )
                )
                .ToList();
            if (cards == null)
                throw new ArgumentException(EntranceResources.CardsNotFound);

            // Get origin card
            var originCard = cards
                .Where(x =>
                    x.Uid == entrance.Uid
                )
                .FirstOrDefault();
            if (originCard == null)
                throw new ArgumentException(EntranceResources.OriginCardNotFound);

            // Get new card
            var newCard = (await ServiceCardRepository.GetAsync())
                .Where(x =>
                    x.UidText == arguments.UidText
                )
                .FirstOrDefault();
            if (newCard == null)
                throw new ArgumentException(EntranceResources.DestinationCardNotFound);

            if (originCard.SystemCardId != newCard.SystemCardId)
                throw new ArgumentException(EntranceResources.CardsNotInSameSystem);

            var newLogin = newCard.OwnerLogin;
            var newUid = newCard.Uid;

            entrance.Uid = newUid;
            entrance.Login = newLogin;

			return entrance;
		}
		#endregion ExecuteAsync
	}
}
