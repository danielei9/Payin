using PayIn.Common;
using Xp.Common;

namespace PayIn.Application.Dto.Payments.Results
{
	public class MobileTicketGetOrdersResult
	{
		public int             Id              { get; set; }
		public string          UserName        { get; set; }
		public decimal         Amount          { get; set; }
		public XpDateTime      Date            { get; set; }
		public TicketStateType State           { get; set; }
		public string          SupplierName    { get; set; }
		public string          SupplierAddress { get; set; }
		public string          SupplierPhone   { get; set; }
		public string          WorkerName      { get; set; }
	}
}
