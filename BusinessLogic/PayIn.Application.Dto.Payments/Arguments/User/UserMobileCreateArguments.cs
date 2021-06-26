using PayIn.Common.Resources;
using System.ComponentModel.DataAnnotations;
using Xp.Common;
using Xp.Common.Dto.Arguments;

namespace PayIn.Application.Dto.Payments.Arguments
{
	public class UserMobileCreateArguments : IArgumentsBase
	{
		[RegularExpression(@"^\d{4}$", ErrorMessageResourceType = typeof(UserResources), ErrorMessageResourceName = "PinErrorMessage")]
		[Required(AllowEmptyStrings=false)] public string Pin        { get; set; }
		[Required(AllowEmptyStrings=true)] public string TaxNumber   { get; set; }
		[Required(AllowEmptyStrings=false)] public string TaxName    { get; set; }
		[Required(AllowEmptyStrings=true)] public string TaxAddress { get; set; }
											public XpDate Birthday { get; set; }
		#region Constructors
		public UserMobileCreateArguments(string pin, string taxNumber, string taxName, string taxAddress, XpDate birthday)
		{
			Pin = pin;
			TaxNumber = taxNumber = "";
			TaxName = taxName="";
			TaxAddress = taxAddress = "";
			Birthday = birthday;
		}
		#endregion Constructors
	}
}
