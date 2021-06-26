using PayIn.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using Xp.Domain;

namespace PayIn.Domain.Payments
{
	public class Shipment : Entity
	{
		public string Name { get; set; }
		public decimal Amount { get; set; }
		public DateTime Since { get; set; }
		public DateTime Until { get; set; }
		public ShipmentState State { get; set; }

		#region Tickets
		[InverseProperty("Shipment")]
		public ICollection<Ticket> Tickets { get; set; }
		#endregion Tickets

		#region Concession
		public int ConcessionId { get; set; }
		public PaymentConcession Concession { get; set; }
		#endregion Concession

		#region Constructors
		public Shipment()
		{
			Tickets = new List<Ticket>();
		}
		#endregion Constructors

	}
}



