using Xp.Common.Dto.Arguments;

namespace PayIn.Application.Dto.Payments.Arguments
{
	public class PaymentConcessionCampaignResendNotificationArguments : IArgumentsBase
	{
		public int Id { get; set; }

		#region Constructors
		public PaymentConcessionCampaignResendNotificationArguments(int id)
		{
			Id = id;
		}
		#endregion Constructors

	}
}
