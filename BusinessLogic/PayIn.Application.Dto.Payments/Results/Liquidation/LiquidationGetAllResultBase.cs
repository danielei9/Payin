using Xp.Application.Dto;
using Xp.Common;

namespace PayIn.Application.Dto.Payments.Results
{
	public partial class LiquidationGetAllResultBase : ResultBase<LiquidationGetAllResult>
	{
		public decimal Amount { get; set; }
		public decimal Payin { get; set; }
		public decimal Total { get; set; }
		public XpDateTime LiquidationSince { get; set; }
		public XpDateTime LiquidationRequestDate { get; set; }
		public int PaymentsCount { get; set; }
		public decimal? AmountSum { get; set; }
		public decimal? PayinSum { get; set; }
		public decimal? TotalSum { get; set; }
		public bool DateExists { get; set; }
	}
}
