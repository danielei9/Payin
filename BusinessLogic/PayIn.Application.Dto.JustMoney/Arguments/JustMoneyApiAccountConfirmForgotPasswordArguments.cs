using System.ComponentModel.DataAnnotations;

namespace PayIn.Application.Dto.JustMoney.Security.Arguments
{
	public class JustMoneyApiAccountConfirmForgotPasswordArguments
	{
		[Required] public string Code   { get; set; }
		[Required] public string UserId { get; set; }
		[Required] [Display(Name = "resources.security.password")]        [DataType(DataType.Password)] [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6)] public string Password        { get; set; }
		[Required] [Display(Name = "resources.security.confirmPassword")] [DataType(DataType.Password)] [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]           public string ConfirmPassword { get; set; }

		#region Constructors
		public JustMoneyApiAccountConfirmForgotPasswordArguments(string code, string userId, string password, string confirmPassword)
		{
			Code = code;
			UserId = userId;
			Password = password;
			ConfirmPassword = confirmPassword;
		}
		#endregion Constructors
	}
}