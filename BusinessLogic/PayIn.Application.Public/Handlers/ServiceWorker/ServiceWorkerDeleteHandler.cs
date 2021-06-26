using PayIn.Application.Dto.Arguments.ServiceWorker;
using PayIn.BusinessLogic.Common;
using PayIn.Domain.Public;
using serV = PayIn.Domain.Public.ServiceWorker;
using System;
using System.Threading.Tasks;
using Xp.Application.Dto;
using Xp.Domain;

namespace PayIn.Application.Public.Handlers
{
	public class ServiceWorkerDeleteHandler :
		IServiceBaseHandler<ServiceWorkerDeleteArguments>
	{
		private readonly IEntityRepository<serV> Repository;

		#region Constructors
		public ServiceWorkerDeleteHandler(ISessionData sessionData, IEntityRepository<serV> repository)
		{
			if (repository == null) throw new ArgumentNullException("repository");
			Repository = repository;
		}
		#endregion Constructors

		#region ControlItemDelete
		async Task<dynamic> IServiceBaseHandler<ServiceWorkerDeleteArguments>.ExecuteAsync(ServiceWorkerDeleteArguments arguments)
		{
			var item = await Repository.GetAsync(arguments.Id);
			await Repository.DeleteAsync(item);

			return null;
		}
		#endregion ControlItemDelete
	}
}
