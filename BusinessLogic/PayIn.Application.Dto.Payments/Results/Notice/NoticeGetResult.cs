using System.Collections.Generic;
using PayIn.Common;
using Xp.Common;

namespace PayIn.Application.Dto.Payments.Results
{
	public partial class NoticeGetResult
	{
		public class NoticeTranslation
		{
			public int Id;
			public LanguageEnum Language;
		}

		public int Id { get; set; }
		public string Name { get; set; }
		public string ShortDescription { get; set; }
		public string Description { get; set; }
		public bool IsVisible { get; set; }
		public string PhotoUrl { get; set; }
		public string RouteFileName { get; set; }
		public string RouteUrl { get; set; }
		public NoticeState State { get; set; }
		public NoticeVisibility Visibility { get; set; }
		public XpDateTime Start { get; set; }
		public int? EventId { get; set; }
		public string EventName { get; set; }
		public IEnumerable<NoticeTranslation> TranslationsName { get; set; }
		public IEnumerable<NoticeTranslation> TranslationsDescription { get; set; }
		public IEnumerable<NoticeTranslation> TranslationsShortDescription { get; set; }
		public IEnumerable<NoticeTranslation> TranslationsPlace { get; set; }

		public decimal? Longitude { get; set; }
		public decimal? Latitude { get; set; }
		public string Place { get; set; }
		public XpDateTime End { get; set; }
		public NoticeType Type { get; set; }
		public int? SuperNoticeId { get; set; }
		public string SuperNoticeName { get; set; }
	}
}
