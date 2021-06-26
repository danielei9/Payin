using PayIn.Common;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Xp.Domain;

namespace PayIn.Application.Dto.Payments.Results.Translation
{
	public partial class TranslationGetResult
	{
		public int Id { get; set; }

		public int? NoticeNameId { get; set; }
		public int? NoticeDescriptionId { get; set; }
		public int? NoticeShortDescriptionId { get; set; }
		public int? NoticePlaceId { get; set; }

		public int? EventNameId { get; set; }
		public int? EventDescriptionId { get; set; }
		public int? EventShortDescriptionId { get; set; }
		public int? EventPlaceId { get; set; }
		public int? EventConditionsId { get; set; }

		public LanguageEnum Language { get; set; }
		[Display(Name = "resources.translation.originalText")]
		public string OriginalText { get; set; }
		[Display(Name = "resources.translation.translatedText")]
		public string TranslatedText { get; set; }
	}
}
