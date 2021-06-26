using PayIn.Common;
using PayIn.Common.Resources;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Xp.Application.Arguments;
using Xp.Common;

namespace PayIn.Application.Dto.Payments.Arguments
{
	public class PublicTicketPayAndCreateWebCardByUserArguments : MobileConfigurationArguments
	{
		// Datos de la persona a la que se le crea la tarjeta
		[Required(AllowEmptyStrings=false)] public string     Login           { get; set; }
		[Required(AllowEmptyStrings=false)] public string     UserName        { get; set; }
		[Required(AllowEmptyStrings=false)] public string     UserTaxName     { get; set; }
		[Required(AllowEmptyStrings=false)] public string     UserTaxLastName { get; set; }
		                                    public XpDateTime UserBirthday    { get; set; }
		                                    public string     UserTaxNumber   { get; set; }
		                                    public string     UserTaxAddress  { get; set; }
		                                    public string     UserPhone       { get; set; }
		[Required(AllowEmptyStrings=false)] public string     UserEmail       { get; set; }

		// Información del ticket a crear y pagar con la nueva tarjeta
		[Required]                          public int        TicketId        { get; set; }
		[RegularExpression(@"^\d{4}$", ErrorMessageResourceType = typeof(UserResources), ErrorMessageResourceName = "PinErrorMessage")]
		[Required(AllowEmptyStrings=false)] public string     Pin             { get; set; }
		                                    public string     Name            { get; set; }
		                                    public string     BankEntity      { get; set; }

        #region Constructor
        public PublicTicketPayAndCreateWebCardByUserArguments(
			string userEmail, string userName, string userTaxName, string userTaxLastName, XpDateTime userBirthday, string userTaxNumber, string userTaxAddress, string userPhone, string login,
			int ticketId,
			string pin, string name, string bankEntity,
			string deviceManufacturer, string deviceModel, string deviceName, string deviceSerial, string deviceId, string deviceOperator, string deviceImei, string deviceMac, string operatorSim, string operatorMobile
		)
			: base(deviceManufacturer, deviceModel, deviceName, deviceSerial, deviceId, deviceOperator, deviceImei, deviceMac, operatorSim, operatorMobile)
		{
			UserEmail = userEmail;
			UserName = userName;
			UserTaxName = userTaxName;
			UserTaxLastName = userTaxLastName;
			UserBirthday = userBirthday;
			UserTaxNumber = userTaxNumber = "";
			UserTaxAddress = userTaxAddress = "";
			UserPhone = userPhone = "";
			Login = login;
			TicketId = ticketId;
			Pin = pin;
			Name = name ?? "";
			BankEntity = bankEntity ?? "";
		}
		#endregion Constructor
	}
}
