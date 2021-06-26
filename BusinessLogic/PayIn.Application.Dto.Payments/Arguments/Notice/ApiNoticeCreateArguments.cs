using PayIn.Common;
using System.ComponentModel.DataAnnotations;
using Xp.Common;
using Xp.Common.Dto.Arguments;

namespace PayIn.Application.Dto.Payments.Arguments
{
	public partial class ApiNoticeCreateArguments : IArgumentsBase
	{
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
		public string Name { get; set; }

		[Display(Name = "resources.notice.shortDescription")]
		public string ShortDescription { get; set; }

		[Display(Name = "resources.notice.description")]
        [Formatable]
        public string Description { get; set; }

		[Display(Name = "resources.notice.isVisible")]
		public bool IsVisible { get; set; }

		[Display(Name = "resources.notice.visibility")]
		public NoticeVisibility Visibility { get; set; }

		[Display(Name = "resources.notice.eventId")]
		public int? EventId { get; set; }

		[Display(Name = "resources.notice.sendNotification")]
		public bool SendNotification { get; set; } = true;

		#region Constructors
		public ApiNoticeCreateArguments(string shortDescription, string description, bool isVisible, NoticeVisibility visibility, int? eventId, decimal? longitude, decimal? latitude, string place, XpDateTime start, XpDateTime end, bool sendNotification)
		{
			ShortDescription = shortDescription ?? "";
			Description = description ?? "";
			IsVisible = isVisible;
			Visibility = visibility;
			EventId = eventId;
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
