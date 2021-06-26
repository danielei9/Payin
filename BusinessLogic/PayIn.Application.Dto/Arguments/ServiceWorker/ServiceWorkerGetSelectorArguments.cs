using Xp.Common.Dto.Arguments;

namespace PayIn.Application.Dto.Arguments.ServiceWorker
{
	public partial class ServiceWorkerGetSelectorArguments : IArgumentsBase
	{
		public string Filter { get; private set; }

		#region Constructors
		public ServiceWorkerGetSelectorArguments(string filter)
		{
			Filter = filter;
		}
		#endregion Constructors
	}
}

