using Xp.Common;

namespace PayIn.Application.Dto.Payments.Results.Purse
{
	public class PurseGetAllResult
	{
		public int Id { get; set; }
		public string Name { get; set; }
		public XpDate Validity { get; set; }
		public XpDate Expiration { get; set; }
		public int NumberPaymentConcessions { set; get; }
		public int NumberActivePaymentConcessions { set; get; }
		public bool IsSupplier { set; get; }
		public string Supplier { set; get; }
		public decimal? Total { get; set; }
	}
}
