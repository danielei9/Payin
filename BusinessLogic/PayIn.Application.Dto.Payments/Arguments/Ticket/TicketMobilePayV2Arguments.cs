using PayIn.Common.Resources;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Xp.Application.Arguments;

namespace PayIn.Application.Dto.Payments.Arguments
{
	public class TicketMobilePayV2Arguments : MobileConfigurationArguments
	{
		[RegularExpression(@"^\d{4}$", ErrorMessageResourceType = typeof(UserResources), ErrorMessageResourceName = "PinErrorMessage")]
		[Required(AllowEmptyStrings=false)] public string Pin            { get; set; }
		[Required]                          public int    Id             { get; set; }
                                            public IEnumerable<MobileTicketPayV2Arguments_PaymentMedia>  PaymentMedias { get; set; }

		#region Constructors
		public TicketMobilePayV2Arguments(int id, IEnumerable<MobileTicketPayV2Arguments_PaymentMedia> paymentMedias, string pin, string deviceManufacturer, string deviceModel, string deviceName, string deviceSerial, string deviceId, string deviceOperator, string deviceImei, string deviceMac, string operatorSim, string operatorMobile)
			: base(deviceManufacturer, deviceModel, deviceName, deviceSerial, deviceId, deviceOperator, deviceImei, deviceMac, operatorSim, operatorMobile)
		{
			Id = id;
			PaymentMedias = paymentMedias;
			Pin = pin;
		}
		#endregion Constructors
	}
}
