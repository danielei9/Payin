using PayIn.Common.Security;
using System.ComponentModel.DataAnnotations;
using Xp.Common;
using PayIn.Common;

namespace PayIn.Application.Dto.Security.Arguments
{
    public class AccountUpdateArguments
    {
		[Display(Name = "resources.security.name")]                     public string   Name { get; set; }
		[Display(Name = "resources.security.mobile")]                   public string   Mobile { get; set; }
		[Display(Name = "resources.security.sex")]                      public SexType? Sex { get; set; }         
		[Display(Name = "resources.security.taxNumber")]                public string   TaxNumber { get; set; }
		[Display(Name = "resources.security.taxName")]                  public string   TaxName { get; set; }
		[Display(Name = "resources.security.taxAddress")]               public string   TaxAddress { get; set; }
		[Display(Name = "resources.security.birthday")]                 public XpDate   Birthday  { get; set; }
		[Display(ResourceType = typeof(SecurityResources), Name = "User Photo")]               public byte[]   UserPhoto { get; set; }
    }
}