using PayIn.Common;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Xp.Domain;

namespace PayIn.Domain.Payments
{
	public class PaymentWorker : IEntity
	{
	   	                                                       public int         Id     { get; set; }
		[Required(AllowEmptyStrings = false)]                  public string      Name   { get; set; }
		[Required(AllowEmptyStrings = false)] [MaxLength(200)] public string      Login  { get; set; }
		                                                       public WorkerState State  { get; set; }

		#region Tickets 
		[InverseProperty("PaymentWorker")]
		public ICollection<Ticket> Tickets { get; set; } 
		#endregion Tickets

		#region Concession
		public int ConcessionId { get; set; }
		public PaymentConcession Concession { get; set; }
		#endregion Concession

		#region Constructors
		public PaymentWorker()
		{
			Tickets = new List<Ticket>();
		}
		#endregion Constructors
	}
}
