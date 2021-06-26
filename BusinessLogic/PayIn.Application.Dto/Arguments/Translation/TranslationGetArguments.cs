using PayIn.Common;
using Xp.Common.Dto.Arguments;

namespace PayIn.Application.Dto.Arguments
{
	public class TranslationGetArguments : IArgumentsBase
	{
		public int? Id { get; set; }
		public int? NoticeId { get; set; }
		public int? EventId { get; set; }
		public TranslationType TranslationType { get; set; }
		public LanguageEnum Language { get; set; }

		#region Constructors
		public TranslationGetArguments(int? id, int? noticeId, int? eventId, TranslationType translationType, LanguageEnum? language)
		{
			Id = id;
			NoticeId = noticeId;
			EventId = eventId;
			TranslationType = translationType;
			Language = language ?? LanguageEnum.New;
		}
		#endregion Constructors
	}

}
