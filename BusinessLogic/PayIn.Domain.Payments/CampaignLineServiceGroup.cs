using PayIn.Domain.Public;
using System.ComponentModel.DataAnnotations.Schema;
using Xp.Domain;

namespace PayIn.Domain.Payments
{
	public class CampaignLineServiceGroup : Entity
	{
        #region CampaignLine
        public int CampaignLineId { get; set; }
        [ForeignKey("CampaignLineId")]
        public CampaignLine CampaignLine { get; set; }
        #endregion CampaignLine

        #region ServiceGroup
        public int ServiceGroupId { get; set; }
        [ForeignKey("ServiceGroupId")]
        public ServiceGroup ServiceGroup { get; set; } // Unidireccional
        #endregion ServiceGroup
    }
}