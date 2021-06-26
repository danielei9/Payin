using Xp.Common.Dto.Arguments;

namespace PayIn.Application.Dto.Payments.Arguments
{
	public class ProductMobileGetTpvArguments : IArgumentsBase
	{
		public int PaymentConcessionId { get; set; }

		#region Constructors
		public ProductMobileGetTpvArguments(int paymentConcessionId)
		{
			PaymentConcessionId = paymentConcessionId;
		}
		#endregion Constructors
	}
}
