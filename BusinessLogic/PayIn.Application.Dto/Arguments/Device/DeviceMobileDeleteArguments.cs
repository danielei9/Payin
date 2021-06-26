using PayIn.Common;
using Xp.Common.Dto.Arguments;

namespace PayIn.Application.Dto.Arguments.Device
{
	public class DeviceMobileDeleteArguments : IArgumentsBase
    {
        public DeviceType Type { get; set;}

		#region Constructors
		public DeviceMobileDeleteArguments(DeviceType type)
		{
			Type = type;
		}
		#endregion Constructors
	}
}
