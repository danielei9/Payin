using PayIn.Application.Dto.Arguments.Shop;
using PayIn.Application.Dto.Results.Shop;
using PayIn.BusinessLogic.Common;
using PayIn.Common;
using PayIn.Domain.Payments;
using PayIn.Domain.Public;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xp.Application.Dto;
using Xp.Common;
using Xp.Domain;

namespace PayIn.Application.Public.Handler.Shop
{
    public class ShopEventGetEntranceTypesHandler :
        IQueryBaseHandler<ShopEventGetEntranceTypesArguments, ShopEventGetEntranceTypesResult>
    {
        private readonly IEntityRepository<EntranceType> EntranceTypeRepository;
        private readonly ISessionData SessionData;

        #region Constructors
        public ShopEventGetEntranceTypesHandler(
            IEntityRepository<EntranceType> entranceTypeRepository,
            ISessionData sessionData
            )
        {
            if (entranceTypeRepository == null) throw new ArgumentNullException("entranceTypeRepository");
            if (sessionData == null) throw new ArgumentNullException("sessionData");


            EntranceTypeRepository = entranceTypeRepository;
            SessionData = sessionData;
        }
        #endregion Constructors


        #region ExecuteAsync
        public async Task<ResultBase<ShopEventGetEntranceTypesResult>> ExecuteAsync(ShopEventGetEntranceTypesArguments arguments)
        {

            var result = (await EntranceTypeRepository.GetAsync())
                .Where(x =>
                     x.Event.PaymentConcession.Concession.State == ConcessionState.Active &&
                     x.Event.State == EventState.Active &&
                     x.State == EntranceTypeState.Active &&
                     x.EventId == arguments.Id )
                .Select(x => new
                    {
                        Id = x.Id,
                        Place = x.Event.Place,
                        Name = x.Event.Name,
                        EventStart = x.Event.EventStart,
                        PhotoUrl = x.Event.PhotoUrl,
                        EntranceName = x.Name,
                        EntrancePrice = x.Price,
                        EntrancePhotoUrl = x.PhotoUrl
                }).ToList()
                    .Select(x => new ShopEventGetEntranceTypesResult
                    {
                        Id = x.Id,
                        Place = x.Place,
                        Name = x.Name,
                        EventStart = (x.EventStart == XpDateTime.MinValue) ? (DateTime?)null : x.EventStart.ToUTC(),
                        PhotoUrl = x.PhotoUrl,
                        EntranceName =x.EntranceName ,
                        EntrancePrice = x.EntrancePrice //,
                        //EntrancePhotoUrl= x.EntrancePhotoUrl

                    });

            return new ResultBase<ShopEventGetEntranceTypesResult> { Data = result };
        }
        #endregion ExecuteAsync
    }
}
