using PayIn.Application.Dto.Internal.Arguments;
using PayIn.Application.Dto.Internal.Results;
using PayIn.Application.Dto.Payments.Results;
using PayIn.BusinessLogic.Common;
using PayIn.Domain.Payments.Infrastructure;
using System;
using System.Threading.Tasks;
using Xp.Application.Dto;
using Xp.Infrastructure.Http;
using System.Linq;
using Microsoft.WindowsAzure.ServiceRuntime; // No quitar, se usa en Production
using PayIn.Common;
using Xp.Infrastructure;
using Microsoft.Practices.Unity;
using System.Collections.Generic;

namespace PayIn.Infrastructure.Payments.Services
{
	public class InternalService : BaseService, IInternalService
    {
#if DEBUG || RELEASE
        public override string BaseAddress { get { return "http://localhost:4861/"; } }
#else
		public override string BaseAddress { get {
            return "http://" + 
                RoleEnvironment
                .Roles["Payin.Web.Internal"]
                .Instances[
                    RoleEnvironment
                    .Roles["Payin.Web"]
                    .Instances
                    .IndexOf(
                        RoleEnvironment.CurrentRoleInstance
                    )
                ]
                .InstanceEndpoints["Http"]
                .IPEndpoint
                .ToString()
                + "/";
        } }
#endif

        [Dependency] public override ISessionData SessionData { get; set; }
        [Dependency] public ILogService LogService { get; set; }

        #region VersionAsync
        public async Task<MainGetVersionResult> VersionAsync()
        {
            var arguments = new UserHasPaymentArguments();
            var result = await Server.GetAsync<MainGetVersionResult>("internal/main/v1/version");
            return result;
        }
        #endregion VersionAsync

        #region UserCreateAsync
        public async Task UserCreateAsync(string pin)
        {
            var arguments = new UserCreateArguments
            (
                pin: pin
            );

            try
            {
                var result = await Server.PostAsync("Internal/User", arguments: arguments);

                LogService.TrackEvent("InternalService.UserCreateAsync", new Dictionary<string, string> {
                    { "arguments", arguments.ToJson() },
                    { "result", result.ToJson() }
                });
            }
            catch (Exception e)
            {
                LogService.TrackException(e, new Dictionary<string, string> {
                    { "name", "InternalService.UserCreateAsync" },
                    { "arguments", arguments.ToJson() }
                });
                throw;
            }
        }
        #endregion UserCreateAsync

        #region UserHasPaymentAsync
        public async Task<bool> UserHasPaymentAsync()
        {
            var arguments = new UserHasPaymentArguments();
            try
            {
                var result = await Server.GetAsync("Internal/User/HasPayment", arguments: arguments);

                LogService.TrackEvent("InternalService.InternalService", new Dictionary<string, string> {
                    { "arguments", arguments.ToJson() },
                    { "result", result.ToJson() }
                });

                return Convert.ToBoolean(result["hasPayment"]);
            }
            catch (Exception e)
            {
                LogService.TrackException(e, new Dictionary<string, string> {
                    { "name", "InternalService.InternalService" },
                    { "arguments", arguments.ToJson() }
                });
                throw;
            }
		}
		#endregion UserHasPaymentAsync

		#region UserUpdatePinAsync
		public async Task UserUpdatePinAsync(string oldPin, string pin, string confirmPin)
        {
            var arguments = new UserUpdatePinArguments
            (
                oldPin: oldPin,
                pin: pin,
                confirmPin: confirmPin
            );

            try
            {
                var result = await Server.PostAsync("Internal/User/Pin", arguments: arguments);

                LogService.TrackEvent("InternalService.UserUpdatePinAsync", new Dictionary<string, string> {
                    { "arguments", arguments.ToJson() },
                    { "result", result.ToJson() }
                });
            }
            catch (Exception e)
            {
                LogService.TrackException(e, new Dictionary<string, string> {
                    { "name", "InternalService.UserUpdatePinAsync" },
                    { "arguments", arguments.ToJson() }
                });
                throw;
            }
        }
        #endregion UserUpdatePinAsync

