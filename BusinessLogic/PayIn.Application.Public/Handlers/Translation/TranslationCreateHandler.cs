using Microsoft.Practices.Unity;
using PayIn.Application.Dto.Arguments;
using PayIn.Common;
using PayIn.Domain.Payments;
using PayIn.Domain.Public;
using System;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Xp.Application.Dto;
using Xp.Domain;


namespace PayIn.Application.Public.Handlers
{
	public class TranslationCreateHandler : IServiceBaseHandler<TranslationCreateArguments>
    {
		[Dependency] public IEntityRepository<Translation> Repository { get; set; }

		#region ExecuteAsync
		public async Task<dynamic> ExecuteAsync(TranslationCreateArguments arguments)
		{
			var noticeId = arguments.NoticeId;
			var eventId = arguments.EventId;
			var translationType = arguments.TranslationType;

			var duplicated = (await Repository.GetAsync())
				.Where(x =>
					x.Language == arguments.TranslateTo &&
					(
						(
							noticeId > 0 &&
							(
								(translationType == TranslationType.Name && x.NoticeNameId == noticeId) ||
								(translationType == TranslationType.ShortDescription && x.NoticeShortDescriptionId == noticeId) ||
								(translationType == TranslationType.Description && x.NoticeDescriptionId == noticeId) ||
								(translationType == TranslationType.Place && x.NoticeNameId == noticeId)
							)
						) ||
						(
							eventId > 0 &&
							(
								(translationType == TranslationType.Name && x.EventNameId == eventId) ||
								(translationType == TranslationType.ShortDescription && x.EventShortDescriptionId == eventId) ||
								(translationType == TranslationType.Description && x.EventDescriptionId == eventId) ||
								(translationType == TranslationType.Place && x.EventPlaceId == eventId) ||
								(translationType == TranslationType.Conditions && x.EventConditionsId == eventId)
							)
						)
					)
				)
				.FirstOrDefault();
			if (duplicated != null)
				throw new ApplicationException(String.Format("Ya existe una traducción del texto en {0}", arguments.TranslateTo));

			var translation = new Translation
			{
				NoticeNameId = (arguments.TranslationType == TranslationType.Name && arguments.NoticeId > 0) ? arguments.NoticeId : null,
				NoticeShortDescriptionId = (arguments.TranslationType == TranslationType.ShortDescription && arguments.NoticeId > 0) ? arguments.NoticeId : null,
				NoticeDescriptionId = (arguments.TranslationType == TranslationType.Description && arguments.NoticeId > 0) ? arguments.NoticeId : null,
				NoticePlaceId = (arguments.TranslationType == TranslationType.Place && arguments.NoticeId > 0) ? arguments.NoticeId : null,
				EventNameId = (arguments.TranslationType == TranslationType.Name && arguments.EventId > 0) ? arguments.EventId : null,
				EventShortDescriptionId = (arguments.TranslationType == TranslationType.ShortDescription && arguments.EventId > 0) ? arguments.EventId : null,
				EventDescriptionId = (arguments.TranslationType == TranslationType.Description && arguments.EventId > 0) ? arguments.EventId : null,
				EventPlaceId = (arguments.TranslationType == TranslationType.Place && arguments.EventId > 0) ? arguments.EventId : null,
				EventConditionsId = (arguments.TranslationType == TranslationType.Conditions && arguments.EventId > 0) ? arguments.EventId : null,
				Language = arguments.TranslateTo,
				Text = arguments.TranslatedText
			};

			await Repository.AddAsync(translation);

			return translation;
		}
		#endregion ExecuteAsync
	}
}

