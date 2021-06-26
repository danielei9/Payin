using PayIn.Application.Dto.Payments.Arguments;
using PayIn.BusinessLogic.Common;
using PayIn.Common;
using PayIn.Domain.Payments;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xp.Application.Dto;
using Xp.Domain;

namespace PayIn.Application.Payments.Handlers
{
	public class PaymentConcessionGetSelectorHandler :
		IQueryBaseHandler<PaymentConcessionGetSelectorArguments, SelectorResult>
	{
		private readonly ISessionData SessionData;
		private readonly IEntityRepository<PaymentConcession> Repository;

		#region Constructors
		public PaymentConcessionGetSelectorHandler(
			IEntityRepository<PaymentConcession> repository,
			ISessionData sessionData
		)
		{
			if (sessionData == null) throw new ArgumentNullException("sessionData");
			if (repository == null) throw new ArgumentNullException("repository");

			SessionData = sessionData;
			Repository = repository;
		}
		#endregion Constructors

		#region ExecuteAsync
		public async Task<ResultBase<SelectorResult>> ExecuteAsync(PaymentConcessionGetSelectorArguments arguments)
		{
			var result = await ExecuteInternalAsync(arguments.Filter);

			return new ResultBase<SelectorResult> {
				Data = result
			};
		}
		#endregion ExecuteAsync

		#region ExecuteInternalAsync
		public async Task<IEnumerable<SelectorResult>> ExecuteInternalAsync(string filter)
		{
			var items = (await Repository.GetAsync())
				.Where(x =>
					(
						(x.Concession.State == ConcessionState.Active) &&
						(x.Concession.Supplier.Login == SessionData.Login)
					) || (
						(x.PaymentWorkers
							.Any(y =>
								(y.State == WorkerState.Active) &&
								(y.Login == SessionData.Login)
							)
						)
					)
				);
			if (!filter.IsNullOrEmpty())
				items = items
					.Where(x => x.Concession.Name.Contains(filter));

			var result = items
				.Select(x => new SelectorResult
				{
					Id = x.Id,
					Value = x.Concession.Name
				});

			return result;
		}
		#endregion ExecuteInternalAsync
	}
}
