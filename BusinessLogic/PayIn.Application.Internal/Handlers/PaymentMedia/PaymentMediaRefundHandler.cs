using PayIn.Application.Dto.Internal.Arguments;
using PayIn.Application.Dto.Internal.Results;
using PayIn.Common;
using PayIn.Common.Resources;
using PayIn.Domain.Internal;
using PayIn.Domain.Internal.Infrastructure;
using PayIn.Infrastructure.Sabadell;
using System;
using System.Linq;
using System.Threading.Tasks;
using Xp.Application.Attributes;
using Xp.Application.Dto;
using Xp.Domain;

namespace PayIn.Application.Internal.Handlers
{
	[XpLog("PaymentMedia", "Refund")]
	public class PaymentMediaRefundHandler :
		IServiceBaseHandler<PaymentMediaRefundArguments>
	{
		private readonly UserCheckPinHandler UserCheckPin;
		private readonly IEntityRepository<PaymentMedia> Repository;
		public readonly IPaymentGatewayAdapter PaymentGatewayAdapter;

		#region Contructors
		public PaymentMediaRefundHandler(
			UserCheckPinHandler userCheckPin,
			IEntityRepository<PaymentMedia> repository,
			IPaymentGatewayAdapter paymentGatewayAdapter
		)
		{
			if (userCheckPin == null) throw new ArgumentNullException("userCheckPinHandler");
			if (repository == null) throw new ArgumentNullException("repository");
			if (paymentGatewayAdapter == null) throw new ArgumentNullException("paymentGatewayAdapter");

			UserCheckPin = userCheckPin;
			Repository = repository;
			PaymentGatewayAdapter = paymentGatewayAdapter;
		}
		#endregion Contructors

		#region ExecuteAsync
		public async Task<dynamic> ExecuteAsync(PaymentMediaRefundArguments arguments)
		{
			var paymentMedia = (await Repository.GetAsync())
				.Where(x => x.PublicId == arguments.PublicPaymentMediaId)
				.FirstOrDefault();
			//if (paymentMedia == null)
			//	throw new ArgumentNullException("publicPaymentMediaId");

			// Check Pin
			if (!await UserCheckPin.ExecuteAsync(new UserCheckPinArguments(arguments.Pin)))
				throw new ArgumentException(UserResources.IncorrectPin, "pin");

			dynamic result;
			switch (paymentMedia?.Type ?? PaymentMediaType.WebCard)
			{
				case PaymentMediaType.WebCard:
					var data = await PaymentGatewayAdapter.RefundAsync(arguments.CommerceCode, paymentMedia?.Id, arguments.PublicPaymentMediaId, arguments.PublicTicketId, arguments.PublicPaymentId, paymentMedia?.Reference ?? "", arguments.Order, arguments.Amount);
					result = SabadellGatewayFunctions.GetPaymentResponse(data);
					break;
				default: //Purse
					result = new PaymentMediaRefundResult()
					{
						Amount = (decimal)paymentMedia.Balance + arguments.Amount
					};
					paymentMedia.Balance = result.Amount;
					break;
			}
			return result;
		}
		#endregion ExecuteAsync
	}
}
