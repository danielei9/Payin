using PayIn.Application.Dto.Internal.Arguments;
using PayIn.Domain.Internal;
using PayIn.Domain.Internal.Infrastructure;
using System;
using System.Threading.Tasks;
using Xp.Application.Attributes;
using Xp.Application.Dto;
using Xp.Domain;

namespace PayIn.Application.Internal.Handlers
{
	[XpLog("PaymentMedia", "WebCardRefundSabadell")]
	public class PaymentMediaWebCardRefundSabadellHandler :
		IServiceBaseHandler<PaymentMediaWebCardRefundSabadellArguments>
	{
		public readonly IEntityRepository<PaymentMedia> Repository;
		public readonly IPaymentGatewayAdapter PaymentGatewayAdapter;

		#region Contructors
		public PaymentMediaWebCardRefundSabadellHandler(
			IEntityRepository<PaymentMedia> repository,
			IPaymentGatewayAdapter paymentGatewayAdapter
		)
		{
			if (repository == null) throw new ArgumentNullException("repository");
			if (paymentGatewayAdapter == null) throw new ArgumentNullException("paymentGatewayAdapter");
			
			Repository = repository;
			PaymentGatewayAdapter = paymentGatewayAdapter;
		}
		#endregion Contructors

		#region ExecuteAsync
		public async Task<dynamic> ExecuteAsync(PaymentMediaWebCardRefundSabadellArguments arguments)
		{
			return await Task.Run(() => {
				if (!PaymentGatewayAdapter.VerifyResponse("" + arguments.Ds_Amount + arguments.Ds_Order + arguments.Ds_MerchantCode + arguments.Ds_Currency + arguments.Ds_Response, arguments.Ds_Signature))
				{
					throw new ArgumentException(arguments.Ds_Signature);
				}

				return (dynamic)null;
			});
		}
		#endregion ExecuteAsync
	}
}
