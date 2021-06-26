using System.ComponentModel.DataAnnotations;
using Xp.Application.Arguments;

namespace PayIn.Application.Dto.Payments.Arguments
{
    public class ApiTicketPayArguments : MobileConfigurationArguments
	{
		[Required] public int Id { get; set; }

        #region Constructor
        public ApiTicketPayArguments(
			int id,
			string deviceManufacturer, string deviceModel, string deviceName, string deviceSerial, string deviceId, string deviceOperator, string deviceImei, string deviceMac, string operatorSim, string operatorMobile
		)
			: base(deviceManufacturer, deviceModel, deviceName, deviceSerial, deviceId, deviceOperator, deviceImei, deviceMac, operatorSim, operatorMobile)
		{
            Id = id;
		}
		#endregion Constructor
	}
}
