using System.ComponentModel.DataAnnotations;
using Xp.Common.Dto.Arguments;

namespace PayIn.Application.Dto.JustMoney.Arguments
{
	public class JustMoneyApiPrepaidCardForgotPasswordArguments : IArgumentsBase
	{
		[Required(AllowEmptyStrings = false)]
		[Display(Name = "resources.security.email")]
		[EmailAddress]
		public string Email { get; set; }

		#region Constructors
		public JustMoneyApiPrepaidCardForgotPasswordArguments(string email)
		{
			Email = email ?? "";
		}
		#endregion Constructors
	}
}
