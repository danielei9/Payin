using Xp.Common.Dto.Arguments;

namespace PayIn.Application.Dto.Payments.Arguments
{
	public class PaymentConcessionCampaignGetArguments : IArgumentsBase
	{
		public int Id { get; set; }

		#region Constructors
		public PaymentConcessionCampaignGetArguments( int id)
		{
			Id = id;
		}
		
		#endregion Constructors
	}
}
