using Xp.Common.Dto.Arguments;

namespace PayIn.Application.Dto.Arguments.ServiceWorker
{
	public partial class ServiceWorkerGetArguments : IArgumentsBase
	{
		public int Id { get; set; }

		#region Constructors
		public ServiceWorkerGetArguments(int id)
		{
			Id = id;
		}
		public ServiceWorkerGetArguments()
		{
		}
		#endregion Constructors
	}
}
