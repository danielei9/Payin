using Microsoft.Practices.Unity;
using PayIn.Application.Dto.Payments.Arguments;
using PayIn.Application.Payments.Handlers.User;
using PayIn.BusinessLogic.Common;
using System;
using System.Linq;
using System.Threading.Tasks;
using Xp.Application.Attributes;
using Xp.Application.Dto;

namespace PayIn.Application.Payments.Handlers
{
	[XpLog("Ticket", "Pay")]
	[XpAnalytics("Ticket", "Pay")]
	public class PublicTicketWebPayUserHandler :
		IServiceBaseHandler<PublicTicketWebPayUserArguments>
	{
		[Dependency] public MobilePaymentMediaCreateWebCardAndUserHandler MobilePaymentMediaCreateWebCardAndUserHandler { get; set; }
		[Dependency] public ISessionData SessionData { get; set; }

		#region ExecuteAsync
		public async Task<dynamic> ExecuteAsync(PublicTicketWebPayUserArguments arguments)
		{
			var now = DateTime.Now.ToUTC();

			return await MobilePaymentMediaCreateWebCardAndUserHandler.ExecuteInternalAsync(
				//now,
				ticketId: arguments.Id,
				login: SessionData.Login,
				name: arguments.Name,
				userLogin: arguments.UserLogin,
				userName: arguments.UserName,
				userTaxNumber: arguments.UserTaxNumber,
				userTaxName: arguments.UserTaxName,
				userTaxAddress: arguments.UserTaxAddress,
				pin: arguments.Pin,
				deviceManufacturer: arguments.DeviceManufacturer,
				deviceModel: arguments.DeviceModel,
				deviceName: arguments.DeviceName,
				deviceSerial: arguments.DeviceSerial,
				deviceId: arguments.DeviceId,
				deviceOperator: arguments.DeviceOperator,
				deviceImei: arguments.DeviceImei,
				deviceMac: arguments.DeviceMac,
				operatorSim: arguments.OperatorSim,
				operatorMobile: arguments.OperatorMobile
			);
		}
		#endregion ExecuteAsync
	}
}