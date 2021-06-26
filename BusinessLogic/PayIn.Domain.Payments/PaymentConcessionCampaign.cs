using PayIn.Common;
using Xp.Domain;

namespace PayIn.Domain.Payments
{
	public class PaymentConcessionCampaign : Entity
	{
		public PaymentConcessionCampaignState State { get; set; }

		#region Concession
		public int PaymentConcessionId { get; set; }
		public PaymentConcession PaymentConcession { get; set; }
		#endregion Concession

		#region Campaign
		public int CampaignId { get; set; }
		public Campaign Campaign { get; set; }
		#endregion Campaign
	}
}
