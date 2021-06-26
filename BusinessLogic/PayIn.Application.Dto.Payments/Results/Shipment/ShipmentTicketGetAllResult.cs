using System.Collections.Generic;
using Xp.Common;

namespace PayIn.Application.Dto.Payments.Results.Shipment
{
	public  class ShipmentTicketGetAllResult
	{
		public XpDateTime Date { get; set; }
		public string Login { get; set; } 
		public int Id { get; set; }
		public decimal Amount { get; set; }
		public string Reference { get; set; }
		public bool Payed { get; set; }
		public bool Started { get; set; }
		public IEnumerable<ShipmentTicketGetAllResult_TicketLine> Lines { get; set; }
		public string Name { get; set; }

		#region Constructors
		public ShipmentTicketGetAllResult()
		{
			Lines = new List<ShipmentTicketGetAllResult_TicketLine>();
		}
		#endregion Constructors
	}
}
