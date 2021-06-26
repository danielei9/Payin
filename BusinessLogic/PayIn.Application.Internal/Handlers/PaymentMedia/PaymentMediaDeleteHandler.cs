using PayIn.Application.Dto.Internal.Arguments;
using PayIn.Domain.Internal;
using System;
using System.Linq;
using System.Threading.Tasks;
using Xp.Application.Attributes;
using Xp.Application.Dto;
using Xp.Domain;

namespace PayIn.Application.Internal.Handlers
{
	[XpLog("PaymentMedia", "Delete")]
	public class PaymentMediaDeleteHandler :
		IServiceBaseHandler<PaymentMediaDeleteArguments>
	{
		public readonly IEntityRepository<PaymentMedia> Repository;

		#region Contructors
		public PaymentMediaDeleteHandler(
			IEntityRepository<PaymentMedia> repository
		)
		{
			if (repository == null) throw new ArgumentNullException("repository");

			Repository = repository;
		}
		#endregion Contructors

		#region ExecuteAsync
		public async Task<dynamic> ExecuteAsync(PaymentMediaDeleteArguments arguments)
		{
			var paymentMedia = (await Repository.GetAsync())
				.Where(x => x.PublicId == arguments.PublicId)
				.FirstOrDefault();
			//if (paymentMedia == null)
			//	throw new ArgumentNullException("paymentMediaId");

			if (paymentMedia != null)
				paymentMedia.State = Common.PaymentMediaState.Delete;

			return null;
		}
		#endregion ExecuteAsync
	}
}
