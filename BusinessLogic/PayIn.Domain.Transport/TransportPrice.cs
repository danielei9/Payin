using PayIn.Common;
using PayIn.Domain.Transport.Eige.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using Xp.Domain;


namespace PayIn.Domain.Transport
{
	public class TransportPrice : Entity
	{
		public DateTime Start { get; set; }
		public DateTime End { get; set; }
		public int Version { get; set; }
		[Precision(5, 2)]
		public decimal Price { get; set; }
		public EigeZonaEnum? Zone { get; set; }
		public TransportPriceState State { get; set; }
		public int? MaxTimeChanges { get; set; }
		public long? OperatorContext { get; set; }

		#region Title
		public int TransportTitleId { get; set; }
		public TransportTitle Title { get; set; }
		#endregion Title

		#region TransportOffer
		[InverseProperty("Price")]
		public ICollection<TransportOffer> Offer { get; set; }
		#endregion TransportOffer

		#region TransportOperation
		[InverseProperty("Price")]
		public ICollection<TransportOperation> Operations { get; set; }
		#endregion TransportOperation

		#region Constructors
		public TransportPrice()
		{
			Offer = new List<TransportOffer>();
			Operations = new List<TransportOperation>();
		}
		#endregion Constructors
	}
}
