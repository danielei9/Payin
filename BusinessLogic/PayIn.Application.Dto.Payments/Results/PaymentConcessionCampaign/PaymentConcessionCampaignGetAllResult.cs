using PayIn.Common;

namespace PayIn.Application.Dto.Payments.Results
{
	public class PaymentConcessionCampaignGetAllResult
	{
		public int Id { set; get; }
		public string PaymentConcessionName { get; set; }		
		public PaymentConcessionCampaignState State { set; get; }
		public bool IsOwner { get; set; }
	}
}
