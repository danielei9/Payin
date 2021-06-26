using PayIn.Application.Dto.Arguments.ServiceWorker;
using PayIn.Application.Dto.Results.ServiceWorker;
using PayIn.Domain.Public;
using System;
using System.Linq;
using System.Threading.Tasks;
using Xp.Application.Dto;
using Xp.Domain;

namespace PayIn.Application.Public.Handlers
{
	public class ServiceWorkerGetAllHandler :
		IQueryBaseHandler<ServiceWorkerGetAllArguments, ServiceWorkerGetAllResult>
	{
		private readonly IEntityRepository<ServiceWorker> Repository;

		#region Constructors
		public ServiceWorkerGetAllHandler(IEntityRepository<ServiceWorker> repository)
		{
			if (repository == null) throw new ArgumentNullException("repository");
			Repository = repository;
		}
		#endregion Constructors

		#region ExecuteAsync
		async Task<ResultBase<ServiceWorkerGetAllResult>> IQueryBaseHandler<ServiceWorkerGetAllArguments, ServiceWorkerGetAllResult>.ExecuteAsync(ServiceWorkerGetAllArguments arguments)
		{
			var items = await Repository.GetAsync();

			if (!arguments.Filter.IsNullOrEmpty())
				items = items.Where(x => (
					x.Name.Contains(arguments.Filter) ||
					x.Login.Contains(arguments.Filter)
				));

			var result = items
				.OrderBy(x => new { x.Name })
				.Select(x => new ServiceWorkerGetAllResult
				{
					Id    = x.Id,
					Name  = x.Name,
					Login = x.Login,
					State = x.State
				});
		
			return new ResultBase<ServiceWorkerGetAllResult> { Data = result };
			
		}
		#endregion ExecuteAsync
	}
}
