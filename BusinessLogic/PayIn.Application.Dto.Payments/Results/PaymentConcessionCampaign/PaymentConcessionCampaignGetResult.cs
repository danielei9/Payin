using PayIn.Common;

namespace PayIn.Application.Dto.Payments.Results
{
	public class PaymentConcessionCampaignGetResult
	{
		public int Id { set; get; }
		public string PaymentConcessionName { get; set; }		
		public PaymentConcessionCampaignState State { set; get; }	
	}
}
