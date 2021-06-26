using PayIn.Application.Dto.Arguments.ServiceWorker;
using PayIn.Application.Dto.Results.ServiceWorker;
using PayIn.BusinessLogic.Common;
using PayIn.Domain.Payments;
using PayIn.Domain.Public;
using System;
using System.Linq;
using System.Threading.Tasks;
using Xp.Application.Dto;
using Xp.Domain;

namespace PayIn.Application.Public.Handlers
{
	public class WorkerMobileGetAllHandler :
		IQueryBaseHandler<WorkerMobileGetAllArguments, WorkerMobileGetAllResult>
	{
		private readonly ISessionData SessionData;
		private readonly IEntityRepository<ServiceWorker> Repository;
		private readonly IEntityRepository<PaymentWorker> RepositoryPaymentWorker;

		#region Constructors
		public WorkerMobileGetAllHandler(
			ISessionData sessionData,
			IEntityRepository<ServiceWorker> repository,
			IEntityRepository<PaymentWorker> repositoryPaymentWorker
		)
		{
			if (sessionData == null) throw new ArgumentNullException("sessionData");
			if (repository == null) throw new ArgumentNullException("repository");
			if (repositoryPaymentWorker == null) throw new ArgumentNullException("repositoryPaymentWorker");

			SessionData = sessionData;
			Repository = repository;
			RepositoryPaymentWorker = repositoryPaymentWorker;
		}
		#endregion Constructors

		#region ExecuteAsync
		async Task<ResultBase<WorkerMobileGetAllResult>> IQueryBaseHandler<WorkerMobileGetAllArguments, WorkerMobileGetAllResult>.ExecuteAsync(WorkerMobileGetAllArguments arguments)
		{
			var controlItems = (await Repository.GetAsync())
				.Where(x => x.Login == SessionData.Login);

			var paymentItems = (await RepositoryPaymentWorker.GetAsync())
				.Where(x =>
					x.Login == SessionData.Login &&
					x.State != Common.WorkerState.Deleted
				);

			if (!arguments.Filter.IsNullOrEmpty())
			{
				controlItems = controlItems.Where(x =>
					x.Supplier.Name.Contains(arguments.Filter)
				);
				paymentItems = paymentItems.Where(x =>
					x.Concession.Concession.Name.Contains(arguments.Filter)
				);
			}

			var result = controlItems
				.Select(x => new WorkerMobileGetAllResult
				{
					Id = x.Id,
					ConcessionName = x.Supplier.Name,
					State = x.State,
					Type = Common.WorkerType.Service
				})
				.Union(
					paymentItems
					.Select(x => new WorkerMobileGetAllResult
					{
						Id = x.Id,
						ConcessionName = x.Concession.Concession.Name,
						State = x.State,
						Type = Common.WorkerType.Payments
					})
				)
				.OrderBy(x => new { x.ConcessionName });

			return new ResultBase<WorkerMobileGetAllResult> { Data = result };

		}
		#endregion ExecuteAsync
	}
}
