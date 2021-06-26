using Xp.Common.Dto.Arguments;

namespace PayIn.Application.Dto.Arguments.ServiceWorker
{
	public partial class ServiceWorkerGetAllArguments : IArgumentsBase
	{
		public string Filter { get; set; }

		#region Constructors
		public ServiceWorkerGetAllArguments(string filter)
		{
			Filter = filter ?? "";
		}
		#endregion Constructors
	}
}