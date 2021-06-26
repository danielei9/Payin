using System.ComponentModel.DataAnnotations;
using PayIn.Common.Security;
using System;
using Xp.Common;

namespace PayIn.Application.Dto.Security.Arguments
{
	public class AccountRegisterCompanyArguments
	{
		[Required]	[Display(Name = "resources.security.userName")]		[EmailAddress]		public string Email { get; set; }				
		[Required]	[Display(Name = "resources.security.supplierName")]                     public string SupplierName { get; set; }
		[Required]	[Display(Name = "resources.security.name")]                     		public string Name { get; set; }
		[Required]                                                                   		public Boolean AcceptTerms { get; set; }   
		                                                                                    public Boolean isBussiness { get; set; }
	}
}