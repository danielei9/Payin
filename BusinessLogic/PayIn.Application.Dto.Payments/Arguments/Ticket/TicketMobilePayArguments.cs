using PayIn.Common.Resources;
using System.ComponentModel.DataAnnotations;
using Xp.Application.Arguments;

namespace PayIn.Application.Dto.Payments.Arguments
{
	public class TicketMobilePayArguments : MobileConfigurationArguments
	{
		[RegularExpression(@"^\d{4}$", ErrorMessageResourceType = typeof(UserResources), ErrorMessageResourceName = "PinErrorMessage")]
		[Required(AllowEmptyStrings=false)] public string Pin            { get; set; }
		[Required]                          public int    Id             { get; set; }
		[Required]                          public int    PaymentMediaId { get; set; }

		#region Constructors
		public TicketMobilePayArguments(int id, int paymentMediaId, string pin, string deviceManufacturer, string deviceModel, string deviceName, string deviceSerial, string deviceId, string deviceOperator, string deviceImei, string deviceMac, string operatorSim, string operatorMobile)
			: base(deviceManufacturer, deviceModel, deviceName, deviceSerial, deviceId, deviceOperator, deviceImei, deviceMac, operatorSim, operatorMobile)
		{
			Id = id;
			PaymentMediaId = paymentMediaId;
			Pin = pin;
		}
		#endregion Constructors
	}
}
