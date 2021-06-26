using System.ComponentModel.DataAnnotations;

namespace PayIn.Application.Dto.Payments.Arguments
{
	public class PublicTicketCreateEntrancesAndGetArguments_TicketLine
	{
		public decimal Quantity { get; set; }
        [Required]
		public int? EntranceTypeId { get; set; }
	}
}
