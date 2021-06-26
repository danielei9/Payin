using System.Collections.Generic;

namespace PayIn.Application.Dto.Payments.Results
{
	public class MobileTicketCreateAndGetResult_TicketLine
	{
		public int Id { get; set; }
		public string Title { get; set; }
		public decimal Amount { get; set; }
		public decimal Quantity { get; set; }

		public IEnumerable<MobileTicketCreateAndGetResult_Entrance> Entrances { get; set; }
	}
}
