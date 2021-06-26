//ASM 20150921
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using PayIn.Common.Security;

namespace PayIn.Web.Models
{
    public class ConfirmInvitedUserViewModel
	{
        [Required]
        [Display(Name = "UserId")]
        [DataType(DataType.Text)]
        public string UserId { get; set; }

		[Required]
        [Display(Name = "Code")]
        [DataType(DataType.Text)]
        public string Code { get; set; }

        [Display(Name = "Refresh")]
        public bool Refresh { get; set; }

		[Required]
		[Display(Name = "Name")]
		public string Name { get; set; }

		[Required]
		[Display(Name = "Mobile")]
		public string Mobile { get; set; }

		[Required]
		[DataType(DataType.Password)]
		[Display(Name = "Password")]
		[StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6)]
		public string Password { get; set; }

		[Required]
		[DataType(DataType.Password)]
		[Display(Name = "ConfirmPassword")]
		[Compare("Password", ErrorMessageResourceType = typeof(SecurityResources), ErrorMessageResourceName = "ComparePassword")]
		public string ConfirmPassword { get; set; }
	}
}