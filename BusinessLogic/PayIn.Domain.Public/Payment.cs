using System;
using System.ComponentModel.DataAnnotations;
using Xp.Domain;

namespace PayIn.Domain.Public
{
	public class Payment : IEntity
	{
		                                   public int      Id     { get; set; }
		[Required]                         public decimal  Amount { get; set; }
		                                   public DateTime Date   { get; set; }
		[Required(AllowEmptyStrings=true)] public string   Name   { get; set; }

		#region PaymentMedia
		public int PaymentMediaId { get; set; }
		public PaymentMedia PaymentMedia { get; set; }
		#endregion PaymentMedia

		#region TaxName
		[Required(AllowEmptyStrings = false)]
		public string TaxName { get; set; }
		#endregion TaxName

		#region TaxAddress
		[Required(AllowEmptyStrings = false)]
		public string TaxAddress { get; set; }
		#endregion TaxAddress

		#region TaxNumber
		[Required(AllowEmptyStrings = false)]
		public string TaxNumber { get; set; }
		#endregion TaxNumber

		#region Ticket
		public int TicketId { get; set; }
		public Ticket Ticket { get; set; }
		#endregion Ticket

		#region Until
		public DateTime? Until { get; set; }
		#endregion Until
	}
}
