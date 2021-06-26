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
	[XpLog("PaymentMedia", "CreateWebCardConfirmSabadell")]
	public class PaymentMediaCreateWebCardConfirmSabadellHandler :
		IServiceBaseHandler<PaymentMediaCreateWebCardConfirmSabadellArguments>
	{
		public readonly IEntityRepository<PaymentMedia> Repository;

		#region Contructors
		public PaymentMediaCreateWebCardConfirmSabadellHandler(
			IEntityRepository<PaymentMedia> repository
		)
		{
			if (repository == null) throw new ArgumentNullException("repository");
			
			Repository = repository;
		}
		#endregion Contructors

		#region ExecuteAsync
		public async Task<dynamic> ExecuteAsync(PaymentMediaCreateWebCardConfirmSabadellArguments arguments)
		{
			var paymentMedia = (await Repository.GetAsync())
				.Where(x =>
					x.PublicId == arguments.PublicPaymentMediaId &&
					x.State == PaymentMediaState.Pending
				)
				.FirstOrDefault();
			if (paymentMedia == null)
				throw new ArgumentNullException("publicPaymentMediaId");

			paymentMedia.State = PaymentMediaState.Active;

			return paymentMedia;
		}
		#endregion ExecuteAsync
	}
}