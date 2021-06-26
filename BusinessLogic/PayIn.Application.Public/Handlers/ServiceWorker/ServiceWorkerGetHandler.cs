using PayIn.Application.Dto.Arguments.ServiceWorker;
using PayIn.Application.Dto.Results.ServiceWorker;
using PayIn.BusinessLogic.Common;
using PayIn.Common;
using PayIn.Domain.Public;
using serV = PayIn.Domain.Public.ServiceWorker;
using System;
using System.Linq;
using System.Threading.Tasks;
using Xp.Application.Dto;
using Xp.Domain;

namespace PayIn.Application.Public.Handlers
{
	public class ServiceWorkerGetHandler :
		IQueryBaseHandler<ServiceWorkerGetArguments, ServiceWorkerGetResult>
	{
		private readonly IEntityRepository<serV> _Repository;		
		private readonly ISessionData _SessionData;

		#region Constructors
		public ServiceWorkerGetHandler(IEntityRepository<serV> repository, ISessionData sessionData)
		{
			if (repository == null)
				throw new ArgumentNullException("repository");
			_Repository = repository;
		
			if (sessionData == null)
				throw new ArgumentNullException("sessionData");
			_SessionData = sessionData;
		}
		#endregion Constructors

		#region ExecuteAsync
		public async Task<ResultBase<ServiceWorkerGetResult>> ExecuteAsync(ServiceWorkerGetArguments arguments)
		{
			var items = (await _Repository.GetAsync());

			var result = items
				.Where(x => x.Id == arguments.Id)
				.Select(x => new ServiceWorkerGetResult
				{
					Id = x.Id,
					Name = x.Name,
					Login = x.Login,
					State = x.State,
					SupplierId = x.SupplierId
				});
			return new ResultBase<ServiceWorkerGetResult> { Data = result };
		}
		#endregion ExecuteAsync
	}
}
