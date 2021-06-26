using PayIn.Common;
using PayIn.Domain.Payments;
using Xp.Common.Dto.Arguments;

namespace PayIn.Application.Dto.Arguments
{
	public partial class TranslationUpdateArguments : IUpdateArgumentsBase<Translation>
	{
		public int Id { get; set; }

		public string TranslatedText { get; set; }

		#region Constructors
		public TranslationUpdateArguments(int id, string translatedText) //, LanguageEnum translateTo, int? noticeId, int? eventId, TranslationType translationType)
		{
			Id = id;
			TranslatedText = translatedText;
		}
		#endregion Constructos
	}
}
