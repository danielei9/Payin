using Xp.Common.Dto.Arguments;

namespace PayIn.Application.Dto.SmartCity.Arguments
{
	public class ApiComponentGetAllArguments : IArgumentsBase
	{
		public int DeviceId { get; set; }

		#region Constructors
		public ApiComponentGetAllArguments(int deviceId)
		{
			DeviceId = deviceId;
		}
		#endregion Constructors
	}
}
