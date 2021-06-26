using Xp.Common.Dto.Arguments;

namespace PayIn.Application.Dto.Arguments.ServiceWorker
{
	public partial class ServiceWorkerGetControlArguments : IArgumentsBase
	{
		public string Filter { get; set; }

		#region Constructors
		public ServiceWorkerGetControlArguments(string filter)
		{
			Filter = filter ?? "";
		}
		#endregion Constructors
	}
}