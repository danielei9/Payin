using PayIn.Application.Dto.Arguments.ServiceConcession;
using PayIn.Common;
using PayIn.Domain.Public;
using System;
using System.Threading.Tasks;
using Xp.Application.Dto;
using Xp.Domain;

namespace PayIn.Application.Public.Handlers
{
	public class ServiceConcessionDeleteHandler :
		IServiceBaseHandler<ServiceConcessionDeleteArguments>
	{
		private readonly IEntityRepository<ServiceConcession> Repository;

		#region Constructors
		public ServiceConcessionDeleteHandler(IEntityRepository<ServiceConcession> repository)
		{
			if (repository == null) throw new ArgumentNullException("repository");
			Repository = repository;
		}
		#endregion Constructors

		#region ExecuteAsync
		async Task<dynamic> IServiceBaseHandler<ServiceConcessionDeleteArguments>.ExecuteAsync(ServiceConcessionDeleteArguments arguments)
		{
			var item = await Repository.GetAsync(arguments.Id);
			item.State = ConcessionState.Removed;

			return item;
		}
		#endregion ExecuteAsync
	}
}
