using System.Collections.Generic;
using Xp.Common;

namespace PayIn.Application.Dto.Payments.Results.Shipment
{
	public class ShipmentReceiptGetAllResult
	{
		public int Id { set; get; }
		public bool Paid { set; get; }
		public bool Finished { set; get; }
		public decimal Amount { set; get; }
		public string Reference { get; set; }
		public XpDateTime Since { set; get; }
		public XpDateTime Until { get; set; }
		public IEnumerable<ShipmentReceiptGetAllResult_TicketLine> Lines { get; set; }

		#region Constructors
		public ShipmentReceiptGetAllResult()
		{			
			Lines = new List<ShipmentReceiptGetAllResult_TicketLine>();
		}
		#endregion Constructors
	}
}
