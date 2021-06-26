using Microsoft.Practices.Unity;
using PayIn.Application.Dto.Arguments;
using PayIn.Application.Dto.Payments.Results.Translation;
using PayIn.Application.Dto.Results;
using PayIn.BusinessLogic.Common;
using PayIn.Common;
using PayIn.Domain.Payments;
using PayIn.Domain.Public;
using PayIn.Domain.Security;
using PayIn.Domain.Transport;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xp.Application.Dto;
using Xp.Domain;

namespace PayIn.Application.Public.Handlers
{
    public class TranslationGetHandler :
        IQueryBaseHandler<TranslationGetArguments, TranslationGetResult>
    {
		[Dependency] public IEntityRepository<Translation> Repository { get; set; }
		[Dependency] public IEntityRepository<Notice> NoticesRepository { get; set; }
		[Dependency] public IEntityRepository<Event> EventsRepository { get; set; }

		#region ExecuteAsync
		public async Task<ResultBase<TranslationGetResult>> ExecuteAsync(TranslationGetArguments arguments)
		{
			var result = new TranslationGetResult();

			if (arguments.Id != null)
			{
				var translatedText = (await Repository.GetAsync())
					.Where(x =>
						x.Id == arguments.Id
					)
					.Select(x => new
					{
						x.Id,
						NoticeId = x.NoticeNameId != null ? x.NoticeNameId :
								  x.NoticeShortDescriptionId != null ? x.NoticeShortDescriptionId :
								  x.NoticeDescriptionId != null ? x.NoticeDescriptionId :
								  x.NoticePlaceId != null ? x.NoticePlaceId : null,
						EventId = x.EventNameId != null ? x.EventNameId :
								 x.EventShortDescriptionId != null ? x.EventShortDescriptionId :
								 x.EventDescriptionId != null ? x.EventDescriptionId :
								 x.EventPlaceId != null ? x.EventPlaceId :
								 x.EventConditionsId != null ? x.EventConditionsId : null,
						Type = x.NoticeNameId != null || x.EventNameId != null ? TranslationType.Name :
							   x.NoticeShortDescriptionId != null || x.EventShortDescriptionId != null ? TranslationType.ShortDescription :
							   x.NoticeDescriptionId != null || x.EventDescriptionId != null ? TranslationType.Description :
							   x.NoticePlaceId != null || x.EventPlaceId != null ? TranslationType.Place : TranslationType.Conditions,
						Translated = x.Text,
						x.Language
					})
					.FirstOrDefault();

				if (translatedText == null)
					return null;
					//throw new ArgumentException("translationId");

				TranslationType translationType = translatedText.Type;
				string originalText = "";
				if (translatedText.NoticeId != null)
				{
					var originalNotice = (await NoticesRepository.GetAsync())
						.Where(x =>
							x.Id == translatedText.NoticeId
						)
						.Select(x => new
						{
							Text = translationType == TranslationType.Name ? x.Name :
									translationType == TranslationType.ShortDescription ? x.ShortDescription :
									translationType == TranslationType.Description ? x.Description :
									translationType == TranslationType.Place ? x.Place : ""
						})
						.FirstOrDefault();
					if (originalNotice != null)
						originalText = originalNotice.Text;
				}
				else
				{
					var originalEvent = (await EventsRepository.GetAsync())
						.Where(x =>
							x.Id == translatedText.EventId
						)
						.Select(x => new
						{
							Text = translationType == TranslationType.Name ? x.Name :
									translationType == TranslationType.ShortDescription ? x.ShortDescription :
									translationType == TranslationType.Description ? x.Description :
									translationType == TranslationType.Place ? x.Place :
									translationType == TranslationType.Conditions ? x.Conditions : ""
						})
						.FirstOrDefault();
					if (originalEvent != null)
						originalText = originalEvent.Text;
				}


				result = new TranslationGetResult
				{
					Id = translatedText.Id,
					Language = translatedText.Language,
					OriginalText = originalText,
					TranslatedText = translatedText.Translated,
					NoticeNameId = translatedText.Type == TranslationType.Name ? translatedText.NoticeId : null,
					NoticeShortDescriptionId = translatedText.Type == TranslationType.ShortDescription ? translatedText.NoticeId : null,
					NoticeDescriptionId = translatedText.Type == TranslationType.Description ? translatedText.NoticeId : null,
					NoticePlaceId = translatedText.Type == TranslationType.Place ? translatedText.NoticeId : null,
					EventNameId = translatedText.Type == TranslationType.Name ? translatedText.EventId : null,
					EventShortDescriptionId = translatedText.Type == TranslationType.ShortDescription ? translatedText.EventId : null,
					EventDescriptionId = translatedText.Type == TranslationType.Description ? translatedText.EventId : null,
					EventPlaceId = translatedText.Type == TranslationType.Place ? translatedText.EventId : null,
					EventConditionsId = translatedText.Type == TranslationType.Conditions ? translatedText.EventId : null
				};
			}
			else
			{
				var argNoticeId = arguments.NoticeId ?? 0;
				var argEventId = arguments.EventId ?? 0;
				LanguageEnum argLang = arguments.Language;

				result.Language = arguments.Language;
				if (arguments.TranslationType == TranslationType.Name && argNoticeId > 0) result.NoticeNameId = argNoticeId;
				else if (arguments.TranslationType == TranslationType.ShortDescription && argNoticeId > 0) result.NoticeShortDescriptionId = argNoticeId;
				else if (arguments.TranslationType == TranslationType.Description && argNoticeId > 0) result.NoticeDescriptionId = argNoticeId;
				else if (arguments.TranslationType == TranslationType.Place && argNoticeId > 0) result.NoticePlaceId = argNoticeId;
				else if (arguments.TranslationType == TranslationType.Name && argEventId > 0) result.EventNameId = argEventId;
				else if (arguments.TranslationType == TranslationType.ShortDescription && argEventId > 0) result.EventShortDescriptionId = argEventId;
				else if (arguments.TranslationType == TranslationType.Description && argEventId > 0) result.EventDescriptionId = argEventId;
				else if (arguments.TranslationType == TranslationType.Place && argEventId > 0) result.EventPlaceId = argEventId;
				else if (arguments.TranslationType == TranslationType.Conditions && argEventId > 0) result.EventConditionsId = argEventId;

				var isNotice = argNoticeId > 0;

				if (isNotice)
				{
					var originalNotice = (await NoticesRepository.GetAsync())
						.Where(x =>
							x.Id == argNoticeId
						)
						.Select(x => new
						{
							Text = arguments.TranslationType == TranslationType.Name ? x.Name :
									arguments.TranslationType == TranslationType.ShortDescription ? x.ShortDescription :
									arguments.TranslationType == TranslationType.Description ? x.Description :
									arguments.TranslationType == TranslationType.Place ? x.Place : ""
						})
						.FirstOrDefault();
					if (originalNotice != null)
						result.OriginalText = originalNotice.Text;
				}
				else
				{
					var originalEvent = (await EventsRepository.GetAsync())
						.Where(x =>
							x.Id == argEventId
						)
						.Select(x => new
						{
							Text = arguments.TranslationType == TranslationType.Name ? x.Name :
									arguments.TranslationType == TranslationType.ShortDescription ? x.ShortDescription :
									arguments.TranslationType == TranslationType.Description ? x.Description :
									arguments.TranslationType == TranslationType.Place ? x.Place : ""
						})
						.FirstOrDefault();
					if (originalEvent != null)
						result.OriginalText = originalEvent.Text;
				}

				if (argLang != LanguageEnum.New)
				{
					var translations = (await Repository.GetAsync())
						.Where(x =>
							x.Language == argLang
						);

					var translation = translations
						.Where(x =>
							(arguments.TranslationType == TranslationType.Name && x.NoticeNameId == argNoticeId) ||
							(arguments.TranslationType == TranslationType.ShortDescription && x.NoticeShortDescriptionId == argNoticeId) ||
							(arguments.TranslationType == TranslationType.Description && x.NoticeDescriptionId == argNoticeId) ||
							(arguments.TranslationType == TranslationType.Place && x.NoticePlaceId == argNoticeId) ||

							(arguments.TranslationType == TranslationType.Name && x.EventNameId == argEventId) ||
							(arguments.TranslationType == TranslationType.ShortDescription && x.EventShortDescriptionId == argEventId) ||
							(arguments.TranslationType == TranslationType.Description && x.EventDescriptionId == argEventId) ||
							(arguments.TranslationType == TranslationType.Place && x.EventPlaceId == argEventId) ||
							(arguments.TranslationType == TranslationType.Conditions && x.EventConditionsId == argEventId)
						);


					var translated = translation
						.Select(x => new
						{
							x.Id,
							x.Text
						})
						.FirstOrDefault();

					result.Id = translated.Id;

					var translatedText = translated == null ? "" : translated.Text;
					result.TranslatedText = translatedText;
				}
			}
			
			return new ResultBase<TranslationGetResult> { Data = new List<TranslationGetResult> { result } };
		}
		#endregion ExecuteAsync
	}
}
