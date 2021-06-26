using Xp.Common.Dto.Arguments;

namespace PayIn.Application.Dto.Payments.Arguments
{
	public class PaymentConcessionCampaignGetAllArguments : IArgumentsBase
	{		
		public string Filter { get; set; }
		public int Id { get; set; }

		#region Constructors
		public PaymentConcessionCampaignGetAllArguments(string filter, int id)
		{
			Filter = filter ?? "";
			Id = id;
		}
		public PaymentConcessionCampaignGetAllArguments(){}	
		#endregion Constructors
	}
}
