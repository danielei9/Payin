using System.ComponentModel.DataAnnotations;
using PayIn.Common.Security;
using System;

namespace PayIn.Application.Dto.Security.Arguments
{
	public class AccountForgotPasswordArguments
	{
		[Required]	[Display(Name = "resources.security.email")] [EmailAddress] public string Email { get; set; }
	}
}