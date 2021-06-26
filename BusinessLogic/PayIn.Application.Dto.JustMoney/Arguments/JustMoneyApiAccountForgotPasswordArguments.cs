using System.ComponentModel.DataAnnotations;

namespace PayIn.Application.Dto.JustMoney.Security.Arguments
{
	public class JustMoneyApiAccountForgotPasswordArguments
	{
		[Required]	[Display(Name = "resources.security.email")] [EmailAddress] public string Email { get; set; }
	}
}