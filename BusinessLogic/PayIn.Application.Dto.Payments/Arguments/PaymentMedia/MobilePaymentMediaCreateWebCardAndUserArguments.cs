using Xp.Application.Arguments;

namespace PayIn.Application.Dto.Payments.Arguments
{
	public class MobilePaymentMediaCreateWebCardAndUserArguments : MobileConfigurationArguments
    {
        public string Name { get; set; }
        public string Pin { get; set; }

        #region Constructors
        public MobilePaymentMediaCreateWebCardAndUserArguments(string name, string pin, string deviceManufacturer, string deviceModel, string deviceName, string deviceSerial, string deviceId, string deviceOperator, string deviceImei, string deviceMac, string operatorSim, string operatorMobile)
            : base(deviceManufacturer, deviceModel, deviceName, deviceSerial, deviceId, deviceOperator, deviceImei, deviceMac, operatorSim, operatorMobile)
        {
            Name = name;
            Pin = pin;
        }
        #endregion Constructors
    }
}
