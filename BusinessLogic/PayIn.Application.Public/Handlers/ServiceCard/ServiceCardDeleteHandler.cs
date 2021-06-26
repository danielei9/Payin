using PayIn.Application.Dto.Arguments;
using PayIn.Common;
using PayIn.Domain.Public;
using System;
using System.Threading.Tasks;
using Xp.Application.Dto;
using Xp.Domain;

namespace PayIn.Application.Public.Handlers
{
	public class ServiceCardDeleteHandler :
		IServiceBaseHandler<ServiceCardDeleteArguments>
	{
		private readonly IEntityRepository<ServiceCard> Repository;

		#region Constructors
		public ServiceCardDeleteHandler (IEntityRepository<ServiceCard> repository)
		{
			if (repository == null) throw new ArgumentNullException("repository");
			Repository = repository;
		}
		#endregion Constructors

		#region ExecuteAsync
		async Task<dynamic> IServiceBaseHandler<ServiceCardDeleteArguments>.ExecuteAsync(ServiceCardDeleteArguments arguments)
		{
			var item = (await Repository.GetAsync(arguments.Id));

			item.State = ServiceCardState.Deleted;

			return item;
		}
		#endregion ExecuteAsync
	}
}
