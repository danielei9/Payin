using PayIn.Infrastructure.JustMoney.Enums;
using System.ComponentModel.DataAnnotations;
using Xp.Common.Dto.Arguments;

namespace PayIn.Application.Dto.JustMoney.Arguments
{
	public class PrepaidCardRegisterTokenArguments : IArgumentsBase
	{
		[Required] [RegularExpression("[a-zA-Z0-9 ,'._-]{0,50}")] public string FirstName { get; set; }
		[Required] [RegularExpression("[-_0-9A-Za-z]{0,50}")]     public string LastName { get; set; }
		[Required] [RegularExpression("[-_0-9A-Za-z]{0,50}")]     public string Address1 { get; set; }
		[Required] [RegularExpression("[-_0-9A-Za-z]{0,50}")]     public string Address2 { get; set; }
		[Required] [RegularExpression("[-_0-9A-Za-z]{1,10}")]     public string ZipCode { get; set; }
		[Required] [RegularExpression("[-_0-9A-Za-z]{0,50}")]     public string City { get; set; }
		           [RegularExpression("[-_0-9A-Za-z]{0,40}")]     public string State { get; set; }
		[Required]                                                public JustMoneyCountryEnum Country { get; set; }

		#region Constructors
		public PrepaidCardRegisterTokenArguments() { }
		#endregion Constructors
	}
}
