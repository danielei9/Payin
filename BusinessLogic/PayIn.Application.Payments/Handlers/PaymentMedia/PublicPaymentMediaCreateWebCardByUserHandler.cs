using Microsoft.Practices.Unity;
using PayIn.Application.Dto.Payments.Arguments;
using PayIn.BusinessLogic.Common;
using PayIn.Common;
using PayIn.Domain.Payments;
using System;
using System.Linq;
using System.Threading.Tasks;
using Xp.Application.Attributes;
using Xp.Application.Dto;
using Xp.Domain;

namespace PayIn.Application.Payments.Handlers
{
	[XpLog("PaymentMedia", "CreateWebCardByUser")]
	[XpAnalytics("PaymentMedia", "CreateWebCardByUser")]
	public class PublicPaymentMediaCreateWebCardByUserHandler :
		IServiceBaseHandler<PublicPaymentMediaCreateWebCardByUserArguments>
	{
		[Dependency] public ISessionData SessionData { get; set; }
		[Dependency] public IEntityRepository<PaymentConcession> PaymentConcessionRepository { get; set; }
		[Dependency] public MobilePaymentMediaCreateWebCardHandler MobilePaymentMediaCreateWebCardHandler { get; set; }

		#region ExecuteAsync
		public async Task<dynamic> ExecuteAsync(PublicPaymentMediaCreateWebCardByUserArguments arguments)
		{
			var now = DateTime.UtcNow;

			var paymentConcession = (await PaymentConcessionRepository.GetAsync())
				.Where(x =>
					x.Concession.State == ConcessionState.Active &&
					x.Concession.Supplier.Login == SessionData.Login
				)
				.FirstOrDefault();
			if (paymentConcession == null)
				throw new ArgumentNullException("login");

			var result = await MobilePaymentMediaCreateWebCardHandler.ExecuteInternalAsync(null, PaymentMediaCreateType.WebPaymentMediaCreate, now, paymentConcession.Id, arguments.Name, arguments.BankEntity, arguments.Pin,
				arguments.Login, arguments.UserName, arguments.UserTaxName, arguments.UserTaxLastName, arguments.UserBirthday, arguments.UserTaxNumber, arguments.UserTaxAddress, arguments.UserPhone, arguments.UserEmail,
				arguments.DeviceManufacturer, arguments.DeviceModel, arguments.DeviceName, arguments.DeviceSerial, arguments.DeviceId, arguments.DeviceOperator, arguments.DeviceImei, arguments.DeviceMac, arguments.OperatorSim, arguments.OperatorMobile
			);
			return result;
		}
		#endregion ExecuteAsync
	}
}
