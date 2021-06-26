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
	public class PaymentMediaGetBalanceHandler :
		IQueryBaseHandler<PaymentMediaGetBalanceArguments, PaymentMediaGetBalanceResult>
	{
		public readonly IEntityRepository<PaymentMedia> Repository;
		public readonly ISessionData SessionData;

		#region Contructors
		public PaymentMediaGetBalanceHandler(
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
		public async Task<ResultBase<PaymentMediaGetBalanceResult>> ExecuteAsync(PaymentMediaGetBalanceArguments arguments)
		{
			var result = (await Repository.GetAsync("User"))
				.Where(x =>
					x.PublicId == arguments.PublicId &&
					x.Type == Common.PaymentMediaType.Purse &&
					x.State == Common.PaymentMediaState.Active &&
					x.User.Login == SessionData.Login
				)
				.Select(x => new PaymentMediaGetBalanceResult
				{
					Id = x.Id,
					Balance = x.Balance.Value
				}).ToList();

            return new ResultBase<PaymentMediaGetBalanceResult> { Data = result };
        }
		#endregion ExecuteAsync
	}
}
