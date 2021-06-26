using PayIn.Common.Resources;
using System.ComponentModel.DataAnnotations;
using Xp.Common.Dto.Arguments;

namespace PayIn.Application.Dto.Payments.Arguments.Payment
{
	public class PaymentRefundPinArguments : IArgumentsBase
	{
												public int Id { get; set; }

		[RegularExpression(@"^\d{4}$", ErrorMessageResourceType = typeof(UserResources), ErrorMessageResourceName = "PinErrorMessage")]
		[Required(AllowEmptyStrings = false)]	public string Pin { get; set; }


		
		#region Constructors
		public PaymentRefundPinArguments(int id, string pin)
		{
			Id = id;
			Pin = pin;
		}
		#endregion Constructors
	}
}