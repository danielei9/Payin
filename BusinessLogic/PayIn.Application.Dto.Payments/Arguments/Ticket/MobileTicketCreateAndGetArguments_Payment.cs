using System.ComponentModel.DataAnnotations;
using Xp.Common;

namespace PayIn.Application.Dto.Payments.Arguments
{
	public class MobileTicketCreateAndGetArguments_Payment
	{
		                                     public decimal?   Amount        { get; set; }
		[Required(AllowEmptyStrings = true)] public string     ExternalLogin { get; set; } = "";
		                                     public long?      Uid           { get; set; }
		[Required]                           public XpDateTime Date          { get; set; }
	}
}
