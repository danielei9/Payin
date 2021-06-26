using PayIn.Application.Dto.Arguments.ServiceWorker;
using PayIn.Domain.Public;
using serV = PayIn.Domain.Public.ServiceWorker;
using System;
using System.Threading.Tasks;
using Xp.Application.Dto;
using Xp.Domain;

namespace PayIn.Application.Public.Handlers
{
	public class ServiceWorkerUpdateHandler :
		IServiceBaseHandler<ServiceWorkerUpdateArguments>
	{
		private readonly IEntityRepository<serV> Repository;

		#region Constructors
		public ServiceWorkerUpdateHandler(IEntityRepository<serV> repository)
		{
			if (repository == null)
				throw new ArgumentNullException("repository");
			Repository = repository;
		}
		#endregion Constructors

		#region ExecuteAsync
		async Task<dynamic> IServiceBaseHandler<ServiceWorkerUpdateArguments>.ExecuteAsync(ServiceWorkerUpdateArguments arguments)
		{
			var serviceWorkerItem = await Repository.GetAsync(arguments.Id);
			serviceWorkerItem.Name  = arguments.Name;
			serviceWorkerItem.Login = arguments.Login;

			return serviceWorkerItem;
		}
		#endregion ExecuteAsync
	}
}
