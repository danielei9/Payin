using PayIn.Common;

namespace PayIn.Application.Dto.Results.Main
{
	public partial class MainMobileGetAllV3Result
	{
		public int Id { get; set; }
		public int Day { get; set; }
		public int Month { get; set; }
		public decimal Amount { get; set; }
		public string Reference { get; set; }
		public string ConcessionName { get; set; }
		public TicketType Type { get; set; }
	}
}
