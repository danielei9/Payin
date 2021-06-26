using PayIn.Common;

namespace PayIn.Application.Dto.Payments.Results
{
	public class PaymentWorkerMobileGetAllResult
	{
        public WorkerState State { get; set; }

        public int Id { get; set; }
		public string ConcessionName { get; set; }
	}
}
