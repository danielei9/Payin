using PayIn.Application.Dto.Payments.Arguments;
using PayIn.BusinessLogic.Common;
using PayIn.Infrastructure.Security;
using System;
using System.Threading.Tasks;
using Xp.Application.Attributes;
using Xp.Application.Dto;

namespace PayIn.Application.Payments.Handlers.User
{
	[XpLog("User", "Create")]
    [XpAnalytics("User", "Create")]
    public class MobilePaymentMediaCreateWebCardAndUserHandler :
        IServiceBaseHandler<MobilePaymentMediaCreateWebCardAndUserArguments>
    {
        private readonly ISessionData SessionData;
        private readonly MobilePaymentMediaCreateWebCardHandler PaymentMediaHandler;
        private readonly UserMobileCreateHandler UserHandler;

        #region Constructors
        public MobilePaymentMediaCreateWebCardAndUserHandler(
            ISessionData sessionData,
            MobilePaymentMediaCreateWebCardHandler paymentMediaHandler,
            UserMobileCreateHandler userHandler
        )
        {
            if (sessionData == null) throw new ArgumentNullException("sessionData");
            if (paymentMediaHandler == null) throw new ArgumentNullException("paymentMediaHandler");
            if (userHandler == null) throw new ArgumentNullException("paymentUserHandler");
            SessionData = sessionData;
            PaymentMediaHandler = paymentMediaHandler;
            UserHandler = userHandler;
        }
        #endregion Constructors

        #region ExecuteAsync
        public async Task<dynamic> ExecuteAsync(MobilePaymentMediaCreateWebCardAndUserArguments arguments)
        {
            await UserHandler.ExecuteAsync(new UserMobileCreateArguments(
                pin: arguments.Pin,
                taxNumber: null,
                taxName: SessionData.Name,
                taxAddress : null,
                birthday : null
            ));

            var securityRepository = new SecurityRepository();
            await securityRepository.UpdateTaxDataAsync(
                SessionData.Login,
                "",
                SessionData.Name,
                "",
                null
            );

            var result = await PaymentMediaHandler.ExecuteAsync(new MobilePaymentMediaCreateWebCardArguments(
                name: arguments.Name,
                pin: arguments.Pin,
                bankEntity: arguments.Name,
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
            ));
            return result;
        }
        #endregion ExecuteAsync
    }
}
