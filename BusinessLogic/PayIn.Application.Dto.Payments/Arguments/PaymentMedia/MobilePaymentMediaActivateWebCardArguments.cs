using PayIn.Common.Resources;
using System.ComponentModel.DataAnnotations;
using Xp.Application.Arguments;

namespace PayIn.Application.Dto.Payments.Arguments
{
	public class MobilePaymentMediaActivateWebCardArguments : MobileConfigurationArguments
	{
		[RegularExpression(@"^\d{4}$", ErrorMessageResourceType = typeof(UserResources), ErrorMessageResourceName = "PinErrorMessage")]
		
        [Required(AllowEmptyStrings=false)] public string Pin        { get; set; }

        public int Id { get; set; }
        
        #region Constructors
        public MobilePaymentMediaActivateWebCardArguments( string pin, int id, string deviceManufacturer, string deviceModel, string deviceName, string deviceSerial, string deviceId, string deviceOperator, string deviceImei, string deviceMac, string operatorSim, string operatorMobile)
			: base(deviceManufacturer, deviceModel, deviceName, deviceSerial, deviceId, deviceOperator, deviceImei, deviceMac, operatorSim, operatorMobile)
		{
			Pin = pin;
            Id = id;
		}
		#endregion Constructors
	}
}
