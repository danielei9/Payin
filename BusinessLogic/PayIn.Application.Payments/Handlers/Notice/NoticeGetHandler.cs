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
using static PayIn.Application.Dto.Payments.Results.NoticeGetResult;

namespace PayIn.Application.Payments.Handlers
{
	public class NoticeGetHandler :
		IQueryBaseHandler<NoticeGetArguments, NoticeGetResult>
	{
		[Dependency] public IEntityRepository<Notice> Repository { get; set; }
		[Dependency] public IEntityRepository<Translation> TranslationRepository { get; set; }

		#region ExecuteAsync
		public async Task<ResultBase<NoticeGetResult>> ExecuteAsync(NoticeGetArguments arguments)
		{
			var translationsName = (await TranslationRepository.GetAsync())
				.Where(x => x.NoticeNameId == arguments.Id)
				.Select(x => new NoticeTranslation
				{
					Id = x.Id,
					Language = x.Language
				});

			var translationsDescription = (await TranslationRepository.GetAsync())
				.Where(x => x.NoticeDescriptionId == arguments.Id)
				.Select(x => new NoticeTranslation
				{
					Id = x.Id,
					Language = x.Language
				});

			var translationsShortDescription = (await TranslationRepository.GetAsync())
				.Where(x => x.NoticeShortDescriptionId == arguments.Id)
				.Select(x => new NoticeTranslation
				{
					Id = x.Id,
					Language = x.Language
				});

			var translationsPlace = (await TranslationRepository.GetAsync())
				.Where(x => x.NoticePlaceId == arguments.Id)
				.Select(x => new NoticeTranslation
				{
					Id = x.Id,
					Language = x.Language
				});

			var item = (await Repository.GetAsync())
				.Where(x => x.Id == arguments.Id);

			var result = item
				.Select(x => new 
				{
					x.Id,
					x.Name,
					x.ShortDescription,
					x.Description,
					x.IsVisible,
					x.State,
					x.Visibility,
					x.PhotoUrl,
					x.RouteUrl,
					Start = (x.Start == XpDateTime.MinValue) ? null : (DateTime?)x.Start,
					x.EventId,
					EventName = x.Event.Name ?? "",
					TranslationsName = translationsName,
					TranslationsDescription = translationsDescription,
					TranslationsShortDescription = translationsShortDescription,
					TranslationsPlace = translationsPlace,
					x.Place,
					x.Longitude,
					x.Latitude,
					End = (x.End == XpDateTime.MaxValue) ? null : (DateTime?)x.End,
					x.SuperNoticeId,
					SuperNoticeName = x.SuperNotice.Name
				})
				.ToList()
				.Select(x => new NoticeGetResult
				{
					Id = x.Id,
					Name = x.Name,
					ShortDescription = x.ShortDescription,
					Description = x.Description,
					IsVisible = x.IsVisible,
					State = x.State,
					Visibility = x.Visibility,
					PhotoUrl = x.PhotoUrl,
					RouteUrl = x.RouteUrl,
					RouteFileName = x.RouteUrl.Split(new char[] { '/' }).Last(),
					Start = x.Start.ToUTC(),
					EventId = x.EventId,
					EventName = x.EventName,
					TranslationsName = x.TranslationsName,
					TranslationsDescription = x.TranslationsDescription,
					TranslationsShortDescription = x.TranslationsShortDescription,
					TranslationsPlace = x.TranslationsPlace,
					Place = x.Place,
					Longitude = x.Longitude,
					Latitude = x.Latitude,
					End = x.End.ToUTC(),
					SuperNoticeId = x.SuperNoticeId,
					SuperNoticeName = x.SuperNoticeName
				});

			return new ResultBase<NoticeGetResult> { Data = result };
		}
		#endregion ExecuteAsync
	}
}
