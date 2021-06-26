using Microsoft.Practices.Unity;
using PayIn.Application.Dto.Internal.Results;
using PayIn.Application.Dto.Payments.Arguments;
using PayIn.BusinessLogic.Common;
using PayIn.Common;
using PayIn.Common.Resources;
using PayIn.Domain.Payments;
using PayIn.Domain.Payments.Infrastructure;
using PayIn.Domain.Public;
using PayIn.Domain.Security;
using PayIn.Infrastructure.Payments.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xp.Application.Attributes;
using Xp.Application.Dto;
using Xp.Domain;

namespace PayIn.Application.Payments.Handlers
{
    [XpLog("Ticket", "Pay")]
	[XpAnalytics("Ticket", "Pay")]
	public class ApiTicketPayHandler : IServiceBaseHandler<ApiTicketPayArguments>
    {
        [Dependency] public IEntityRepository<Ticket> TicketRepository { get; set; }
        [Dependency] public IEntityRepository<Payment> PaymentRepository { get; set; }
        [Dependency] public IEntityRepository<PaymentConcession> PaymentConcessionRepository { get; set; }
        [Dependency] public IEntityRepository<ServiceOption> ServiceOptionRepository { get; set; }
        [Dependency] public ISessionData SessionData { get; set; }
        [Dependency] public IUnitOfWork UnitOfWork { get; set; }
        [Dependency] public IInternalService InternalService { get; set; }

        #region ExecuteAsync
        public async Task<dynamic> ExecuteAsync(ApiTicketPayArguments arguments)
		{
			var now = DateTime.Now;
            
			var result2 = await ExecuteInternalAsync(
				arguments.Id,
				now,
				SessionData.Login, "", "", "", null, "", "", "", "",
				arguments.DeviceManufacturer, arguments.DeviceModel, arguments.DeviceName, arguments.DeviceSerial, arguments.DeviceId, arguments.DeviceOperator, arguments.DeviceImei, arguments.DeviceMac, arguments.OperatorSim, arguments.OperatorMobile
			) as PaymentMediaCreateWebCardResult;

			return new ResultBase<PaymentMediaCreateWebCardResult>
			{
				Data = new List<PaymentMediaCreateWebCardResult>() { result2 }
			};
		}
        #endregion ExecuteAsync

        #region ExecuteInternalAsync
        public async Task<dynamic> ExecuteInternalAsync(
            int ticketId,
            DateTime now,
            string login, string userName, string userTaxName, string userTaxLastName, DateTime? userBirthday, string userTaxNumber, string userTaxAddress, string userPhone, string userEmail,
            string deviceManufacturer, string deviceModel, string deviceName, string deviceSerial, string deviceId, string deviceOperator, string deviceImei, string deviceMac, string operatorSim, string operatorMobile
        )
        {
            // GetTicket
            var ticket = (await TicketRepository.GetAsync(ticketId, "Concession"));
            if (ticket == null)
                throw new ArgumentNullException("id");

            // Comprobar pago no seguro
            if (string.Compare(login, SessionData.Login, true) != 0)
            {
                var paymentConcession = (await PaymentConcessionRepository.GetAsync())
                    .Where(x =>
                        x.Id == ticket.ConcessionId &&
                        x.AllowUnsafePayments
                    )
                    .FirstOrDefault();
                if (paymentConcession == null)
                    throw new ApplicationException(PaymentMediaResources.PaymentNotSecureNotAllowedException);
            }

            // Calcular OrderId
            var lastOrderId = (await ServiceOptionRepository.GetAsync())
                .Where(x => x.Name == "LastOrderId")
                .FirstOrDefault();
            var order = Convert.ToInt32(lastOrderId.Value) + 1;
            lastOrderId.Value = Convert.ToString(order);

            // Crear pago
            var payment = new Payment
            {
                AuthorizationCode = "",
				CommerceCode = "",
                Amount = ticket.Amount,
                Date = now.ToUTC(),
                ExternalLogin = "",
                Order = order,
                Payin = 0,
                PaymentMedia = null,
                ErrorCode = "",
                ErrorText = "",
                ErrorPublic = "",
                State = PaymentState.Pending,
                Uid = null,
                UidFormat = null,
                Seq = null,
                UserLogin = login,
                UserName = userName,
                TaxName = userTaxName,
                TaxNumber = userTaxNumber,
                TaxAddress = userTaxAddress,
                TicketId = ticketId
            };
            await PaymentRepository.AddAsync(payment);
            await UnitOfWork.SaveAsync();

            // Ejecutar pago
            var result = await InternalService.TicketPayWeb(
				SessionData.ClientId == AccountClientId.Web ? ticket.Concession.MerchantCode : "",
				ticketId, payment.Id,
                login, order, PaymentMediaCreateType.WebTicketPay, payment.Amount,
                deviceManufacturer, deviceModel, deviceName, deviceSerial, deviceId, deviceOperator, deviceImei, deviceMac, operatorSim, operatorMobile
            );

            return result;
        }
        #endregion ExecuteInternalAsync
    }
}
