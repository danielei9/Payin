using PayIn.Common.Resources;
using System.ComponentModel.DataAnnotations;
using Xp.Common.Dto.Arguments;

namespace PayIn.Application.Dto.Internal.Arguments
{
	public class UserCheckPinArguments : IArgumentsBase
	{
		[RegularExpression(@"^\d{4}$", ErrorMessageResourceType = typeof(UserResources), ErrorMessageResourceName = "PinErrorMessage")]
		[Required(AllowEmptyStrings = false)] public string Pin { get; set; }
		
		#region Constructors
		public UserCheckPinArguments(string pin)
		{
			Pin = pin;
		}
		#endregion Constructors
	}
}