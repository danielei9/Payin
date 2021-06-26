using PayIn.Common;

namespace PayIn.Application.Dto.Payments.Results
{
	public class PaymentWorkerGetAllResult
	{
		public int Id { set; get; }
		public string Name { set; get; }
		public string Login { set; get; }
		public WorkerState State { set; get; }
		public bool HasAccepted { set; get; }
	}
}
