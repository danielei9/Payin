using Xp.Common.Dto.Arguments;

namespace PayIn.Application.Dto.Arguments.ServiceWorker
{
	public class ServiceWorkerDeleteArguments : IArgumentsBase
	{
		public int Id { get; set; }

		#region Constructor
		public ServiceWorkerDeleteArguments(int id)
		{
			Id = id;
		}
		#endregion Constructor
	}
}
