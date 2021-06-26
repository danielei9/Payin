using System.ComponentModel.DataAnnotations;
using PayIn.Common;
using PayIn.Common.Security;
using System;
using Xp.Common;

namespace PayIn.Application.Dto.Security.Arguments
{
	public class AccountRegisterArguments
	{
		[Required] [Display(Name = "resources.security.userName")]        [EmailAddress]                public string UserName { get; set; }
		[Required] [Display(Name = "resources.security.passwordMin")]     [DataType(DataType.Password)] [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6)] public string Password { get; set; }
		[Required] [Display(Name = "resources.security.confirmPassword")] [DataType(DataType.Password)] [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")] public string ConfirmPassword { get; set; }
		[Required] [Display(Name = "resources.security.name")]                                          public string Name { get; set; }
		[Required] [Display(Name = "resources.security.mobile")]                                        public string Mobile { get; set; }
		           [Display(Name = "resources.security.taxNumber")]                                     public string TaxNumber { get; set; }
		           [Display(Name = "resources.security.taxName")]                                       public string TaxName { get; set; }
		           [Display(Name = "resources.security.taxAddress")]                                    public string TaxAddress { get; set; }
		           [Display(Name = "resources.security.birthday")]                                      public DateTime? Birthday { get; set; }
		[Required]												    									public Boolean AcceptTerms { get; set; }   
                                                                                                        public string PhotoUrl { get; set; }
																										public UserType isBussiness { get; set; }
	}
}