using System;
using System.ComponentModel.DataAnnotations;
using System.Xml.Serialization;
using Xp.Common;
using Xp.Common.Dto.Arguments;

namespace PayIn.Application.Dto.Payments.Arguments
{
    public class PublicCampaignCodeCreateArguments : IArgumentsBase
	{
		/// <summary>
		/// Id de la campaña
		/// </summary>
        [Required]
        public int CampaignId { get; set; }
        /// <summary>
        /// Login del usuario
        /// </summary>
        [Required(AllowEmptyStrings = false)]
        public string Login { get; set; }
        [XmlIgnore]
        [Required]
        public XpDateTime Now { get; set; }

        #region Constructors
        public PublicCampaignCodeCreateArguments(
            int campaignId,
            string login
#if DEBUG
            , XpDateTime now
#endif //DEBUG
        )
        {
            CampaignId = campaignId;
            Login = login;
#if DEBUG
            Now = now ?? DateTime.UtcNow;
#else //DEBUG
			Now = DateTime.UtcNow;
#endif //DEBUG
        }
        #endregion Constructors
    }
}
