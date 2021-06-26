using PayIn.Common.Resources;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Xp.Application.Arguments;

namespace PayIn.Application.Dto.Payments.Arguments
{
	public class MobileTicketPayV3Arguments : MobileConfigurationArguments
	{
		[Required]
		public int Id { get; set; }
		[RegularExpression(@"^\d{4}$", ErrorMessageResourceType = typeof(UserResources), ErrorMessageResourceName = "PinErrorMessage")]
		[Required(AllowEmptyStrings = false)]
		public string Pin { get; set; }
		public IEnumerable<MobileTicketPayV3Arguments_PaymentMedia>  PaymentMedias { get; set; }
		public IEnumerable<MobileTicketPayV3Arguments_Promotion> Promotions { get; set; }

		#region Constructors
		public MobileTicketPayV3Arguments(int id, IEnumerable<MobileTicketPayV3Arguments_PaymentMedia> paymentMedias, IEnumerable<MobileTicketPayV3Arguments_Promotion> promotions, string pin, string deviceManufacturer, string deviceModel, string deviceName, string deviceSerial, string deviceId, string deviceOperator, string deviceImei, string deviceMac, string operatorSim, string operatorMobile)
			: base(deviceManufacturer, deviceModel, deviceName, deviceSerial, deviceId, deviceOperator, deviceImei, deviceMac, operatorSim, operatorMobile)
		{
			Id = id;
			PaymentMedias = paymentMedias;
			Promotions = promotions;
			Pin = pin;
		}
		#endregion Constructors
	}
}
