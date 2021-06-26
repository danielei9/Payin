using PayIn.Application.Dto.Internal.Arguments;
using PayIn.Domain.Internal;
using PayIn.Domain.Internal.Infrastructure;
using System;
using System.Linq;
using System.Threading.Tasks;
using Xp.Application.Attributes;
using Xp.Application.Dto;
using Xp.Domain;

namespace PayIn.Application.Internal.Handlers
{
	[XpLog("PaymentMedia", "PaySabadell")]
	public class PaymentMediaPaySabadellHandler :
		IServiceBaseHandler<PaymentMediaPaySabadellArguments>
	{
		public readonly IEntityRepository<PaymentMedia> Repository;
		public readonly IPaymentGatewayAdapter PaymentGatewayAdapter;

		#region Contructors
		public PaymentMediaPaySabadellHandler(
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
		public async Task<dynamic> ExecuteAsync(PaymentMediaPaySabadellArguments arguments)
		{
			if (!PaymentGatewayAdapter.VerifyResponse("" + arguments.Ds_Amount + arguments.Ds_Order + arguments.Ds_MerchantCode + arguments.Ds_Currency + arguments.Ds_Response, arguments.Ds_Signature))
			{
				throw new ArgumentException(arguments.Ds_Signature);
			} 

			var paymentMediaId = (int)Math.Floor(arguments.Ds_Order / 100.0);

			var paymentMedia = (await Repository.GetAsync())
				.Where(x => x.Id == paymentMediaId)
				.FirstOrDefault();
			if (paymentMedia == null)
				throw new ArgumentNullException("Ds_MerchantOrder");

			if (arguments.Ds_Response == 99)
				paymentMedia.State = Common.PaymentMediaState.Expired;

			return new { PublicId = paymentMedia.Id };
		}
		#endregion ExecuteAsync
	}
}
