using PayIn.Application.Dto.Internal.Arguments;
using PayIn.BusinessLogic.Common;
using PayIn.Common;
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
	[XpLog("PaymentMedia", "CreateWebCardRefundSabadell")]
	public class PaymentMediaCreateWebCardRefundSabadellHandler :
		IServiceBaseHandler<PaymentMediaCreateWebCardRefundSabadellArguments>
	{
		public readonly IEntityRepository<PaymentMedia> Repository;
		public readonly IPaymentGatewayAdapter PaymentGatewayAdapter;
		public readonly ISessionData SessionData;

		#region Contructors
		public PaymentMediaCreateWebCardRefundSabadellHandler(
			IEntityRepository<PaymentMedia> repository,
			IPaymentGatewayAdapter paymentGatewayAdapter,
			ISessionData sessionData
		)
		{
			if (repository == null) throw new ArgumentNullException("repository");
			if (paymentGatewayAdapter == null) throw new ArgumentNullException("paymentGatewayAdapter");
			if (sessionData == null) throw new ArgumentNullException("sessionData");
			
			Repository = repository;
			PaymentGatewayAdapter = paymentGatewayAdapter;
			SessionData = sessionData;
		}
		#endregion Contructors

		#region ExecuteAsync
		public async Task<dynamic> ExecuteAsync(PaymentMediaCreateWebCardRefundSabadellArguments arguments)
		{
			var paymentMedia = (await Repository.GetAsync())
				.Where(x =>
					x.PublicId == arguments.PublicPaymentMediaId &&
					x.State == PaymentMediaState.Pending
				)
				.FirstOrDefault();
					
			if (paymentMedia == null)
				throw new ArgumentNullException("publicPaymentMediaId");

			var data = await PaymentGatewayAdapter.RefundAsync(arguments.CommerceCode, paymentMedia.Id, arguments.PublicPaymentMediaId, arguments.PublicTicketId, arguments.PublicPaymentId, paymentMedia.Reference, arguments.OrderId, arguments.Amount);
			var result = SabadellGatewayFunctions.GetPaymentResponse(data);

			if (result.IsError)
				paymentMedia.State = PaymentMediaState.Error;
			else
				paymentMedia.State = PaymentMediaState.Active;

			return result;
		}
		#endregion ExecuteAsync
	}
}