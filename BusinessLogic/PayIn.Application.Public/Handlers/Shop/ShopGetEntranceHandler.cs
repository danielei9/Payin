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
    public class ShopGetEntranceHandler :
        IQueryBaseHandler<ShopGetEntranceArguments, ShopGetEntranceResult>
    {
        private readonly IEntityRepository<EntranceType> EntranceTypeRepository;
        private readonly IEntityRepository<EntranceTypeForm> EntranceTypeFormRepository;
        private readonly ISessionData SessionData;

        #region Constructors
        public ShopGetEntranceHandler(
            IEntityRepository<EntranceType> entranceTypeRepository,
            IEntityRepository<EntranceTypeForm> entranceTypeFormRepository,
            ISessionData sessionData
            )
        {
            if (entranceTypeRepository == null) throw new ArgumentNullException("entranceTypeRepository");
            if (entranceTypeFormRepository == null) throw new ArgumentNullException("entranceTypeFormRepository");
            if (sessionData == null) throw new ArgumentNullException("sessionData");

            EntranceTypeRepository = entranceTypeRepository;
            EntranceTypeFormRepository = entranceTypeFormRepository;
            SessionData = sessionData;
        }
        #endregion Constructors


        #region ExecuteAsync
        public async Task<ResultBase<ShopGetEntranceResult>> ExecuteAsync(ShopGetEntranceArguments arguments)
        {
            var EntranceType = (await EntranceTypeRepository.GetAsync())
                .Where(x =>
                    (x.State == EntranceTypeState.Active &&
                     x.Id == arguments.Id &&
                     x.Event.State == EventState.Active)
                );

            var forms = (await EntranceTypeFormRepository.GetAsync())
                .Where(x =>
                   x.EntranceTypeId == arguments.Id);

            var formList = "";

            foreach(var formId in forms)
            {
                formList += formId.FormId.ToString() + ",";
            }

            if(formList.Length >=1)
                formList= formList.Remove(formList.Length - 1);

            var result = EntranceType
                .Select(x => new
                {
                    Id = x.Id,
                    Name = x.Name,                  
                    EventName = x.Event.Name,
                    Place = x.Event.Place,
                    EventStart = x.Event.EventStart,
                    EventEnd = x.Event.EventEnd,
                    PhotoUrl = x.Event.PhotoUrl,
                    Description = x.Event.Description,
                    ShortDescription = x.Event.ShortDescription,
                    Conditions = x.Event.Conditions,
                    ConcessionId = x.Event.PaymentConcessionId,                            
                    EntranceConditions = x.Conditions,
                    EntranceDescription = x.Description,
                    EntranceShortDescription = x.ShortDescription,
                    Price = x.Price
                }).ToList()
                        .Select(x => new ShopGetEntranceResult
                        {
                            Id = x.Id,
                            Name = x.Name,
                            Place = x.Place,
                            EventName = x.EventName,
                            EventStart = (x.EventStart == XpDateTime.MinValue) ? (DateTime?)null : x.EventStart.ToUTC(),
                            EventEnd = (x.EventEnd == XpDateTime.MinValue) ? (DateTime?)null : x.EventEnd.ToUTC(),
                            PhotoUrl = x.PhotoUrl,
                            Description = x.Description,
                            ShortDescription = x.ShortDescription,
                            Conditions = x.Conditions,
                            Price = x.Price,
                            EntranceDescription = x.EntranceDescription,
                            EntranceShortDescription = x.EntranceShortDescription,
                            ConcessionId = x.ConcessionId,
                            FormId= formList ?? ""
                        });

            return new ResultBase<ShopGetEntranceResult> { Data = result };
        }
        #endregion ExecuteAsync
    }
}
