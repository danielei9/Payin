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
	public class PaymentWorkerGetAllConcessionHandler :
		IQueryBaseHandler<PaymentWorkerGetAllConcessionArguments, PaymentWorkerGetAllConcessionResult>
	{
		private readonly IEntityRepository<PaymentWorker> Repository;
		private readonly ISessionData SessionData;

		#region Constructors
		public PaymentWorkerGetAllConcessionHandler(IEntityRepository<PaymentWorker> repository,ISessionData sessionData)
		{
			if (repository == null)
				throw new ArgumentNullException("repository");
			Repository = repository;

			if (sessionData == null) throw new ArgumentNullException("sessionData");
			SessionData = sessionData;
		}
		#endregion Constructors

		#region ExecuteAsync
		async Task<ResultBase<PaymentWorkerGetAllConcessionResult>> IQueryBaseHandler<PaymentWorkerGetAllConcessionArguments, PaymentWorkerGetAllConcessionResult>.ExecuteAsync(PaymentWorkerGetAllConcessionArguments arguments)
		{
			var item = (await Repository.GetAsync())
			.Where(x => x.Login == SessionData.Login && x.State == Common.WorkerState.Active && x.Login != x.Concession.Concession.Supplier.Login); 

			var result = item
			.Select(x => new
			{
				Id = x.Id,
				ConcessionId = x.ConcessionId,
				SupplierName = x.Concession.Concession.Supplier.Name,
				ConcessionName = x.Concession.Concession.Name,
				State = x.State
			})
			.Select(x => new PaymentWorkerGetAllConcessionResult
			{
				Id = x.Id,
				ConcessionId = x.ConcessionId,
				SupplierName = x.SupplierName,
				ConcessionName = x.ConcessionName,
				State = x.State
			});

			return new ResultBase<PaymentWorkerGetAllConcessionResult> { Data = result };
		}
		#endregion ExecuteAsync
	}
}
