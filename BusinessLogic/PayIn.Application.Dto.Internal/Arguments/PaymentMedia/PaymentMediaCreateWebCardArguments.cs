using PayIn.Common;
using PayIn.Common.Resources;
using System.ComponentModel.DataAnnotations;
using Xp.Application.Arguments;

namespace PayIn.Application.Dto.Internal.Arguments
{
	public partial class PaymentMediaCreateWebCardArguments : MobileConfigurationArguments
	{
		                                      public string  CommerceCode         { get; set; }
		[Required(AllowEmptyStrings=false)]   public string  Login                { get; set; }
		[Required]                            public PaymentMediaCreateType PaymentMediaCreateType { get; set; }
        [Required]                            public decimal Amount               { get; set; }
		[RegularExpression(@"^\d{4}$", ErrorMessageResourceType = typeof(UserResources), ErrorMessageResourceName = "PinErrorMessage")]
		[Required(AllowEmptyStrings=false)]   public string  Pin                  { get; set; }
		                                      public string  Name                 { get; set; }
		                                      public int     OrderId              { get; set; }
		                                      public int     PublicPaymentMediaId { get; set; }
		                                      public int     PublicTicketId       { get; set; }
		                                      public int     PublicPaymentId      { get; set; }
		                                      public string  BankEntity           { get; set; }

		#region PaymentMediaCreateWebCardArguments
		public PaymentMediaCreateWebCardArguments(string commerceCode, string pin, string name, int orderId, int publicPaymentMediaId, int publicTicketId, int publicPaymentId, string bankEntity, 
			string login, PaymentMediaCreateType paymentMediaCreateType, decimal amount,
			string deviceManufacturer, string deviceModel, string deviceName, string deviceSerial, string deviceId, string deviceOperator, string deviceImei, string deviceMac, string operatorSim, string operatorMobile)
			: base(deviceManufacturer, deviceModel, deviceName, deviceSerial, deviceId, deviceOperator, deviceImei, deviceMac, operatorSim, operatorMobile)
        {
			CommerceCode = commerceCode;
			Pin = pin;
            Name = name ?? "";
            OrderId = orderId;
            PublicPaymentMediaId = publicPaymentMediaId;
            PublicTicketId = publicTicketId;
            PublicPaymentId = publicPaymentId;
            BankEntity = bankEntity ?? "";

            Login = login;
			PaymentMediaCreateType = paymentMediaCreateType;
            Amount = amount;
		}
		#endregion PaymentMediaCreateWebCardArguments
	}
}
