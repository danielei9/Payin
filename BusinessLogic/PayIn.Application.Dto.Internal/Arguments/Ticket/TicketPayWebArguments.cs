using PayIn.Common;
using System.ComponentModel.DataAnnotations;
using Xp.Application.Arguments;

namespace PayIn.Application.Dto.Internal.Arguments
{
	public partial class TicketPayWebArguments : MobileConfigurationArguments
	{
		                                      public string                 CommerceCode           { get; set; }
		[Required(AllowEmptyStrings=false)]   public string                 Login                  { get; set; }
		                                      public int                    OrderId                { get; set; }
        [Required]                            public PaymentMediaCreateType PaymentMediaCreateType { get; set; }
        [Required]                            public decimal                Amount                 { get; set; }

		                                      public int                    PublicTicketId         { get; set; }
		                                      public int                    PublicPaymentId        { get; set; }

		#region PaymentMediaCreateWebCardArguments
		public TicketPayWebArguments(
			string commerceCode,
			int publicTicketId, int publicPaymentId,
            string login, int orderId, PaymentMediaCreateType paymentMediaCreateType, decimal amount,
			string deviceManufacturer, string deviceModel, string deviceName, string deviceSerial, string deviceId, string deviceOperator, string deviceImei, string deviceMac, string operatorSim, string operatorMobile)
			: base(deviceManufacturer, deviceModel, deviceName, deviceSerial, deviceId, deviceOperator, deviceImei, deviceMac, operatorSim, operatorMobile)
        {
			CommerceCode = commerceCode;

			PublicTicketId = publicTicketId;
            PublicPaymentId = publicPaymentId;

            Login = login;
            OrderId = orderId;
            PaymentMediaCreateType = paymentMediaCreateType;
            Amount = amount;
		}
		#endregion PaymentMediaCreateWebCardArguments
	}
}
