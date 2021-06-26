using PayIn.Common;
using System;
using System.Collections.Generic;
using Xp.Common;

namespace PayIn.Application.Dto.Payments.Results.Shipment
{
	public class ShipmentGetAllResult
	{
		public int      Id     { set; get; }
		public string   Name   { set; get; }
		public decimal  Amount { set; get; }
		public XpDateTime Since  { set; get; }
		public XpDateTime Until  { get; set; }		
		public Boolean Started { get; set; }
		public Boolean Finished { get; set; }
		public int NumberTickets { get; set; }
		public int NumberPayers { get; set; }
	}
}
