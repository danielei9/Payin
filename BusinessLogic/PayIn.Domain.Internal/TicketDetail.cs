using PayIn.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Xp.Domain;

namespace PayIn.Domain.Internal
{
	public class TicketDetail : IEntity
	{
		                                      public int      Id        { get; set; }
		[Required(AllowEmptyStrings = false)] public string   Reference { get; set; }
		[Required(AllowEmptyStrings = false)] public string   Article   { get; set; }
		                                      public decimal  Price     { get; set; }
		                                      public int      Quantity  { get; set; }
		                                      public decimal? VAT       { get; set; }
		                                      public decimal  Total     { get; set; }

		#region Ticket
		public int TicketId { get; set; }
		public Ticket Ticket { get; set; }
		#endregion Ticket
	}
}
