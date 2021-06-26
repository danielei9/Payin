using System.ComponentModel.DataAnnotations;
using PayIn.Common.Security;
using System;
using Xp.Common;

namespace PayIn.Application.Dto.Security.Arguments
{
	public class AccountConfirmCompanyEMailAndDataArguments
	{
		[Display(Name = "resources.security.userId")]								public string userId { get; set; }
		[Display(Name = "resources.security.code")]									public string code { get; set; }
		[Display(Name = "resources.security.refresh")]								public bool refresh { get; set; }

		[Display(Name = "resources.security.name")]									public string name { get; set; }
		//[Display(Name = "resources.security.userName")] [EmailAddress] public string userName { get; set; }
		[Display(Name = "resources.security.userName")]								public string userName { get; set; }
		[Display(Name = "resources.security.password")]								public string password { get; set; }
		//[Display(Name = "resources.security.passwordMin")] [DataType(DataType.Password)] [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6)] public string password { get; set; }
		//[Display(Name = "resources.security.confirmPassword")]		[DataType(DataType.Password)][Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]		public string confirmPassword { get; set; }
		//[Display(Name = "resources.security.supplierName")]							public string supplierName { get; set; }
		[Display(Name = "resources.security.mobile")]								public string mobile { get; set; }
		[Display(Name = "resources.security.taxNumber")]							public string taxNumber { get; set; }
		[Display(Name = "resources.security.taxName")]								public string taxName { get; set; }
		[Display(Name = "resources.security.taxAddress")]							public string taxAddress { get; set; }
		//[Display(Name = "resources.security.birthday")]								public DateTime? birthday { get; set; }
		                                               								//public Boolean acceptTerms { get; set; }
		[Display(Name = "resources.security.bankAccountNumber")]					public string bankAccountNumber { get; set; }
		//[Display(Name = "resources.security.phone")]								public string phone { get; set; }
		[Display(Name = "resources.security.address")]								public string address { get; set; }
		[Display(Name = "resources.security.observations")]							public string observations { get; set; }
		//[Display(Name = "resources.security.userPhone")] 							public string userPhone { get; set; }
		//[Display(Name = "resources.security.userAddress")]							public string userAddress { get; set; }
		//         		[Display(Name = "resources.security.userObservations")]		public string userObservations { get; set; }
		//[Display(Name = "resources.security.pin")]									public string pin { get; set; }
		//[Display(Name = "resources.security.pinConfirmation")]	[Compare("Pin", ErrorMessage = "The Pin and confirmation pin do not match.")] 	public string pinConfirmation { get; set; }


		//[Display(Name = "resources.security.checkTerms")]							public bool checkTerms { get; set; }

	}
}