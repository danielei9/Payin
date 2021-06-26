using PayIn.Common;
using Xp.Common;

namespace PayIn.Application.Dto.Transport.Results.TransportPrice
{
	public class TransportPriceGetAllResult
	{
		public int Id { get; set; }		
		public XpDateTime Start { get; set; }		
		public XpDateTime End { get; set; }		
		public int Version { get; set; }
		public decimal Price { get; set; }
		public string ZoneAlias { get; set; }
		public TransportPriceState State { get; set; }
	}
}
