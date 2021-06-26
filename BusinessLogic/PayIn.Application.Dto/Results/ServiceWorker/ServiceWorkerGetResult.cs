using PayIn.Common;

namespace PayIn.Application.Dto.Results.ServiceWorker
{
	public partial class ServiceWorkerGetResult
	{
		public int         Id         { get; set; }
		public string      Name       { get; set; }
		public string      Login      { get; set; }
		public int         SupplierId { get; set; }
		public WorkerState State      { get; set; }
	}
}
