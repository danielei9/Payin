using PayIn.Domain.Transport;
using PayIn.Domain.Transport.MifareClassic.Operations;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Xp.Common.Dto.Arguments;
using Xp.Domain.Transport;

namespace PayIn.Application.Dto.Transport.Arguments.TransportOperation
{
	public class TransportOperationRecharge2Arguments : IArgumentsBase
	{
		public int Code { get; set; }
        public decimal? Quantity { get { return quantity; } set { quantity = value == null ? (decimal?)null : Math.Round(value.Value, 2); } }
        private decimal? quantity;
        public int PriceId { get; set; }
		public int TicketId { get; set; }
		public RechargeType RechargeType { get; set; }
		public IEnumerable<TransportOperationRechargeArguments_Promotion> Promotions { get; set; }
		// Generic arguments
		public OperationInfo Operation { get; set; }
		public int? OperationId { get { return Operation.Id; } }
		public MifareOperationResultArguments[] Script { get; set; }
		public DateTime Now { get; set; }

		#region Constructors
		public TransportOperationRecharge2Arguments(
			int code,
			decimal? quantity,
			int? priceId,
			int ticketId,
			RechargeType rechargeType,
			IEnumerable<TransportOperationRechargeArguments_Promotion> promotions,
			// Generic arguments
			OperationInfo operation,
			MifareOperationResultArguments[] script
#if DEBUG || TEST || HOMO
			, DateTime? now = null
#endif
		)
		{
			Code = code;
			Quantity = quantity;
			PriceId = priceId ?? 0;
			TicketId = ticketId;
			RechargeType = rechargeType;
			Promotions = promotions ?? new List<TransportOperationRechargeArguments_Promotion>();
			// Generic arguments
			Operation = operation;
			Script = script;
#if DEBUG || TEST || HOMO
			Now = now ?? DateTime.Now;
#else
			Now = DateTime.Now;
#endif
		}
		#endregion Constructors
	}
}
