using PayIn.Application.Dto.Arguments.ServiceConcession;
using PayIn.Domain.Public;
using System;
using System.Threading.Tasks;
using Xp.Application.Dto;
using Xp.Domain;

namespace PayIn.Application.Public.Handlers
{
	public class ServiceConcessionUpdateHandler :
		IServiceBaseHandler<ServiceConcessionUpdateArguments>
	{
		private readonly IEntityRepository<ServiceConcession> Repository;

		#region Constructors
		public ServiceConcessionUpdateHandler(IEntityRepository<ServiceConcession> repository)
		{
			if (repository == null) throw new ArgumentNullException("repository");
			Repository = repository;
		}
		#endregion Constructors

		#region ExecuteAsync
		async Task<dynamic> IServiceBaseHandler<ServiceConcessionUpdateArguments>.ExecuteAsync(ServiceConcessionUpdateArguments arguments)
		{
			var item = await Repository.GetAsync(arguments.Id);

			item.Name = arguments.Name;			
			item.MaxWorkers = arguments.MaxWorkers;
			item.State = arguments.State;

			return item;			
		}
		#endregion ExecuteAsync
	}
}

