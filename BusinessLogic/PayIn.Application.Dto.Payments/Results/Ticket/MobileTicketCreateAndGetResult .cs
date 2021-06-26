using PayIn.Common;
using System.Collections.Generic;
using Xp.Common;

namespace PayIn.Application.Dto.Payments.Results
{
	public class MobileTicketCreateAndGetResult
	{
		public int             Id              { get; set; }
		public string          Reference       { get; set; }
		public string          Title           { get; set; }
		public decimal         Amount          { get; set; }
		public decimal         PayedAmount     { get; set; }
		public XpDateTime      Date            { get; set; }
		public TicketStateType State           { get; set; }
		public bool            CanReturn       { get; set; }
		public string          SupplierName    { get; set; }
		public string          SupplierTaxName { get; set; }
		public string          SupplierAddress { get; set; }
		public string          SupplierNumber  { get; set; }
		public string          SupplierPhone   { get; set; }
		public string          WorkerName      { get; set; }
		public TicketType      Type            { get; set; }

		public IEnumerable<MobileTicketCreateAndGetResult_Payment> Payments   { get; set; }
		public IEnumerable<MobileTicketCreateAndGetResult_TicketLine> Lines { get; set; }
		public IEnumerable<MobileTicketCreateAndGetResult_Promotion> Promotions { get; set; }
	}
}
