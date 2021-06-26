using PayIn.Common;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Xp.Domain;

namespace PayIn.Domain.Payments
{
	public class CampaignCode : Entity
	{
		public long Code { get; set; }
		public CampaignCodeState State { get; set; }
		[Required(AllowEmptyStrings = false)]
		public string Login { get; set; }

		#region Campaign
		public int CampaignId { get; set; }
		[ForeignKey("CampaignId")]
		public Campaign Campaign { get; set; }
		#endregion Campaign
	}
}
