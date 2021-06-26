using Microsoft.Practices.Unity;
using PayIn.Application.Dto.Internal.Results;
using PayIn.Application.Dto.Payments.Arguments;
using PayIn.Application.Dto.Payments.Results;
using PayIn.BusinessLogic.Common;
using PayIn.Common;
using PayIn.Infrastructure.Payments.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xp.Application.Attributes;
using Xp.Application.Dto;

namespace PayIn.Application.Payments.Handlers
{
	[XpLog("Ticket", "PayAndCreateWebCardByUser")]
	[XpAnalytics("Ticket", "PayAndCreateWebCardByUser")]
	public class PublicTicketPayAndCreateWebCardByUserHandler : IServiceBaseHandler<PublicTicketPayAndCreateWebCardByUserArguments>
	{
		[Dependency] public PaymentConcessionRepository PaymentConcessionRepository { get; set; }
		[Dependency] public MobileTicketCreateAndGetHandler MobileTicketCreateAndGetHandler { get; set; }
		[Dependency] public PublicPaymentMediaGetByUserHandler PublicPaymentMediaGetByUserHandler { get; set; }
		[Dependency] public MobilePaymentMediaCreateWebCardHandler MobilePaymentMediaCreateWebCardHandler { get; set; }
		[Dependency] public ISessionData SessionData { get; set; }

		#region ExecuteAsync
		public async Task<dynamic> ExecuteAsync(PublicTicketPayAndCreateWebCardByUserArguments arguments)
		{
			var now = DateTime.Now;

			var paymentConcession = (await PaymentConcessionRepository.GetAsync())
				.Where(x =>
					x.Concession.State == ConcessionState.Active &&
					x.Concession.Supplier.Login == SessionData.Login
				)
				.FirstOrDefault();
			if (paymentConcession == null)
				throw new ArgumentNullException("login");

			var result2 = await MobilePaymentMediaCreateWebCardHandler.ExecuteInternalAsync(
				arguments.TicketId,
                PaymentMediaCreateType.WebTicketPayAndPaymentMediaCreate,
				now,
                paymentConcession.Id,
				arguments.Name,
                arguments.BankEntity,
                arguments.Pin,
				arguments.Login, arguments.UserName, arguments.UserTaxName, arguments.UserTaxLastName, arguments.UserBirthday, arguments.UserTaxNumber, arguments.UserTaxAddress, arguments.UserPhone, arguments.UserEmail,
				arguments.DeviceManufacturer, arguments.DeviceModel, arguments.DeviceName, arguments.DeviceSerial, arguments.DeviceId, arguments.DeviceOperator, arguments.DeviceImei, arguments.DeviceMac, arguments.OperatorSim, arguments.OperatorMobile
			) as PaymentMediaCreateWebCardResult;

			return new ResultBase<PaymentMediaCreateWebCardResult>
			{
				Data = new List<PaymentMediaCreateWebCardResult>() { result2 }
			};
		}
        #endregion ExecuteAsync
    }
}
