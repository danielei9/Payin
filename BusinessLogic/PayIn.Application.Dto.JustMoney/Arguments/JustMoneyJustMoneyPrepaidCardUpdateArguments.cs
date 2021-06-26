using PayIn.Infrastructure.JustMoney.Enums;
using System.ComponentModel.DataAnnotations;
using Xp.Common;
using Xp.Common.Dto.Arguments;

namespace PayIn.Application.Dto.JustMoney.Arguments
{
	public class JustMoneyJustMoneyPrepaidCardUpdateArguments : IArgumentsBase
	{
		[Required(AllowEmptyStrings = false)] public string FirstName { get; set; }
		[Required(AllowEmptyStrings = false)] public string LastName { get; set; }
		[Required]                            public XpDate BirthDay { get; set; }
		[Required(AllowEmptyStrings = false)] public string Address1 { get; set; }
		[Required(AllowEmptyStrings = true)]  public string Address2 { get; set; }
		[Required(AllowEmptyStrings = false)] public string ZipCode { get; set; }
		[Required(AllowEmptyStrings = false)] public string City { get; set; }
		[Required(AllowEmptyStrings = false)] public string Province { get; set; }
		[Required]                            public JustMoneyCountryEnum Country { get; set; }
		[Required(AllowEmptyStrings = true)]  public string Phone { get; set; }
		[Required(AllowEmptyStrings = false)] public string Mobile { get; set; }

		#region Constructors
		public JustMoneyJustMoneyPrepaidCardUpdateArguments(
			string firstName, string lastName, XpDate birthDay,
			string address1, string address2, string zipCode, string city, string province, JustMoneyCountryEnum country,
			string phone,
			string mobile
		)
		{
			FirstName = firstName ?? "";
			LastName = lastName ?? "";
			BirthDay = birthDay;
			Address1 = address1 ?? "";
			Address2 = address2 ?? "";
			ZipCode = zipCode ?? "";
			City = city ?? "";
			Province = province ?? "";
			Country = country;
			Phone = phone ?? "";
			Mobile = mobile ?? "";
		}
		#endregion Constructors
	}
}
