using PayIn.Application.Dto.Payments.Arguments;
using PayIn.Application.Dto.Payments.Results;
using PayIn.BusinessLogic.Common;
using PayIn.Common;
using PayIn.Domain.Payments;
using System;
using System.Linq;
using System.Threading.Tasks;
using Xp.Application.Dto;
using Xp.Domain;

namespace PayIn.Application.Payments.Handlers
{
	public class PaymentWorkerMobileGetAllHandler :
		IQueryBaseHandler<PaymentWorkerMobileGetAllArguments, PaymentWorkerMobileGetAllResult>
	{
		private readonly SessionData SessionData;
		private readonly IEntityRepository<PaymentWorker> Repository;

		#region Contructors
		public PaymentWorkerMobileGetAllHandler(
			SessionData sessionData,
			IEntityRepository<PaymentWorker> repository
		)
		{
			if (sessionData == null) throw new ArgumentNullException("sessionData");
			if (repository == null) throw new ArgumentNullException("repository");

			SessionData = sessionData;
			Repository = repository;
		}
		#endregion Contructors

		#region ExecuteAsync
		public async Task<ResultBase<PaymentWorkerMobileGetAllResult>> ExecuteAsync(PaymentWorkerMobileGetAllArguments arguments)
		{
            var result = (await Repository.GetAsync())
				.Where(
					x => x.Login == SessionData.Login && 
					(x.State == WorkerState.Active || x.State == WorkerState.Pending)
				)
				.Select(x => new PaymentWorkerMobileGetAllResult
                {
					Id = x.Id,
                    ConcessionName = x.Concession.Concession.Name,
                    State = x.State
				})
				.Skip(arguments.Skip)
				.Take(arguments.Top)
				.ToList();

			return new ResultBase<PaymentWorkerMobileGetAllResult> { Data = result };
		}
        #endregion ExecuteAsync
    }
}
