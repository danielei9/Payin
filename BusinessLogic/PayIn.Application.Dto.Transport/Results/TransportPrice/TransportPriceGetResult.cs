using PayIn.Domain.Transport;
using PayIn.Domain.Transport.Eige.Enums;
using System;

namespace PayIn.Application.Dto.Transport.Results.TransportPrice
{
	public class TransportPriceGetResult
	{
		public int Id { get; set; }		
		public DateTime Start { get; set; }		
		public DateTime End { get; set; }		
		public int Version { get; set; }
		public decimal Price { get; set; }
		public EigeZonaEnum? Zone { get; set; }
		public string ZoneAlias { get; set; }
		public int? MaxTimeChanges { get; set; }
		public long? OperatorContext { get; set; }
	}
}
