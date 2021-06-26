using PayIn.Common;

namespace PayIn.Application.Dto.Results.ServiceWorker
{
	public class ServiceWorkerGetAllResult
	{
		public int         Id    { get; set; }
		public string      Name  { get; set; }
		public string      Login { get; set; }
		public WorkerState State { get; set; }
	}
}