        #region UserForgotPinAsync
        public async Task<UserForgotPinResult> UserForgotPinAsync(string pin)
        {
            var arguments = new UserForgotPinArguments
            (
                pin: pin
            );

            try
            {
                var result = await Server.PostAsync<UserForgotPinResult>("Internal/User/ForgotPin", arguments: arguments);

                LogService.TrackEvent("InternalService.UserForgotPinAsync", new Dictionary<string, string> {
                    { "arguments", arguments.ToJson() },
                    { "result", result.ToJson() }
                });

                return result;
            }
            catch (Exception e)
            {
                LogService.TrackException(e, new Dictionary<string, string> {
                    { "name", "InternalService.UserForgotPinAsync" },
                    { "arguments", arguments.ToJson() }
                });
                throw;
            }
        }
        #endregion UserForgotPinAsync

        #region UserCheckPinAsync
        public async Task<bool> UserCheckPinAsync(string pin)
        {
            var arguments = new UserCheckPinArguments
            (
                pin: pin
            );

            try
            {
                var result = await Server.GetAsync("Internal/User/CheckPin", arguments: arguments);

                LogService.TrackEvent("InternalService.UserCheckPinAsync", new Dictionary<string, string> {
                    { "arguments", arguments.ToJson() },
                    { "result", result.ToJson() }
                });

                return Convert.ToBoolean(result["checkPin"]);
            }
            catch (Exception e)
            {
                LogService.TrackException(e, new Dictionary<string, string> {
                    { "name", "InternalService.UserCheckPinAsync" },
                    { "arguments", arguments.ToJson() }
                });
                throw;
            }
		}
        #endregion UserCheckPinAsync

        #region TicketPayWeb
        public async Task<PaymentMediaCreateWebCardResult> TicketPayWeb(
			string commerceCode,
			int publicTicketId, int publicPaymentId,
            string login, int orderId, PaymentMediaCreateType paymentMediaCreateType, decimal amount,
            string deviceManufacturer, string deviceModel, string deviceName, string deviceSerial, string deviceId, string deviceOperator, string deviceImei, string deviceMac, string operatorSim, string operatorMobile)
        {
            var arguments = new TicketPayWebArguments(
				commerceCode: commerceCode,
				publicTicketId: publicTicketId,
                publicPaymentId: publicPaymentId,
                login: login,
                orderId: orderId,
                paymentMediaCreateType: paymentMediaCreateType,
                amount: amount,
                deviceManufacturer: deviceManufacturer,
                deviceModel: deviceModel,
                deviceName: deviceName,
                deviceSerial: deviceSerial,
                deviceId: deviceId,
                deviceOperator: deviceOperator,
                deviceImei: deviceImei,
                deviceMac: deviceMac,
                operatorSim: operatorSim,
                operatorMobile: operatorMobile
            );

            try
            {
                var result = await Server.PostAsync<PaymentMediaCreateWebCardResult>("Internal/Ticket/PayWeb", arguments: arguments);

                LogService.TrackEvent("InternalService.TicketPayWebAsync", new Dictionary<string, string> {
                    { "arguments", arguments.ToJson() },
                    { "result", result.ToJson() }
                });

                return result;
            }
            catch (Exception e)
            {
                LogService.TrackException(e, new Dictionary<string, string> {
                    { "name", "InternalService.PaymentMediaCreateWebCardAsync" },
                    { "arguments", arguments.ToJson() }
                });
                throw;
            }
        }
        #endregion TicketPayWeb

