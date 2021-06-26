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
    public class ShopGetSelectorHandler :
        IQueryBaseHandler<ShopGetSelectorArguments, ShopGetSelectorResult>
    {
        private readonly IEntityRepository<Event> Repository;
        private readonly IEntityRepository<EventImage> ImagesRepository;
        private readonly ISessionData SessionData;

        #region Constructors
        public ShopGetSelectorHandler(
            IEntityRepository<Event> repository,
            IEntityRepository<EventImage> imagesRepository,
        ISessionData sessionData
            )
        {
            if (repository == null) throw new ArgumentNullException("repository");
            if (imagesRepository == null) throw new ArgumentNullException("imagesRepository");
            if (sessionData == null) throw new ArgumentNullException("sessionData");

            Repository = repository;
            ImagesRepository=imagesRepository;
            SessionData = sessionData;
        }
        #endregion Constructors


        #region ExecuteAsync
        public async Task<ResultBase<ShopGetSelectorResult>> ExecuteAsync(ShopGetSelectorArguments arguments)
        {
            var now = DateTime.UtcNow;

            if (arguments.Filter == "undefined")
                arguments.Filter = null;

            var events = (await Repository.GetAsync())
                .Where(x =>
                    (x.State == EventState.Active &&
                     x.EntranceTypes.Count() >= 1 &&
                     x.EventStart >= now)
                );
            if (!arguments.Filter.IsNullOrEmpty())
                events = events
                    .Where(x =>
                        x.Name.Contains(arguments.Filter) ||
                        x.Place.Contains(arguments.Filter)
                    );

            var images = (await ImagesRepository.GetAsync())
                .Where(x =>
                        x.PhotoUrl != null
                       );


            var EventImagesData = new List<Object>();

            foreach (var item in images)
            {
                foreach (var eventSel in events)
                {
                    if (eventSel.Id == item.EventId)
                        EventImagesData.Add(item.PhotoUrl);
                }
            }



            var result = events
                    .Select(x => new
                    {
                        Id = x.Id,
                        Name = x.Name,
                        PhotoUrl = x.PhotoUrl,
                        Place = x.Place,
                        Date = x.EventStart,
                        PriceStart = x.EntranceTypes
                                        .OrderBy(y =>
                                                    y.Price).FirstOrDefault()
                    })
                    .ToList()
                    .Select(x => new ShopGetSelectorResult
                    {
                        Id = x.Id,
                        Name = x.Name,
                        Place = x.Place,
                        Date = (x.Date == XpDateTime.MinValue) ? (DateTime?)null : x.Date.ToUTC(),
                        PhotoUrl = x.PhotoUrl,
                        PriceStart = x.PriceStart.Price,
                        Images = EventImagesData
                    });

            return new ResultBase<ShopGetSelectorResult> { Data = result };
        }
        #endregion ExecuteAsync
    }
}
