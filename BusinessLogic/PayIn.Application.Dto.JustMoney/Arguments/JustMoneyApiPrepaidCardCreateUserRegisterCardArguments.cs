using PayIn.Infrastructure.JustMoney.Enums;
using System.ComponentModel.DataAnnotations;
using Xp.Common;
using Xp.Common.Dto.Arguments;

namespace PayIn.Application.Dto.JustMoney.Arguments
{
	public class JustMoneyApiPrepaidCardCreateUserRegisterCardArguments : IArgumentsBase
	{
		[Required(AllowEmptyStrings = false)]	public string FirstName { get; set; }
		[Required(AllowEmptyStrings = false)]	public string LastName { get; set; }
		[Required(AllowEmptyStrings = false)]	public string Alias1 { get; set; }
												//public string Alias2 { get; set; }
												//public string Alias3 { get; set; }
		[Required(AllowEmptyStrings = false)]	public XpDate BirthDay { get; set; }
		[Required(AllowEmptyStrings = true)]	public string Mobile { get; set; }
		[Required(AllowEmptyStrings = true)]	public string Phone { get; set; }
		[Required(AllowEmptyStrings = false)]	public string Email { get; set; }
		[Required(AllowEmptyStrings = false)]	public string ConfirmEmail { get; set; }
		[Required(AllowEmptyStrings = false)]	public string Password { get; set; }
		[Required(AllowEmptyStrings = false)]	public string Address1 { get; set; }
		[Required(AllowEmptyStrings = true)]	public string Address2 { get; set; }
		[Required(AllowEmptyStrings = false)]	public string City { get; set; }
		[Required(AllowEmptyStrings = false)]	public string ZipCode { get; set; }
		[Required(AllowEmptyStrings = false)]	public string Province { get; set; }
		[Required]								public JustMoneyCountryEnum Country { get; set; }
		[Required(AllowEmptyStrings = false)]	public string CardHolderId { get; set; }

		#region Constructors
		public JustMoneyApiPrepaidCardCreateUserRegisterCardArguments(
			string firstName, string lastName,
			string alias1, string alias2, string alias3,
			XpDate birthDay,
			string mobile, string phone,
			string email, string confirmEmail,
			string password,
			string address1, string address2, string city, string zipCode, string province, JustMoneyCountryEnum country,
			string cardHolderId
		)
		{
			FirstName = firstName ?? "";
			LastName = lastName ?? "";
			Alias1 = alias1 ?? "";
			//Alias2 = alias2 ?? "";
			//Alias3 = alias3 ?? "";
			BirthDay = birthDay;
			Mobile = mobile ?? "";
			Phone = phone ?? "";
			Email = email ?? "";
			ConfirmEmail = confirmEmail ?? "";
			Password = password;
			Address1 = address1 ?? "";
			Address2 = address2 ?? "";
			City = city ?? "";
			ZipCode = zipCode ?? "";
			Province = province ?? "";
			Country = country;
			CardHolderId = cardHolderId ?? "";
		}
		#endregion Constructors
	}
}
