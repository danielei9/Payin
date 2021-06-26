using PayIn.Application.Dto.Arguments.ServiceWorker;
using PayIn.BusinessLogic.Common;
using PayIn.Domain.Public;
using serV = PayIn.Domain.Public.ServiceWorker;
using System;
using System.Linq;
using System.Threading.Tasks;
using Xp.Application.Dto;
using Xp.Domain;
using PayIn.Infrastructure.Security;

namespace PayIn.Application.Public.Handlers
{
	public class ServiceWorkerAddRoleHandler :
		IServiceBaseHandler<ServiceWorkerChangeRoleArguments>
	{
		private readonly ISessionData                       SessionData;
		private readonly IEntityRepository<serV>   Repository;
		private readonly IEntityRepository<ServiceSupplier> RepositorySupplier;

		#region Constructors
		public ServiceWorkerAddRoleHandler(
			ISessionData sessionData,
			IEntityRepository<serV> repository,
			IEntityRepository<ServiceSupplier> repositorySupplier
		)
		{
			if (sessionData == null)        throw new ArgumentNullException("sessionData");
			if (repository == null)         throw new ArgumentNullException("repository");
			if (repositorySupplier == null) throw new ArgumentNullException("repositorySupplier");

			SessionData        = sessionData;
			Repository         = repository;
			RepositorySupplier = repositorySupplier;
		}
		#endregion Constructors

		#region ExecuteAsync
		async Task<dynamic> IServiceBaseHandler<ServiceWorkerChangeRoleArguments>.ExecuteAsync(ServiceWorkerChangeRoleArguments arguments)
		{
			var suppliers = await RepositorySupplier.GetAsync();
			var supplier = suppliers
				.Where(x => x.Login == SessionData.Login)
				.FirstOrDefault();

			var serviceWorkers = await Repository.GetAsync();
			var serviceWorker = serviceWorkers
				.Where(x => x.Login == arguments.userLogin && x.SupplierId == supplier.Id)
				.FirstOrDefault();
			if (serviceWorker == null)
				return false;

			if (arguments.add.GetValueOrDefault())
				return await new SecurityRepository().AddRole(serviceWorker.Login, arguments.roleName);
			else if (arguments.remove.GetValueOrDefault())
				return await new SecurityRepository().RemoveRole(serviceWorker.Login, arguments.roleName);
			else
				return false;
		}
		#endregion ExecuteAsync
	}
}
