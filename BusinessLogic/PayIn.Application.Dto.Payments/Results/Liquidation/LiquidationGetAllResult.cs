using PayIn.Common;
using System;
using Xp.Common;

namespace PayIn.Application.Dto.Payments.Results
{
	public partial class LiquidationGetAllResult 
	{
		public int? Id { get; set; }
		public decimal Amount { get; set; }
		public decimal Paid { get; set; }
		public XpDateTime PaymentDate { get; set; }
		public XpDateTime Since { get; set; }
		public XpDateTime Until { get; set; }
		public LiquidationState State { get; set; }
        public int PaymentsCount { get; set; }
        public int LinesCount { get; set; }
		public int ConcessionId { get; set; }
		public String ConcessionName { get; set; }
	}
}
