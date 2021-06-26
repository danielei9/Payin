using PayIn.Common;
using System;
using System.Collections.Generic;
using System.Text;
using Xp.Common.Dto.Arguments;

namespace PayIn.Application.Dto.Arguments.Device
{
    public class DeviceMobileCreateArguments : IArgumentsBase
    {
        public string Token {get; set;}
        public DeviceType Type { get; set;}

		public DeviceMobileCreateArguments(string token, DeviceType type)
		{
			Token = token;
			Type = type;
		}
    }
}
