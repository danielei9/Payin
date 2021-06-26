using PayIn.Common;
using Xp.Common;

namespace PayIn.Application.Dto.Payments.Results
{
	public class TicketGetResult
	{
		public int Id { get; set; }
		public string SupplierName { get; set; }
		public decimal Amount { get; set; }
		public XpDateTime Date { get; set; }
		public XpDateTime Since { get; set; }
		public XpDateTime Until { get; set; }
		public string Reference { get; set; }
		public TicketStateType State { get; set; }
	}
}
