using PayIn.Application.Dto.Arguments.ServiceWorker;
using PayIn.Application.Dto.Results.ServiceWorker;
using PayIn.Domain.Public;
using serV = PayIn.Domain.Public.ServiceWorker;
using System;
using System.Linq;
using System.Threading.Tasks;
using Xp.Application.Dto;
using Xp.Domain;

namespace PayIn.Application.Public.Handlers
{
	public class ServiceWorkerGetControlHandler :
		IQueryBaseHandler<ServiceWorkerGetControlArguments, ServiceWorkerGetControlResult>
	{
		private readonly IEntityRepository<serV> Repository;

		#region Constructors
		public ServiceWorkerGetControlHandler(IEntityRepository<serV> repository)
		{
			if (repository == null) throw new ArgumentNullException("repository");
			Repository = repository;
		}
		#endregion Constructors

		#region ExecuteAsync
		public async Task<ResultBase<ServiceWorkerGetControlResult>> ExecuteAsync(ServiceWorkerGetControlArguments arguments)
		{
			var items = await Repository.GetAsync();

			if (!arguments.Filter.IsNullOrEmpty())
				items = items.Where(x => (
					x.Name.Contains(arguments.Filter) ||
					x.Login.Contains(arguments.Filter)
				));

			var result = items
				.OrderBy(x => new { x.Name })
				.Select(x => new ServiceWorkerGetControlResult
				{
					Id          = x.Id,
					Name        = x.Name,
					Login       = x.Login
				});

			return new ResultBase<ServiceWorkerGetControlResult> { Data = result };
		}
		#endregion ExecuteAsync
	}
}
