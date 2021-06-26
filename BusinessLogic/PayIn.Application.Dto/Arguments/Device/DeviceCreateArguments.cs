using PayIn.Common;
using Xp.Common.Dto.Arguments;

namespace PayIn.Application.Dto.Arguments.Device
{
    public class DeviceCreateArguments : IArgumentsBase
    {
        public string Token {get; set;}
        public DeviceType Type { get; set;}

		public DeviceCreateArguments(string token, DeviceType type)
		{
			Token = token;
			Type = type;
		}
    }
}
