using PayIn.Domain.Public;
using System;
using System.ComponentModel.DataAnnotations.Schema;
using Xp.Domain;

namespace PayIn.Domain.Payments
{
	public class PurseValue : Entity
	{
		public decimal Amount { get; set; }
		public int? Slot { get; set; }

		#region Purse
		public int PurseId { get; set; }
        [ForeignKey("PurseId")]
        public Purse Purse { get; set; }
		#endregion Purse

		#region ServiceOperation
		public int ServiceOperationId { get; set; }
		[ForeignKey("ServiceOperationId")]
		public ServiceOperation ServiceOperation { get; set; }
		#endregion ServiceOperation

		#region Contructors
		public PurseValue()
		{
		}
		public PurseValue(decimal amount, Purse purse, ServiceOperation serviceOperation, int? slot = null)
		{
			Amount = amount;
			Purse = purse;
			ServiceOperation = serviceOperation;
			Slot = slot;
		}
		#endregion Contructors
	}
}