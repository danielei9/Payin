using PayIn.Common.Resources;
using System.ComponentModel.DataAnnotations;
using Xp.Application.Arguments;

namespace PayIn.Application.Dto.Payments.Arguments
{
	public class PaymentRefundArguments : MobileConfigurationArguments
	{
		[RegularExpression(@"^\d{4}$", ErrorMessageResourceType = typeof(UserResources), ErrorMessageResourceName = "PinErrorMessage")]
		[Required(AllowEmptyStrings = false)]
		public string Pin { get; set; }
		public int    Id  { get; set; }

		#region Constructors
		public PaymentRefundArguments(int id, string pin, string deviceManufacturer, string deviceModel, string deviceName, string deviceSerial, string deviceId, string deviceOperator, string deviceImei, string deviceMac, string operatorSim, string operatorMobile)
			: base(deviceManufacturer, deviceModel, deviceName, deviceSerial, deviceId, deviceOperator, deviceImei, deviceMac, operatorSim, operatorMobile)
		{
			Id = id;
			Pin = pin;
		}
		#endregion Constructors
	}
}
