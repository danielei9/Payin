using System;
using System.Collections.Generic;
using System.Text;

namespace PayIn.Application.Dto.Results.TicketDetail
{
    public class TicketDetailGetResult
    {
		public int Id { get; set; }
		public string Reference { get; set; }
		public string Article { get; set; }
		public decimal Price { get; set; }
		public int Quantity { get; set; }
		public decimal? Vat { get; set; }
		public decimal Total { get; set; }
		public int TicketId { get; set; }
    }
}
