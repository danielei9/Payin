using System;
using System.ComponentModel.DataAnnotations;
using System.Xml.Serialization;
using Xp.Common;
using Xp.Common.Dto.Arguments;

namespace PayIn.Application.Dto.Payments.Arguments
{
    public class PublicCampaignGetQrArguments : IArgumentsBase
	{
		/// <summary>
		/// Id de la campaña
		/// </summary>
        [Required]
		public int Id { get; set; }
        /// <summary>
        /// Id del evento sobre el que se quiere consultar la campaña
        /// </summary>
        public int? EventId { get; set; }
        /// <summary>
        /// Login del usuario para el que se quiere generar el QR
        /// </summary>
        [Required(AllowEmptyStrings = false)]
        public string Login { get; set; }
        [Required]
        [XmlIgnore]
        public XpDateTime Now { get; set; }

        #region Constructors
        public PublicCampaignGetQrArguments(
            int id,
            int? eventId,
            string login
#if DEBUG
            , XpDateTime now
#endif //DEBUG
        )
        {
            Id = id;
            EventId = eventId;
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
