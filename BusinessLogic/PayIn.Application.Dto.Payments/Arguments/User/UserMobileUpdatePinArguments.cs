using PayIn.Common.Resources;
using System.ComponentModel.DataAnnotations;
using Xp.Common.Dto.Arguments;

namespace PayIn.Application.Dto.Payments.Arguments
{
	public class UserMobileUpdatePinArguments : IArgumentsBase
	{
		[RegularExpression(@"^\d{4}$", ErrorMessageResourceType = typeof(UserResources), ErrorMessageResourceName = "PinErrorMessage")]
		[Required(AllowEmptyStrings=false)] public string OldPin     { get; set; }
		[RegularExpression(@"^\d{4}$", ErrorMessageResourceType = typeof(UserResources), ErrorMessageResourceName = "PinErrorMessage")]
		[Required(AllowEmptyStrings=false)] public string Pin        { get; set; }
		[Compare("Pin", ErrorMessageResourceType = typeof(UserResources), ErrorMessageResourceName = "ConfirmPin")]
		[Required(AllowEmptyStrings=false)] public string ConfirmPin { get; set; }

		#region Constructors
		public UserMobileUpdatePinArguments(string oldPin, string pin, string confirmPin)
		{
			OldPin = oldPin;
			Pin = pin;
			ConfirmPin = confirmPin;
		}
		#endregion Constructors
	}
}
