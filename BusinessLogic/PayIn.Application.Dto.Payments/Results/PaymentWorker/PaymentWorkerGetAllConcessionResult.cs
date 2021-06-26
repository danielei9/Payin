using PayIn.Common;

namespace PayIn.Application.Dto.Payments.Results
{
	public class PaymentWorkerGetAllConcessionResult
	{
		public int Id { get; set; }
		public int ConcessionId { get; set; }
		public string SupplierName { get; set; }
		public string ConcessionName { get; set; }		
		public WorkerState State { get; set; }
	}
}
