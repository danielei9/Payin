using PayIn.Common;
using Xp.Domain;

namespace PayIn.Domain.Payments
{
	public class PaymentConcessionPurse : Entity
	{
		public PaymentConcessionPurseState State { get; set; }

		#region PaymentConcession
		public int PaymentConcessionId { get; set; }
		public PaymentConcession PaymentConcession { get; set; }
		#endregion PaymentConcession

		#region Purse
		public int PurseId { get; set; }
		public Purse Purse { get; set; }
		#endregion Purse
	}
}