        #region PaymentMediaCreateWebCardAsync
        public async Task<PaymentMediaCreateWebCardResult> PaymentMediaCreateWebCardAsync(string commerceCode, string pin, string name, int orderId, int publicPaymentMediaId, int publicTicketId, int publicPaymentId, string bankEntity,
			string login, PaymentMediaCreateType paymentMediaCreateType, decimal amount,
			string deviceManufacturer, string deviceModel, string deviceName, string deviceSerial, string deviceId, string deviceOperator, string deviceImei, string deviceMac, string operatorSim, string operatorMobile)
        {
            var arguments = new PaymentMediaCreateWebCardArguments(
				commerceCode: commerceCode,
                pin: pin,
                name: name,
                orderId: orderId,
                publicPaymentMediaId: publicPaymentMediaId,
                publicTicketId: publicTicketId,
                publicPaymentId: publicPaymentId,
                bankEntity: bankEntity,
                login: login,
                paymentMediaCreateType: paymentMediaCreateType,
                amount: amount,
                deviceManufacturer: deviceManufacturer,
                deviceModel: deviceModel,
                deviceName: deviceName,
                deviceSerial: deviceSerial,
                deviceId: deviceId,
                deviceOperator: deviceOperator,
                deviceImei: deviceImei,
                deviceMac: deviceMac,
                operatorSim: operatorSim,
                operatorMobile: operatorMobile
            );

            try
            {
                var result = await Server.PostAsync<PaymentMediaCreateWebCardResult>("Internal/PaymentMedia/WebCard", arguments: arguments);

                LogService.TrackEvent("InternalService.PaymentMediaCreateWebCardAsync", new Dictionary<string, string> {
                    { "arguments", arguments.ToJson() },
                    { "result", result.ToJson() }
                });

                return result;
            }
            catch (Exception e)
            {
                LogService.TrackException(e, new Dictionary<string, string> {
                    { "name", "InternalService.PaymentMediaCreateWebCardAsync" },
                    { "arguments", arguments.ToJson() }
                });
                throw;
            }
        }
		#endregion PaymentMediaCreateWebCardAsync

		#region PaymentMediaCreateWebCardSabadellAsync
		public async Task PaymentMediaCreateWebCardSabadellAsync(PaymentMediaCreateWebCardSabadellArguments arguments)
        {
            try
            {
                var result = await Server.PostAsync("Sabadell/WebCard", arguments: arguments);

                LogService.TrackEvent("InternalService.PaymentMediaCreateWebCardSabadellAsync", new Dictionary<string, string> {
                    { "arguments", arguments.ToJson() },
                    { "result", result.ToJson() }
                });
            }
            catch (Exception e)
            {
                LogService.TrackException(e, new Dictionary<string, string> {
                    { "name", "InternalService.PaymentMediaCreateWebCardSabadellAsync" },
                    { "arguments", arguments.ToJson() }
                });
                throw;
            }
        }
		#endregion PaymentMediaCreateWebCardSabadellAsync

		#region PaymentMediaCreateWebCardRefundSabadellAsync
		public async Task<PaymentMediaPayResult> PaymentMediaCreateWebCardRefundSabadellAsync(string commerceCode, int publicPaymentMediaId, int publicTicketId, int publicPaymentId, decimal amount, string currency, int orderId, int terminal)
        {
            var arguments = new PaymentMediaCreateWebCardRefundSabadellArguments(
				commerceCode: commerceCode,
				publicPaymentMediaId: publicPaymentMediaId,
                publicTicketId: publicTicketId,
                publicPaymentId: publicPaymentId,
                amount: amount,
                currency: currency,
                orderId: orderId,
                terminal: terminal
            );

            try
            {
                var result = await Server.PostAsync<PaymentMediaPayResult>("Sabadell/WebCardRefund", arguments: arguments);

                LogService.TrackEvent("InternalService.PaymentMediaCreateWebCardRefundSabadellAsync", new Dictionary<string, string> {
                    { "arguments", arguments.ToJson() },
                    { "result", result.ToJson() }
                });

                return result;
            }
            catch (Exception e)
            {
                LogService.TrackException(e, new Dictionary<string, string> {
                    { "name", "InternalService.PaymentMediaCreateWebCardRefundSabadellAsync" },
                    { "arguments", arguments.ToJson() }
                });
                throw;
            }
		}
        #endregion PaymentMediaCreateWebCardRefundSabadellAsync

