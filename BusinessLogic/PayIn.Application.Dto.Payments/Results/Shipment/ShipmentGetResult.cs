using PayIn.Common;
using System;
using Xp.Common;

namespace PayIn.Application.Dto.Payments.Results.Shipment
{
	public partial class ShipmentGetResult
	{
		public int Id { get; set; }
		public XpDateTime Since { get; set; }
		public XpDateTime Until { get; set; }
		public decimal Amount { get; set; }
		public string Name { get; set; }
		public bool Started { get; set; }
		public bool Finished { get; set; }
		
	}
}
