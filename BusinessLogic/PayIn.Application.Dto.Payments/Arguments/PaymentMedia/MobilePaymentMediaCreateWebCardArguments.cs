using PayIn.Common.Resources;
using System.ComponentModel.DataAnnotations;
using Xp.Application.Arguments;

namespace PayIn.Application.Dto.Payments.Arguments
{
	public partial class MobilePaymentMediaCreateWebCardArguments : MobileConfigurationArguments
	{
		[RegularExpression(@"^\d{4}$", ErrorMessageResourceType = typeof(UserResources), ErrorMessageResourceName = "PinErrorMessage")]
		[Required(AllowEmptyStrings=false)] public string Pin        { get; set; }
		[Required(AllowEmptyStrings=false)] public string Name       { get; set; }
		[Required(AllowEmptyStrings=false)] public string BankEntity { get; set; }

		#region Constructors
		public MobilePaymentMediaCreateWebCardArguments(string name, string pin, string bankEntity, string deviceManufacturer, string deviceModel, string deviceName, string deviceSerial, string deviceId, string deviceOperator, string deviceImei, string deviceMac, string operatorSim, string operatorMobile)
			: base(deviceManufacturer, deviceModel, deviceName, deviceSerial, deviceId, deviceOperator, deviceImei, deviceMac, operatorSim, operatorMobile)
		{
			Name = name;
			Pin = pin;
			BankEntity = bankEntity;
		}
		#endregion Constructors
	}
}
