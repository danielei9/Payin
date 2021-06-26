using PayIn.Common.Resources;
using System.ComponentModel.DataAnnotations;
using Xp.Common.Dto.Arguments;

namespace PayIn.Application.Dto.Payments.Arguments.Payment
{
	public class PaymentTpvRefundArguments : IArgumentsBase
	{
		[RegularExpression(@"^\d{4}$", ErrorMessageResourceType = typeof(UserResources), ErrorMessageResourceName = "PinErrorMessage")]
		[Required(AllowEmptyStrings = false)]
		public string Pin { get; set; }
		public int    PaymentId  { get; set; }

		#region Constructors
		public PaymentTpvRefundArguments(int paymentId, string pin)
		{
			Pin = pin;
			PaymentId = paymentId;
		}
		#endregion Constructors
	}
}
