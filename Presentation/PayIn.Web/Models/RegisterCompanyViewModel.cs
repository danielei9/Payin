using PayIn.Common;
using PayIn.Common.Security;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace PayIn.Web.Models
{
	public class RegisterCompanyViewModel
	{
		[Required]
		[Display(Name = "Email")]
		[EmailAddress]
		public string Email { get; set; }	

		[Required]
		[Display(Name = "SupplierName")]
		public string SupplierName { get; set; }


		[Required]
		[Display(Name = "CheckTerms")]
		public bool CheckTerms { get; set; }

		[Required]
		[DataType(DataType.Password)]
		[Display(Name = "Password")]
		public string Password { get; set; }

		[Required]
		[DataType(DataType.Password)]
		[Display(Name = "ConfirmPassword")]
		[Compare("Password", ErrorMessageResourceType = typeof(SecurityResources), ErrorMessageResourceName = "ComparePassword")]
		public string ConfirmPassword { get; set; }

		[Required]
		[Display(Name = "UserType")]
		public UserType isBussiness { get; set; }

	}
}