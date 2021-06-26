using PayIn.Common;
using System.ComponentModel.DataAnnotations;
using Xp.Common;
using Xp.Common.Dto.Arguments;

namespace PayIn.Application.Dto.Payments.Arguments
{
	public partial class ApiNoticeCreatePageArguments : IArgumentsBase
	{
		[Display(Name = "resources.notice.longitude")]
		public decimal? Longitude { get; set; }

		[Display(Name = "resources.notice.latitude")]
		public decimal? Latitude { get; set; }

		[Display(Name = "resources.notice.place")]
		public string Place { get; set; }

		[Display(Name = "resources.notice.superNoticeId")]
		public int? SuperNoticeId { get; set; }

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

		[Display(Name = "resources.notice.sendNotification")]
		public bool SendNotification { get; set; } = false;

		#region Constructors
		public ApiNoticeCreatePageArguments (string shortDescription, string description, bool isVisible, NoticeVisibility visibility, decimal? longitude, decimal? latitude, string place, int? superNoticeId, bool sendNotification)
		{
			ShortDescription = shortDescription ?? "";
			Description = description ?? "";
			IsVisible = isVisible;
			Visibility = visibility;
			Longitude = longitude;
			Latitude = latitude;
			Place = place;
			SuperNoticeId = superNoticeId;
			SendNotification = sendNotification;
		}
		#endregion Constructors
	}
}