        #region PaymentMediaCreateWebCardConfirmSabadellAsync
        public async Task<PaymentMediaPayResult> PaymentMediaCreateWebCardConfirmSabadellAsync(int publicPaymentMediaId)
        {
            var arguments = new PaymentMediaCreateWebCardConfirmSabadellArguments(
                publicPaymentMediaId: publicPaymentMediaId
            );

            try
            {
                var result = await Server.PostAsync<PaymentMediaPayResult>("Sabadell/WebCardConfirm", arguments: arguments);

                LogService.TrackEvent("InternalService.PaymentMediaCreateWebCardConfirmSabadellAsync", new Dictionary<string, string> {
                    { "arguments", arguments.ToJson() },
                    { "result", result.ToJson() }
                });

                return result;
            }
            catch (Exception e)
            {
                LogService.TrackException(e, new Dictionary<string, string> {
                    { "name", "InternalService.PaymentMediaCreateWebCardConfirmSabadellAsync" },
                    { "arguments", arguments.ToJson() }
                });
                throw;
            }
        }
        #endregion PaymentMediaCreateWebCardConfirmSabadellAsync

        #region PaymentMediaPayAsync
        public async Task<PaymentMediaPayResult> PaymentMediaPayAsync(string pin, int publicPaymentMediaId, int publicTicketId, int publicPaymentId, int order, decimal amount, string login)
        {
            var arguments = new PaymentMediaPayArguments(
                pin: pin,
                publicPaymentMediaId: publicPaymentMediaId,
                publicTicketId: publicTicketId,
                publicPaymentId: publicPaymentId,
                order: order,
                amount: amount,
                login: login
            );

            try
            {
                var result = await Server.PostAsync<PaymentMediaPayResult>("Internal/PaymentMedia/Pay", arguments: arguments);

                LogService.TrackEvent("InternalService.PaymentMediaPayAsync", new Dictionary<string, string> {
                    { "arguments", arguments.ToJson() },
                    { "result", result.ToJson() }
                });

                return result;
            }
            catch (Exception e)
            {
                LogService.TrackException(e, new Dictionary<string, string> {
                    { "name", "InternalService.PaymentMediaPayAsync" },
                    { "arguments", arguments.ToJson() }
                });
                throw;
            }
        }
        #endregion PaymentMediaPayAsync

        #region PaymentMediaRechargeAsync
        public async Task<PaymentMediaRechargeResult> PaymentMediaRechargeAsync(PaymentMediaRechargeArguments arguments)
        {
            try
            {
                var result = await Server.PostAsync<PaymentMediaRechargeResult>("Internal/PaymentMedia/Recharge", arguments: arguments);

                LogService.TrackEvent("InternalService.PaymentMediaRechargeAsync", new Dictionary<string, string> {
                    { "arguments", arguments.ToJson() },
                    { "result", result.ToJson() }
                });

                return result;
            }
            catch (Exception e)
            {
                LogService.TrackException(e, new Dictionary<string, string> {
                    { "name", "InternalService.PaymentMediaRechargeAsync" },
                    { "arguments", arguments.ToJson() }
                });
                throw;
            }
        }
        #endregion PaymentMediaPayAsync

        #region PaymentMediaRefundAsync
        public async Task<PaymentMediaPayResult> PaymentMediaRefundAsync(string commerceCode, string pin, int? publicPaymentMediaId, int publicTicketId, int publicPaymentId, int order, decimal amount)
        {
            var arguments = new PaymentMediaRefundArguments(
				commerceCode: commerceCode,
				pin: pin,
                publicPaymentMediaId: publicPaymentMediaId,
                publicTicketId: publicTicketId,
                publicPaymentId: publicPaymentId,
                order: order,
                amount: amount
            );

            try
            {
                var result = await Server.PostAsync<PaymentMediaPayResult>("Internal/PaymentMedia/Refund", arguments: arguments);

                LogService.TrackEvent("InternalService.PaymentMediaRefundAsync", new Dictionary<string, string> {
                    { "arguments", arguments.ToJson() },
                    { "result", result.ToJson() }
                });

                return result;
            }
            catch (Exception e)
            {
                LogService.TrackException(e, new Dictionary<string, string> {
                    { "name", "InternalService.PaymentMediaRefundAsync" },
                    { "arguments", arguments.ToJson() }
                });
                throw;
            }
        }
        #endregion PaymentMediaRefundAsync

