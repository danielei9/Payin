using PayIn.Common;
using Xp.Common;

namespace PayIn.Application.Dto.Payments.Results
{
	public partial class LiquidationPayResult
	{
		public int Id { get; set; }
		//public decimal TotalQuantity { get; set; }
		//public decimal PayinQuantity { get; set; }
		public decimal PaidQuantity { get; set; }
		public XpDate PaymentDate { get; set; }
		public XpDate LiquidationSince { get; set; }
		public XpDate LiquidationUntil { get; set; }
		public LiquidationState State { get; set; }
		public string Cif { get; set; }
		public string ConcessionName { get; set; }
		public string AccountNumber { get; set; }
		public int PaymentsCount { get; set; }
		public bool PaidBank { get; set; }
		public bool PaidTPV { get; set; }
	}
}
