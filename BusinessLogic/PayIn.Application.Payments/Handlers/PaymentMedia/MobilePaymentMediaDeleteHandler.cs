using PayIn.Application.Dto.Payments.Arguments;
using PayIn.Domain.Payments;
using PayIn.Domain.Payments.Infrastructure;
using System;
using System.Threading.Tasks;
using Xp.Application.Attributes;
using Xp.Application.Dto;
using Xp.Domain;

namespace PayIn.Application.Payments.Handlers
{
	[XpLog("PaymentMedia", "Delete")]
	public class MobilePaymentMediaDeleteHandler :
		IServiceBaseHandler<MobilePaymentMediaDeleteArguments>
	{
		public readonly IEntityRepository<PaymentMedia> Repository;
		public readonly IInternalService InternalService;

		#region Contructors
		public MobilePaymentMediaDeleteHandler(
			IEntityRepository<PaymentMedia> repository,
			IInternalService internalService
		)
		{
			if (repository == null) throw new ArgumentNullException("repository");
			if (internalService == null) throw new ArgumentNullException("internalService");

			Repository = repository;
			InternalService = internalService;
		}
		#endregion Contructors

		#region ExecuteAsync
		public async Task<dynamic> ExecuteAsync(MobilePaymentMediaDeleteArguments arguments)
		{
			var paymentMedia = (await Repository.GetAsync(arguments.Id));
			if (paymentMedia == null)
				throw new ArgumentNullException("paymentMediaId");

			await InternalService.PaymentMediaDeleteAsync(arguments.Id);

			paymentMedia.State = Common.PaymentMediaState.Delete;

			return null;
		}
		#endregion ExecuteAsync
	}
}
