using Xp.Common;

namespace PayIn.Application.Dto.Payments.Results
{
	public partial class EntranceTicketGetResult
	{
		public int Id { get; set; }
		public long Code { get; set; }
		public decimal Amount { get; set; }
		public XpDateTime Date { get; set; }
		public XpDateTime Since { get; set; }
		public XpDateTime Until { get; set; }
		public string SupplierName { get; set; }
		public string TaxName { get; set; }
		public string TaxAddress { get; set; }
		public string TaxNumber { get; set; }
	}
}
