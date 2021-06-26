using PayIn.Common;

namespace PayIn.Application.Dto.Results.Main
{
    public class MainMobileGetEntailmentsResult
	{
		public int Id { get; set; }
		public WorkerState State { get; set; }
		public string ConcessionName { get; set; }
	}
}
