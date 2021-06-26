using PayIn.Common;
using Xp.Common;

namespace PayIn.Application.Dto.Payments.Results
{
	public class MobileTicketGetAllResult
	{
		public int             Id              { get; set; }
		public decimal         Amount          { get; set; }
		public XpDateTime      Date            { get; set; }
		public TicketStateType State           { get; set; }
		public TicketType      Type            { get; set; }
		public string          SupplierName    { get; set; }
		public string          WorkerName      { get; set; }
		public decimal         PayedAmount     { get; set; }
	}
}
