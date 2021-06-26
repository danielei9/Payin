using System.ComponentModel.DataAnnotations;
using Xp.Common.Dto.Arguments;

namespace Xp.Application.Arguments
{
  public abstract class MobileConfigurationArguments : IArgumentsBase
  {
		public string DeviceManufacturer { get; set; }
		public string DeviceModel        { get; set; }
		public string DeviceName         { get; set; } 
		public string DeviceSerial       { get; set; }
		public string DeviceId           { get; set; }
		public string DeviceOperator     { get; set; }
		public string DeviceImei         { get; set; }
		public string DeviceMac          { get; set; }
		public string OperatorSim        { get; set; }
		public string OperatorMobile     { get; set; }

		#region Constructors
		public MobileConfigurationArguments(string deviceManufacturer, string deviceModel, string deviceName, string deviceSerial, string deviceId, string deviceOperator, string deviceImei, string deviceMac, string operatorSim, string operatorMobile)
		{
			DeviceManufacturer = deviceManufacturer ?? "";
			DeviceModel = deviceModel ?? "";
			DeviceName = deviceName ?? "";
			DeviceSerial = deviceSerial ?? "";
			DeviceId = deviceId ?? "";
			DeviceOperator = deviceOperator ?? "";
			DeviceImei = deviceImei ?? "";
			DeviceMac = deviceMac ?? "";
			OperatorSim = operatorSim ?? "";
			OperatorMobile = operatorMobile ?? "";
		}
		#endregion Constructors
  }
}
