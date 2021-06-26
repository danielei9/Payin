using PayIn.Common;
using System.ComponentModel.DataAnnotations.Schema;
using Xp.Domain;

namespace PayIn.Domain.Payments
{
    public class Translation : Entity
    {
        public LanguageEnum Language { get; set; }
        public string Text { get; set; }

        #region NoticeName
        public int? NoticeNameId { get; set; }
        [ForeignKey(nameof(Translation.NoticeNameId))]
        public Notice NoticeName { get; set; }
        #endregion NoticeName

        #region NoticeDescription
        public int? NoticeDescriptionId { get; set; }
        [ForeignKey(nameof(Translation.NoticeDescriptionId))]
        public Notice NoticeDescription { get; set; }
        #endregion NoticeDescription

        #region NoticeShortDescription
        public int? NoticeShortDescriptionId { get; set; }
        [ForeignKey(nameof(Translation.NoticeShortDescriptionId))]
        public Notice NoticeShortDescription { get; set; }
        #endregion NoticeShortDescription

        #region NoticePlace
        public int? NoticePlaceId { get; set; }
        [ForeignKey(nameof(Translation.NoticePlaceId))]
        public Notice NoticePlace { get; set; }
        #endregion NoticePlace

        #region EventName
        public int? EventNameId { get; set; }
        [ForeignKey(nameof(Translation.EventNameId))]
        public Event EventName { get; set; }
        #endregion EventName

        #region EventDescription
        public int? EventDescriptionId { get; set; }
        [ForeignKey(nameof(Translation.EventDescriptionId))]
        public Event EventDescription { get; set; }
        #endregion EventDescription

        #region EventShortDescription
        public int? EventShortDescriptionId { get; set; }
        [ForeignKey(nameof(Translation.EventShortDescriptionId))]
        public Event EventShortDescription { get; set; }
        #endregion EventShortDescription

        #region EventPlace
        public int? EventPlaceId { get; set; }
        [ForeignKey(nameof(Translation.EventPlaceId))]
        public Event EventPlace { get; set; }
        #endregion EventPlace

        #region EventConditions
        public int? EventConditionsId { get; set; }
        [ForeignKey(nameof(Translation.EventShortDescriptionId))]
        public Event EventConditions { get; set; }
        #endregion EventConditions
    }
}
