using PayIn.Domain.Transport;
using PayIn.Domain.Transport.Eige.Enums;
using System;
using System.ComponentModel.DataAnnotations;
using Xp.Common.Dto.Arguments;
using Xp.Domain;

namespace PayIn.Application.Dto.Transport.Arguments.TransportPrice
{
	public class TransportPriceCreateArguments : IArgumentsBase
	{
		public int TransportTitleId { get; set; }

		[Display(Name = "resources.transportPrice.start")]
		[Required]
		public DateTime Start { get; set; }

		[Display(Name = "resources.transportPrice.end")]
		[Required]
		public DateTime End { get; set; }

		[Display(Name = "resources.transportPrice.version")]
		[Required]
		public int Version { get; set; }

		[Display(Name = "resources.transportPrice.price")]
		[Required]
		[Precision(5, 2)]
		public decimal Price { get; set; }
		
		[Display(Name = "resources.transportPrice.zone")]
		public EigeZonaEnum? Zone { get; set; }

		[Display(Name = "resources.transportPrice.maxTimeChanges")]
		public int? MaxTimeChanges { get; set; }
		[Display(Name = "resources.transportPrice.operatorContext")]
		public long? OperatorContext { get; set; }
		#region Constructors
		public TransportPriceCreateArguments(int transportTitleId, DateTime start, DateTime end, int version,decimal price, EigeZonaEnum? zone, int? maxTimeChanges, long? operatorContext)
		{
			TransportTitleId = transportTitleId;
			Start = start;
			End = end;
			Version = version;
			Price = price;			
			Zone = zone;		
			MaxTimeChanges = maxTimeChanges;
			OperatorContext = operatorContext;
		}
		#endregion Constructors
	}
}