        #region PaymentMediaDeleteAsync
        public async Task PaymentMediaDeleteAsync(int id)
        {
            try
            {
                await Server.DeleteAsync("Internal/PaymentMedia", id: id);

                LogService.TrackEvent("InternalService.PaymentMediaDeleteAsync", new Dictionary<string, string> {
                    { "id", id.ToJson() }
                });
            }
            catch (Exception e)
            {
                LogService.TrackException(e, new Dictionary<string, string> {
                    { "name", "InternalService.PaymentMediaDeleteAsync" },
                    { "id", id.ToJson() }
                });
                throw;
            }
        }
		#endregion PaymentMediaDeleteAsync

		#region RechargePaymentMediaAsync 
		public async Task<PaymentMediaRechargeResult> RechargePaymentMediaAsync(PaymentMediaRechargeArguments arguments)
        {
            try
            {
                var result = await Server.PostAsync<PaymentMediaRechargeResult>("Internal/PaymentMedia/Recharge", arguments: arguments);

                LogService.TrackEvent("InternalService.RechargePaymentMediaAsync", new Dictionary<string, string> {
                    { "arguments", arguments.ToJson() },
                    { "result", result.ToJson() }
                });

                return result;
            }
            catch (Exception e)
            {
                LogService.TrackException(e, new Dictionary<string, string> {
                    { "arguments", arguments.ToJson() },
                    { "name", "InternalService.RechargePaymentMediaAsync" }
                });
                throw;
            }
        }

        #endregion RechargePaymentMediaAsync

        #region PaymentMediaGetBalanceAsync 
        public async Task<PaymentMediaGetBalanceResult> PaymentMediaGetBalanceAsync(int id)
        {
            try
            {
                var result = await Server.GetAsync<ResultBase<PaymentMediaGetBalanceResult>>("Internal/PaymentMedia/GetBalance", id: id);

                LogService.TrackEvent("InternalService.PaymentMediaGetBalanceAsync", new Dictionary<string, string> {
                    { "id", id.ToJson() },
                    { "result", result.ToJson() }
                });

                return result.Data.FirstOrDefault();
            }
            catch (Exception e)
            {
                LogService.TrackException(e, new Dictionary<string, string> {
                    { "name", "InternalService.PaymentMediaGetBalanceAsync" },
                    { "id", id.ToJson() }
                });
                throw;
            }
        }
		#endregion PaymentMediaGetBalanceAsync

		#region PaymentMediaGetBalanceToRefundAsync 
		public async Task<PaymentMediaGetBalanceToRefundResult> PaymentMediaGetBalanceToRefundAsync(int id)
        {
            try
            {
                var result = await Server.GetAsync<ResultBase<PaymentMediaGetBalanceToRefundResult>>("Internal/PaymentMedia/GetBalanceToRefund", id: id);

                LogService.TrackEvent("InternalService.PaymentMediaGetBalanceToRefundAsync", new Dictionary<string, string> {
                    { "id", id.ToJson() },
                    { "result", result.ToJson() }
                });

                return result.Data.FirstOrDefault();
            }
            catch (Exception e)
            {
                LogService.TrackException(e, new Dictionary<string, string> {
                    { "name", "InternalService.PaymentMediaGetBalanceToRefundAsync" },
                    { "id", id.ToJson() }
                });
                throw;
            }
		}
        #endregion PaymentMediaGetBalanceToRefundAsync
    }
}