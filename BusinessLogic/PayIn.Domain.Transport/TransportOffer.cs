using System;
using System.ComponentModel.DataAnnotations;
using Xp.Domain;

namespace PayIn.Domain.Transport
{
	public class TransportOffer : Entity
	{
		public int Quantity { get; set; }
		public DateTime Start { get; set; }
		public DateTime End { get; set; }
		[Required(AllowEmptyStrings = true)]
		public string ImageUrl { get; set; }

		#region TransportPrice
		public int TransportPriceId { get; set; }
		public TransportPrice Price { get; set; }
		#endregion TransportPrice
	}
}
