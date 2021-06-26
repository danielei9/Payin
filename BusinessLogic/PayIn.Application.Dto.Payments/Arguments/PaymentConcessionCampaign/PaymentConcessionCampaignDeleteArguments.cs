using Xp.Common.Dto.Arguments;

namespace PayIn.Application.Dto.Payments.Arguments
{
	public class PaymentConcessionCampaignDeleteArguments : IArgumentsBase
	{
		public int Id { get; set; }

		#region Constructor
		public PaymentConcessionCampaignDeleteArguments(int id)
		{
			Id = id;
		}
		#endregion Constructor
	}
}
