using PayIn.Application.Dto.Payments.Arguments;
using PayIn.Application.Dto.Payments.Results;
using PayIn.BusinessLogic.Common;
using PayIn.Domain.Payments;
using System;
using System.Linq;
using System.Threading.Tasks;
using Xp.Application.Dto;
using Xp.Domain;

namespace PayIn.Application.Payments.Handlers
{
	public class PaymentWorkerGetAllHandler :
		IQueryBaseHandler<PaymentWorkerGetAllArguments, PaymentWorkerGetAllResult>
	{
		private readonly IEntityRepository<PaymentWorker> Repository;
		private readonly ISessionData SessionData;

		#region Constructors
		public PaymentWorkerGetAllHandler(
			ISessionData sessionData,
			IEntityRepository<PaymentWorker> repository
		)
		{
			if (sessionData == null) throw new ArgumentNullException("sessionData");
			SessionData = sessionData;

			if (repository == null)
				throw new ArgumentNullException("repository");
			Repository = repository;
		}
		#endregion Constructors

		#region ExecuteAsync
		public async Task<ResultBase<PaymentWorkerGetAllResult>> ExecuteAsync(PaymentWorkerGetAllArguments arguments)
		{
            var items = (await Repository.GetAsync())
                .Where(x =>
                    x.Concession.Concession.Supplier.Login == SessionData.Login &&
                    x.State != Common.WorkerState.Deleted
                );

            if (!arguments.Filter.IsNullOrEmpty())
                items = items
                    .Where(x =>
                        x.Name.Contains(arguments.Filter) ||
                        x.Login.Contains(arguments.Filter)
                    );

            var result = items
			    .Select(x => new PaymentWorkerGetAllResult
			    {
				    Id = x.Id,
				    State = x.State,
				    Login = x.Login,
				    Name = x.Name
			    });

			return new ResultBase<PaymentWorkerGetAllResult> { Data = result };
		}
		#endregion ExecuteAsync
	}
}

