using Newtonsoft.Json;
using PayIn.Common;
using System.ComponentModel.DataAnnotations;
using Xp.Common;
using Xp.Common.Dto.Arguments;

namespace PayIn.Application.Dto.Payments.Arguments
{
	public partial class ApiNoticeUpdateEdictArguments : IArgumentsBase
	{
		[JsonIgnore] public int Id { get; set; }

		[Display(Name = "resources.notice.longitude")]
		public decimal? Longitude { get; set; }

		[Display(Name = "resources.notice.latitude")]
		public decimal? Latitude { get; set; }

		[Display(Name = "resources.notice.place")]
		public string Place { get; set; }

		[Display(Name = "resources.notice.start")]
		public XpDateTime Start { get; set; }

		[Display(Name = "resources.notice.end")]
		public XpDateTime End { get; set; }

		[Display(Name = "resources.notice.name")]
		[Required(AllowEmptyStrings = false)]
        [Multilanguage("translationsName", "{id:item.id||0,translationType:0,noticeId:arguments.id}")]
        public string Name { get; set; }

		[Display(Name = "resources.notice.shortDescription")]
        [Multilanguage("translationsShortDescription", "{id:item.id||0,translationType:2,noticeId:arguments.id}")]
        public string ShortDescription { get; set; }

		[Display(Name = "resources.notice.description")]
        [Multilanguage("translationsDescription", "{id:item.id||0,translationType:1,noticeId:arguments.id}")]
        [Formatable]
        public string Description { get; set; }

		[Display(Name = "resources.notice.isVisible")]
		public bool IsVisible { get; set; }

		[Display(Name = "resources.notice.visibility")]
		public NoticeVisibility Visibility { get; set; }

		[Display(Name = "resources.notice.state")]
		public NoticeState State { get; set; }

		[Display(Name = "resources.notice.sendNotification")]
		public bool SendNotification { get; set; } = false;

		#region Constructors
		public ApiNoticeUpdateEdictArguments(int id, string shortDescription, string description, bool isVisible, NoticeVisibility visibility, NoticeState state, decimal? longitude, decimal? latitude, string place, XpDateTime start, XpDateTime end, bool sendNotification)
		{
			Id = id;
			ShortDescription = shortDescription;
			Description = description;
			IsVisible = isVisible;
			Visibility = visibility;
			State = state;
			Longitude = longitude;
			Latitude = latitude;
			Place = place;
			Start = start;
			End = end;
			SendNotification = sendNotification;
		}
		#endregion Constructors
	}
}
