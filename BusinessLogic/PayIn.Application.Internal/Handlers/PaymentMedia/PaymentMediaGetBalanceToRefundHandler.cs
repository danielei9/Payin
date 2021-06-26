using PayIn.Application.Dto.Internal.Arguments;
using PayIn.Application.Dto.Internal.Results;
using PayIn.BusinessLogic.Common;
using PayIn.Domain.Internal;
using System;
using System.Linq;
using System.Threading.Tasks;
using Xp.Application.Attributes;
using Xp.Application.Dto;
using Xp.Domain;

namespace PayIn.Application.Internal.Handlers
{
	[XpLog("PaymentMedia", "GetBalance")]
	public class PaymentMediaGetBalanceToRefundHandler :
		IQueryBaseHandler<PaymentMediaGetBalanceToRefundArguments, PaymentMediaGetBalanceToRefundResult>
	{
		public readonly IEntityRepository<PaymentMedia> Repository;
		public readonly ISessionData SessionData;

		#region Contructors
		public PaymentMediaGetBalanceToRefundHandler(
			IEntityRepository<PaymentMedia> repository,
			ISessionData sessionData
		)
		{
			if (repository == null) throw new ArgumentNullException("repository");
			if (sessionData == null) throw new ArgumentNullException("sessionData");

			Repository = repository;
			SessionData = sessionData;
		}
		#endregion Contructors

		#region ExecuteAsync
		public async Task<ResultBase<PaymentMediaGetBalanceToRefundResult>> ExecuteAsync(PaymentMediaGetBalanceToRefundArguments arguments)
		{
			var result = (await Repository.GetAsync("User"))
				.Where(x =>
					x.PublicId == arguments.PublicId &&
					x.Type == Common.PaymentMediaType.Purse &&
					x.State == Common.PaymentMediaState.Active
				)
				.Select(x => new PaymentMediaGetBalanceToRefundResult
				{
					Id = x.Id,
					Balance = x.Balance.Value
				}).ToList();

            return new ResultBase<PaymentMediaGetBalanceToRefundResult> { Data = result };
        }
		#endregion ExecuteAsync
	}
}
