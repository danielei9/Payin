using System.ComponentModel.DataAnnotations;
using Xp.Common.Dto.Arguments;

namespace PayIn.Application.Dto.Payments.Arguments
{
	public class PaymentConcessionCampaignCreateArguments : IArgumentsBase
	{
		[Display(Name = "resources.paymentConcessionCampaign.login")]        public string Login { get; set; }
		                                                                     public int Id { get; set; }

		#region Constructors
		public PaymentConcessionCampaignCreateArguments(string login, int id)
		{
			Login = login;
			Id = id;
		}

		#endregion Constructors
	}
}
