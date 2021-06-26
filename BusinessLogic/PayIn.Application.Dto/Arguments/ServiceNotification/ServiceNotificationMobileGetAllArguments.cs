using Xp.Common.Dto.Arguments;

namespace PayIn.Application.Dto.Arguments.ServiceNotification
{
	public class ServiceNotificationMobileGetAllArguments : IArgumentsBase
	{
		public int Skip { get; set; }
		public int Top { get; set; }
	}
}
