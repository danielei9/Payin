using System.ComponentModel.DataAnnotations;
using PayIn.Common.Security;
using System;

namespace PayIn.Application.Dto.Security.Arguments
{
	public class AccountConfirmArguments
	{
		 [Display(Name = "resources.security.userId")]		public string userId { get; set; }  
		 [Display(Name = "resources.security.code")]		public string code { get; set; }
	}
}
