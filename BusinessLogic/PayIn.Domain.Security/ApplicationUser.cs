using System;
using Microsoft.AspNet.Identity.EntityFramework;
using System.ComponentModel.DataAnnotations;
using PayIn.Common;
using Xp.Common;

namespace PayIn.Domain.Security
{
	public class ApplicationUser : IdentityUser
	{              
		[Required(AllowEmptyStrings = false)] public string    Name         { get; set; }
		[Required(AllowEmptyStrings = true)]  public string    Mobile       { get; set; }
		[Required(AllowEmptyStrings = true)]  public string    PhotoUrl     { get; set; }
		                                      public SexType   Sex          { get; set; }
		[Required(AllowEmptyStrings = true)]  public string    TaxNumber    { get; set; }
		[Required(AllowEmptyStrings = true)]  public string    TaxName      { get; set; }
		[Required(AllowEmptyStrings = true)]  public string    TaxAddress   { get; set; }
		                                      public DateTime? Birthday     { get; set; }
											  public UserType  isBussiness  { get; set; }
	}
}
