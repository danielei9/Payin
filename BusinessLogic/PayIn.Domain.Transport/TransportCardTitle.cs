using PayIn.Domain.Transport.Eige.Enums;
using System;
using Xp.Domain;

namespace PayIn.Domain.Transport
{
	public class TransportCardTitle : Entity
	{
		public int Quantity { get; set; }
		public EigeTipoUnidadesSaldoEnum? QuantityType { get; set; }
		public SlotEnum SlotEnum { get; set; }
		public DateTime? SubscriptionExpiryDate { get; set; }
		public int IncreasedNumber { get; set; }

		#region TransportTitle
		public int TransportTitleId { get; set; }
		public TransportTitle TransportTitle { get; set; }
		#endregion TransportTitle

		#region TransportCard
		public int TransportCardId { get; set; }
		public TransportCard TransportCard { get; set; }
		#endregion TransportCard
	}
}
