using PayIn.Domain.Public;
using System.ComponentModel.DataAnnotations.Schema;
using Xp.Domain;

namespace PayIn.Domain.Payments
{
	public class CampaignLineServiceUser : Entity
    {
        #region CampaignLine
        public int CampaignLineId { get; set; }
        [ForeignKey("CampaignLineId")]
        public CampaignLine CampaignLine { get; set; }
        #endregion CampaignLine

        #region ServiceUser
        public int ServiceUserId { get; set; }
        [ForeignKey("ServiceUserId")]
        public ServiceUser ServiceUser { get; set; } // Unidireccional
        #endregion ServiceUser
    }
}