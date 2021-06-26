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
	public class ServiceWorkerGetSelectorHandler :
		IQueryBaseHandler<ServiceWorkerGetSelectorArguments, ServiceWorkerGetSelectorResult>
	{
		private readonly IEntityRepository<serV> _Repository;

		#region Constructors
		public ServiceWorkerGetSelectorHandler(IEntityRepository<serV> repository)
		{
			if (repository == null)
				throw new ArgumentNullException("repository");
			_Repository = repository;
		}
		#endregion Constructors

		#region ExecuteAsync
		public async Task<ResultBase<ServiceWorkerGetSelectorResult>> ExecuteAsync(ServiceWorkerGetSelectorArguments arguments)
		{
			var items = await _Repository.GetAsync();

			var result = items
				.Where(x => x.Name.Contains(arguments.Filter))
				.Select(x => new ServiceWorkerGetSelectorResult
				{
					Id = x.Id,
					Value = x.Name
				});

			return new ResultBase<ServiceWorkerGetSelectorResult> { Data = result };
		}
		#endregion ExecuteAsync
	}
}
