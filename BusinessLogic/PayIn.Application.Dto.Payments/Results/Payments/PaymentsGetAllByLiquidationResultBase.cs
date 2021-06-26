using Xp.Application.Dto;
using Xp.Common;

namespace PayIn.Application.Dto.Payments.Results
{
	public class PaymentsGetAllByLiquidationResultBase : ResultBase<PaymentsGetAllByLiquidationResult>
	{
		public XpDateTime LiquidationSince { get; set; }
		public XpDateTime LiquidationUntil { get; set; }
		public decimal LiquidationAmount { get; set; }
		public decimal LiquidationCommission { get; set; }
		public decimal TotalAmount { get; set; }
		public decimal TotalCommission { get; set; }
	}
}
