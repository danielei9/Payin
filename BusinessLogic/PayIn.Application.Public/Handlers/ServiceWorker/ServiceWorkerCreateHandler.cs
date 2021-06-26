using PayIn.Application.Dto.Arguments.ServiceWorker;
using PayIn.BusinessLogic.Common;
using PayIn.Domain.Public;
using System;
using System.Linq;
using System.Threading.Tasks;
using Xp.Application.Dto;
using Xp.Domain;
using PayIn.Common.Resources;
using PayIn.Common;
using Xp.Common.Exceptions;

namespace PayIn.Application.Public.Handlers
{
	public class ServiceWorkerCreateHandler :
		IServiceBaseHandler<ServiceWorkerCreateArguments>
	{
		private readonly ISessionData                       SessionData;
		private readonly IEntityRepository<ServiceWorker> Repository;
		private readonly IEntityRepository<ServiceSupplier> RepositorySupplier;
		private readonly IEntityRepository<ServiceConcession> RepositoryServiceConcession;

		#region Constructors
		public ServiceWorkerCreateHandler(
			ISessionData sessionData,
			IEntityRepository<ServiceWorker> repository,
			IEntityRepository<ServiceSupplier> repositorySupplier,
			IEntityRepository<ServiceConcession> repositoryServiceConcession
		)
		{
			if (sessionData == null)        throw new ArgumentNullException("sessionData");
			if (repository == null)         throw new ArgumentNullException("repository");
			if (repositorySupplier == null) throw new ArgumentNullException("repositorySupplier");
			if (repositoryServiceConcession == null) throw new ArgumentNullException("repositoryServiceConcession");

			SessionData        = sessionData;
			Repository         = repository;
			RepositorySupplier = repositorySupplier;
			RepositoryServiceConcession = repositoryServiceConcession;
		}
		#endregion Constructors

		#region ExecuteAsync
		async Task<dynamic> IServiceBaseHandler<ServiceWorkerCreateArguments>.ExecuteAsync(ServiceWorkerCreateArguments arguments)
		{
			var suppliers = await RepositorySupplier.GetAsync();
			var supplier = suppliers
				.Where(x => x.Login == SessionData.Login)
				.FirstOrDefault();

			//No deben haber más trabajadores que los que se indica en la empresa
				var numWorkers = (await Repository.GetAsync())
					.Where(x => x.Supplier.Login == SessionData.Login)
					.Count();

				var concession = (await RepositoryServiceConcession.GetAsync())
					.Where(x => x.Supplier.Login == SessionData.Login)
					.FirstOrDefault();

				if (numWorkers >= concession.MaxWorkers)
					throw new XpException(ServiceWorkerResources.ExceptionWorkerMaxWorkers);

			//No pueden haber 2 trabajadores con el mismo mail
			var worker = (await Repository.GetAsync())
				.Where(x => x.Login == arguments.Login)
				.FirstOrDefault(); 
			
			if (worker != null)
				throw new Exception(ServiceWorkerResources.ExceptionWorkerMailAlreadyExists);

			var serviceWorker = new ServiceWorker
			{
				Name = arguments.Name,
				Login = arguments.Login,
				State = WorkerState.Pending,
				Supplier = supplier
			};
			await Repository.AddAsync(serviceWorker);

			return serviceWorker;
		}
		#endregion ExecuteAsync
	}
}

