using Microsoft.Practices.Unity;
using PayIn.Application.Dto.Payments.Arguments;
using PayIn.Application.Dto.Payments.Results;
using PayIn.Domain.Payments;
using System;
using System.Linq;
using System.Threading.Tasks;
using Xp.Application.Dto;
using Xp.Common;
using Xp.Domain;
using static PayIn.Application.Dto.Payments.Results.EventGetResult;

namespace PayIn.Application.Payments.Handlers
{
	public class EventGetHandler :
		IQueryBaseHandler<EventGetArguments, EventGetResult>
	{
		[Dependency] public IEntityRepository<Event> Repository { get; set; }
		[Dependency] public IEntityRepository<EntranceSystem> EntranceSystemRepository { get; set; }
		[Dependency] public IEntityRepository<Translation> TranslationRepository { get; set; }

		#region ExecuteAsync
		public async Task<ResultBase<EventGetResult>> ExecuteAsync(EventGetArguments arguments)
		{
			var translationsName = (await TranslationRepository.GetAsync())
				.Where(x => x.EventNameId == arguments.Id)
				.Select(x => new EventTranslation
				{
					Id = x.Id,
					Language = x.Language
				});

			var translationsDescription = (await TranslationRepository.GetAsync())
				.Where(x => x.EventDescriptionId == arguments.Id)
				.Select(x => new EventTranslation
				{
					Id = x.Id,
					Language = x.Language
				});

			var translationsShortDescription = (await TranslationRepository.GetAsync())
				.Where(x => x.EventShortDescriptionId == arguments.Id)
				.Select(x => new EventTranslation
				{
					Id = x.Id,
					Language = x.Language
				});

			var translationsPlace = (await TranslationRepository.GetAsync())
				.Where(x => x.EventPlaceId == arguments.Id)
				.Select(x => new EventTranslation
				{
					Id = x.Id,
					Language = x.Language
				});

			var translationsConditions = (await TranslationRepository.GetAsync())
				.Where(x => x.EventConditionsId == arguments.Id)
				.Select(x => new EventTranslation
				{
					Id = x.Id,
					Language = x.Language
				});

			var item = (await Repository.GetAsync())
				.Where(x => x.Id == arguments.Id);

			var result = item
				.Select(x => new 
				{
					Id = x.Id,
                    Code = x.Code,
					Longitude = x.Longitude,
					Latitude = x.Latitude,
					Place = x.Place,
					Name = x.Name,
					Description = x.Description,
					PhotoUrl = x.PhotoUrl,
					PhotoMenuUrl = x.PhotoMenuUrl,
					Capacity = x.Capacity,
					EventStart = (x.EventStart == XpDateTime.MinValue) ? null : (DateTime?)x.EventStart,
					EventEnd = (x.EventEnd == XpDateTime.MaxValue) ? null : (DateTime?)x.EventEnd,
					CheckInStart = (x.CheckInStart == XpDateTime.MinValue) ? null : (DateTime?)x.CheckInStart,
					CheckInEnd = (x.CheckInEnd == XpDateTime.MaxValue) ? null : (DateTime?)x.CheckInEnd,
					State = x.State,
					EntraceSystemId = x.EntranceSystemId,
					EntranceSystemName = x.EntranceSystem.Name,
					ProfileId = x.ProfileId,
					ProfileName = x.Profile.Name,
					IsVisible = x.IsVisible,
                    Conditions = x.Conditions,
                    ShortDescription = x.ShortDescription,
					Visibility = x.Visibility,
                    MapUrl=x.MapUrl,
					MaxEntrancesPerCard = x.MaxEntrancesPerCard,
					MaxAmountToSpend = x.MaxAmountToSpend,
					EventImages = x.EventImages
						.Where(y => 
							y.EventId == x.Id
						)
						.Select(y=>new EventImagesGetResult
						{
							PhotoUrl = y.PhotoUrl,
							PhotoId = y.Id
						}),
					TranslationsName = translationsName,
					TranslationsDescription = translationsDescription,
					TranslationsShortDescription = translationsShortDescription,
					TranslationsPlace = translationsPlace,
					TranslationsConditions = translationsConditions
				})
				.ToList()
				.Select(x => new EventGetResult
				{
					Id = x.Id,
                    Code = x.Code,
                    Longitude = x.Longitude,
					Latitude = x.Latitude,
					Place = x.Place,
					Name = x.Name,
					Description = x.Description,
					PhotoUrl = x.PhotoUrl,
					PhotoMenuUrl = x.PhotoMenuUrl,
					Capacity = x.Capacity,
					EventStart = x.EventStart.ToUTC(),
					EventEnd = x.EventEnd.ToUTC(),
					CheckInStart = x.CheckInStart.ToUTC(),
					CheckInEnd = x.CheckInEnd.ToUTC(),
					State = x.State,
					EntranceSystemId = x.EntraceSystemId,
					EntranceSystemName = x.EntranceSystemName,
					ProfileId = x.ProfileId,
					ProfileName = x.ProfileName,
					IsVisible = x.IsVisible,
                    Conditions = x.Conditions,
                    ShortDescription = x.ShortDescription,
					Visibility = x.Visibility,
                    EventImages = x.EventImages,
					MaxEntrancesPerCard = x.MaxEntrancesPerCard,
					MaxAmountToSpend = x.MaxAmountToSpend,
					MapUrl = x.MapUrl,
					TranslationsName = x.TranslationsName,
					TranslationsDescription = x.TranslationsDescription,
					TranslationsShortDescription = x.TranslationsShortDescription,
					TranslationsPlace = x.TranslationsPlace,
					TranslationsConditions = x.TranslationsConditions
				});

			var res = (result
					.Where(x =>
						x.MaxEntrancesPerCard != int.MaxValue
					)
					.Select(x => new EventGetResult
					{
						Id = x.Id,
						Code = x.Code,
						Longitude = x.Longitude,
						Latitude = x.Latitude,
						Place = x.Place,
						Name = x.Name,
						Description = x.Description,
						PhotoUrl = x.PhotoUrl,
						PhotoMenuUrl = x.PhotoMenuUrl,
						Capacity = x.Capacity,
						EventStart = x.EventStart,
						EventEnd = x.EventEnd,
						CheckInStart = x.CheckInStart,
						CheckInEnd = x.CheckInEnd,
						State = x.State,
						EntranceSystemId = x.EntranceSystemId,
						EntranceSystemName = x.EntranceSystemName,
						ProfileId = x.ProfileId,
						ProfileName = x.ProfileName,
						IsVisible = x.IsVisible,
						Conditions = x.Conditions,
						ShortDescription = x.ShortDescription,
						Visibility = x.Visibility,
						EventImages = x.EventImages,
						MaxEntrancesPerCard = x.MaxEntrancesPerCard,
						MaxAmountToSpend = x.MaxAmountToSpend,
						MapUrl = x.MapUrl,
						TranslationsName = x.TranslationsName,
						TranslationsDescription = x.TranslationsDescription,
						TranslationsShortDescription = x.TranslationsShortDescription,
						TranslationsPlace = x.TranslationsPlace,
						TranslationsConditions = x.TranslationsConditions
					})
				)
				.Union
				(result
					.Where(x =>
						x.MaxEntrancesPerCard == int.MaxValue
					)
					.Select(x => new EventGetResult
					{
						Id = x.Id,
						Code = x.Code,
						Longitude = x.Longitude,
						Latitude = x.Latitude,
						Place = x.Place,
						Name = x.Name,
						Description = x.Description,
						PhotoUrl = x.PhotoUrl,
						PhotoMenuUrl = x.PhotoMenuUrl,
						Capacity = x.Capacity,
						EventStart = x.EventStart,
						EventEnd = x.EventEnd,
						CheckInStart = x.CheckInStart,
						CheckInEnd = x.CheckInEnd,
						State = x.State,
						EntranceSystemId = x.EntranceSystemId,
						EntranceSystemName = x.EntranceSystemName,
						ProfileId = x.ProfileId,
						ProfileName = x.ProfileName,
						IsVisible = x.IsVisible,
						Conditions = x.Conditions,
						ShortDescription = x.ShortDescription,
						Visibility = x.Visibility,
						EventImages = x.EventImages,
						MaxEntrancesPerCard = null,
						MaxAmountToSpend = x.MaxAmountToSpend,
						MapUrl = x.MapUrl,
						TranslationsName = x.TranslationsName,
						TranslationsDescription = x.TranslationsDescription,
						TranslationsShortDescription = x.TranslationsShortDescription,
						TranslationsPlace = x.TranslationsPlace,
						TranslationsConditions = x.TranslationsConditions
					})
				);

			// EntranceSystems
			var entranceSystems = (await EntranceSystemRepository.GetAsync())
                .Select(x => new SelectorResult
                {
                    Id = x.Id,
                    Value = x.Name
                })
                .OrderBy(x => x.Value)
                .ToList();

            return new EventGetResultBase
            {
                EntranceSystemId = entranceSystems,
                Data = res
            };
		}
		#endregion ExecuteAsync
	}
}
