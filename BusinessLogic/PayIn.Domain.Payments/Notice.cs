using PayIn.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Xp.Common;
using Xp.Domain;

namespace PayIn.Domain.Payments
{
	public class Notice : Entity
	{
		[Precision(9, 6)]                       public decimal? Longitude { get; set; }
		[Precision(9, 6)]                       public decimal? Latitude { get; set; }
		[Required(AllowEmptyStrings = true)]    public string Place { get; set; }
		[Required(AllowEmptyStrings = false)]	public string Name { get; set; }
		[Required(AllowEmptyStrings = true)]	public string ShortDescription { get; set; }
		[Required(AllowEmptyStrings = true)]	public string Description { get; set; }
		[Required(AllowEmptyStrings = true)]	public string PhotoUrl { get; set; }
		[Required(AllowEmptyStrings = true)]	public string RouteUrl { get; set; } = "";
												public NoticeState State { get; set; }
												public NoticeType Type { get; set; }
												public bool IsVisible { get; set; }
												public NoticeVisibility Visibility { get; set;}
												public DateTime Start { get; set; }
												public DateTime End { get; set; }

		#region Event
		public int? EventId { get; set; }
		[ForeignKey("EventId")]
		public Event Event { get; set; }
        #endregion Event

        #region SuperNotice
        public int? SuperNoticeId { get; set; }
        [ForeignKey("SuperNoticeId")]
        public Notice SuperNotice { get; set; }
        #endregion SuperNotice

        #region SubNotices
        [InverseProperty("SuperNotice")]
        public ICollection<Notice> SubNotices { get; set; } = new List<Notice>();
        #endregion SubNotices

        #region NoticeImage
        [InverseProperty("Notice")]
		public ICollection<NoticeImage> NoticeImages { get; set; } = new List<NoticeImage>();
        #endregion NoticeImage

        #region PaymentConcession
        public int PaymentConcessionId { get; set; }
        [ForeignKey("PaymentConcessionId")]
        public PaymentConcession PaymentConcession { get; set; }
        #endregion PaymentConcession

        #region TranslationNames
        [InverseProperty(nameof(Translation.NoticeName))]
        public ICollection<Translation> TranslationNames { get; set; } = new List<Translation>();
        #endregion TranslationNames

        #region TranslationDescriptions
        [InverseProperty(nameof(Translation.NoticeDescription))]
        public ICollection<Translation> TranslationDescriptions { get; set; } = new List<Translation>();
        #endregion TranslationDescriptions

        #region TranslationShortDescriptions
        [InverseProperty(nameof(Translation.NoticeShortDescription))]
        public ICollection<Translation> TranslationShortDescriptions { get; set; } = new List<Translation>();
        #endregion TranslationShortDescriptions

        #region TranslationPlaces
        [InverseProperty(nameof(Translation.NoticePlace))]
        public ICollection<Translation> TranslationPlaces { get; set; } = new List<Translation>();
        #endregion TranslationPlaces

        #region Create
        private static Notice Create(
            int paymentConcessionId,
            string name, string shortDescription, string description,
            NoticeType type
        )
        {
            var now = DateTime.UtcNow;

            var notice = new Notice
            {
                PaymentConcessionId = paymentConcessionId,
                Type = type,
                PhotoUrl = ""
            }
            .SetState(NoticeState.Active)
            .SetName(name)
            .SetDescription(shortDescription, description)
            .SetVisibility(true, NoticeVisibility.Public)
            .SetPosition("", null, null)
            .SetVisibilityInterval(now, XpDateTime.MaxValue)
            .SetEvent(null)
            .SetSuperNotice(null)
            ;

            return notice;
        }
        #endregion Create

        #region CreateNotice
        public static Notice CreateNotice(
            int paymentConcessionId,
            string name, string shortDescription, string description
		)
        {
            return Create(paymentConcessionId, name, shortDescription, description, NoticeType.Notice);
        }
        #endregion CreateNotice

        #region CreatePage
        public static Notice CreatePage(
            int paymentConcessionId,
            string name, string shortDescription, string description
		)
        {
            return Create(paymentConcessionId, name, shortDescription, description, NoticeType.Page);
        }
        #endregion CreatePage

        #region CreateEdict
        public static Notice CreateEdict(
            int paymentConcessionId,
            string name, string shortDescription, string description
        )
        {
            return Create(paymentConcessionId, name, shortDescription, description, NoticeType.Edict);
        }
        #endregion CreateEdict

        #region SetState
        public Notice SetState(NoticeState state)
        {
            State = state;

            return this;
        }
        #endregion SetState

        #region SetName
        public Notice SetName(string name)
        {
            Name = name;

            return this;
        }
        #endregion SetName

        #region SetDescription
        public Notice SetDescription(string shortDescription, string description)
        {
            ShortDescription = shortDescription ?? "";
            Description = description ?? "";

            return this;
        }
        #endregion SetDescription

        #region SetVisibility
        public Notice SetVisibility(bool isVisible, NoticeVisibility visibility)
        {
            IsVisible = isVisible;
            Visibility = visibility;

            return this;
        }
        #endregion SetVisibility

        #region SetPosition
        public Notice SetPosition(string place, decimal? longitude, decimal? latitude)
        {
            Place = place ?? "";
            Longitude = longitude;
            Latitude = latitude;

            return this;
        }
        #endregion SetPosition

        #region SetVisibilityInterval
        public Notice SetVisibilityInterval(DateTime? start, DateTime? end)
        {
            Start = start ?? XpDateTime.MinValue;
            End = end ?? XpDateTime.MaxValue;

            return this;
        }
        #endregion AddVisibSetVisibilityIntervalilityInterval

        #region SetEvent
        public Notice SetEvent(int? eventId)
        {
            EventId = eventId == 0 ? null : eventId;

            return this;
        }
        #endregion SetEvent

        #region SetSuperNotice
        public Notice SetSuperNotice(int? superNoticeId)
        {
            SuperNoticeId = superNoticeId;

            return this;
        }
		#endregion SetSuperNotice

		
	}
}
