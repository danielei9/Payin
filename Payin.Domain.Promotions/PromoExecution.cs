using PayIn.Common;
using PayIn.Domain.Payments;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using Xp.Domain;

namespace PayIn.Domain.Promotions
{
	public class PromoExecution : Entity
	{	
		public DateTime? AppliedDate { get; set; }
		public long? Uid { get; set; }
		public long? Imei { get; set; }
		public string Code { get; set; }
		public PromotionExecutionType Type { get; set; }
		public decimal Cost { get; set; }
		public PromotionCodeState State { get; set; } //predeterminad a 1
		public string SerialNumber { get; set; }
		public int Number { get; set; }

		#region Promotion
		public int PromotionId { get; set; }
		public Promotion Promotion { get; set; }
		#endregion Promotion

		#region TransportOperation
		public int? TransportOperationId { get; set; }
		#endregion TranspotOperation

		#region TicketLine
		public int? TicketLineId { get; set; }
		public TicketLine TicketLine { get; set; }
		#endregion TicketLine

		#region PromoUser
		public int? PromoUserId { get; set; }
		public PromoUser PromoUser { get; set; }
		#endregion PromoUser
	}
}
