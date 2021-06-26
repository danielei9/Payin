using PayIn.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xp.Domain;

namespace PayIn.Domain.Payments
{
	public class PaymentUser : IEntity
	{
		                                                       public int Id { get; set; }
		[Required(AllowEmptyStrings = false)]                  public string Name { get; set; }
		[Required(AllowEmptyStrings = false)] [MaxLength(200)] public string Login { get; set; }
		                                                       public PaymentUserState State { get; set; }

		#region Concession
		public int ConcessionId { get; set; }
		public PaymentConcession Concession { get; set; }
		#endregion Concession

		#region Tickets
		[InverseProperty("PaymentUser")]
		public ICollection<Ticket> Tickets { get; set; }
		#endregion Tickets

		#region Constructors
		public PaymentUser()
		{
			Tickets = new List<Ticket>();
		}
		#endregion Constructors
	}
}

