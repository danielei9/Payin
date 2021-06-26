using PayIn.Common;
using System.Collections.Generic;
using Xp.Common;

namespace PayIn.Application.Dto.Payments.Results
{
	public class TicketGetDetailsResult
	{
		public int Id { get; set; }
		public XpDateTime Date { get; set; }
		public XpDateTime Since { get; set; }
		public XpDateTime Until { get; set; }
		public TicketStateType State { get; set; }
		public string Reference { get; set; }
		public decimal Amount { get; set; }
		public decimal Total { get; set; }
		public string SupplierName { get; set; }
		public string SupplierTaxAddress { get; set; }
		public string SupplierTaxNumber { get; set; }
		public string SupplierLogin { get; set; }
		public string SupplierFotoUrl { get; set; }
		public bool HasShipment { get; set; }
		public decimal PayedAmount { get; set; }
		public string WorkerName { get; set; }
		public IEnumerable<TicketGetDetailsResult_Payment> Payments { get; set; }
		public IEnumerable<TicketGetDetailsResult_TicketLine> Lines { get; set; }

		#region Constructors
		public TicketGetDetailsResult()
		{
			Payments = new List<TicketGetDetailsResult_Payment>();
			Lines = new List<TicketGetDetailsResult_TicketLine>();
		}
		#endregion Constructors
	}
}
