using PayIn.Application.Dto.Internal.Arguments;
using PayIn.Common;
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
	[XpLog("PaymentMedia", "CreateWebCardSabadell")]
	public class PaymentMediaCreateWebCardSabadellHandler :
		IServiceBaseHandler<PaymentMediaCreateWebCardSabadellArguments>
	{
		public readonly IEntityRepository<PaymentMedia> Repository;
		public readonly IPaymentGatewayAdapter PaymentGatewayAdapter;

		#region Contructors
		public PaymentMediaCreateWebCardSabadellHandler(
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
        public async Task<dynamic> ExecuteAsync(PaymentMediaCreateWebCardSabadellArguments arguments)
        {
            // Verificar MerchantCode
            if (!PaymentGatewayAdapter.VerifyCommerceCode(arguments.CommerceCode))
                throw new ArgumentException("Error", "Ds_MerchantCode");

            if (arguments.PaymentMediaCreateType != PaymentMediaCreateType.WebTicketPay)
            {
                var paymentMedia = (await Repository.GetAsync())
                    .Where(x =>
                        x.Id == arguments.PaymentMediaId &&
                        x.State == PaymentMediaState.Pending
                    )
                    .FirstOrDefault();
                if (paymentMedia == null)
                    throw new ApplicationException("INTERNAL: Payment Media {0} doesn't exist or isn't confirmation pending".FormatString(arguments.PaymentMediaId));
                paymentMedia.Reference = arguments.CardIdentifier;
                paymentMedia.ExpirationMonth = arguments.ExpirationMonth;
                paymentMedia.ExpirationYear = arguments.ExpirationYear;
                paymentMedia.Number = "{0} {1} {2} {3}".FormatString(
                    arguments.CardNumberHash.Substring(0, 4),
                    arguments.CardNumberHash.Substring(4, 4),
                    arguments.CardNumberHash.Substring(8, 4),
                    arguments.CardNumberHash.Substring(12)
                );
                if (arguments.IsError)
                    paymentMedia.State = PaymentMediaState.Error;
            }

			return null;
		}
		#endregion ExecuteAsync
	}
}
