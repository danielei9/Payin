using Xp.Common.Dto.Arguments;

namespace PayIn.Application.Dto.Arguments.ServiceWorker
{
	public class WorkerMobileGetAllArguments : IArgumentsBase 
	{
		public string Filter { get; set; }

		public WorkerMobileGetAllArguments(string filter)
		{
			Filter = filter ?? "";
		}
	}
}
