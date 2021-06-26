using PayIn.Common;
using Xp.Common.Dto.Arguments;

namespace PayIn.Application.Dto.Arguments
{
	public partial class TranslationCreateArguments : IArgumentsBase
	{
		public int? NoticeId { get; set; }
		public int? EventId { get; set; }
		public TranslationType TranslationType { get; set; }
		public LanguageEnum TranslateTo { get; set; } = LanguageEnum.New;
		public string TranslatedText { get; set; }

		#region Constructors
		public TranslationCreateArguments(int? noticeId, int? eventId, TranslationType translationType, LanguageEnum translateTo, string translatedText)
		{
			NoticeId = noticeId;
			EventId = eventId;
			TranslationType = translationType;
			TranslateTo = translateTo;
			TranslatedText = translatedText;
		}
		#endregion Constructos
	}
}
