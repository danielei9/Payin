using System.ComponentModel.DataAnnotations;
using PayIn.Common.Security;
using System;

namespace PayIn.Application.Dto.Security.Arguments
{
	public class AccountChangePasswordArguments
	{
		[Required] [Display(Name = "resources.security.oldPassword")]     [DataType(DataType.Password)] [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6)] public string OldPassword     { get; set; }
		[Required] [Display(Name = "resources.security.password")]        [DataType(DataType.Password)] [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6)] public string Password        { get; set; }
		[Required] [Display(Name = "resources.security.confirmPassword")] [DataType(DataType.Password)] [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]           public string ConfirmPassword { get; set; }
	}
}
