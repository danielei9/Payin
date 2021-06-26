using PayIn.Common;
using System.ComponentModel.DataAnnotations;
using Xp.Application.Dto;

namespace PayIn.Application.Dto.Arguments.PaymentMedia
{
	public partial class PaymentMediaCreateArguments : ArgumentsBase
	{
		                                             public int              Id              { get; set; }
		[Required]	[MinLength(1)]                   public string           Name            { get; set; }
		                                             public PaymentMediaType Type            { get; set; }
		[Required]	[RegularExpression(@"^\d{16}$")] public string           Number          { get; set; }
		            [Range(1, 12)]                   public int              ExpirationMonth { get; set; }
		            [Range(2012, 3000)]              public int              ExpirationYear  { get; set; }
		            [RegularExpression(@"^\d{3}$")]  public string           Cvv             { get; set; } // Card Verification Value
	}
}
