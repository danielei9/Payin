using PayIn.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Xp.Domain;

namespace PayIn.Domain.Payments
{
	public class TicketTemplate : Entity
	{
		[Required(AllowEmptyStrings = false)]		public string                  Name                 { get; set; }
		[Required(AllowEmptyStrings = false)]		public string                  RegEx                { get; set; }
		[Required(AllowEmptyStrings = true)]		public string                  PreviousTextPosition { get; set; }
		[Required(AllowEmptyStrings = true)]		public string                  BackTextPosition     { get; set; }
		[Required(AllowEmptyStrings = false)]		public string	               DateFormat           { get; set; }
		[Required(AllowEmptyStrings = false)]		public DecimalCharDelimiter    DecimalCharDelimiter { get; set; }						
													public int?	                   ReferencePosition    { get; set; }
													public int?                    TitlePosition        { get; set; }
													public int?                    DatePosition         { get; set; }
													public int                     AmountPosition       { get; set; }		
												    public int?                    WorkerPosition       { get; set; }  
													public bool                    IsGeneric            { get; set; }
		
		#region Tickets
		[InverseProperty("Template")]
		public ICollection<Ticket> Tickets { get; set; }
		#endregion Tickets

		#region Concessions
		[InverseProperty("TicketTemplate")]
		public ICollection<PaymentConcession> Concessions { get; set; }
		#endregion Concessions

		#region Constructors
		public TicketTemplate()
		{
			Tickets = new List<Ticket>();
			Concessions = new List<PaymentConcession>();
        }
		#endregion Constructors
	}
}