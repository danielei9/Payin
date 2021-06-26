using PayIn.Application.Dto.Arguments;
using PayIn.Common;
using PayIn.Domain.Public;
using System;
using System.Threading.Tasks;
using Xp.Application.Dto;
using Xp.Domain;

namespace PayIn.Application.Public.Handlers
{
	public class ServiceCardDestroyHandler :
		IServiceBaseHandler<ServiceCardDestroyArguments>
	{
		private readonly IEntityRepository<ServiceCard> Repository;

		#region Constructors
		public ServiceCardDestroyHandler(IEntityRepository<ServiceCard> repository)
		{
			if (repository == null) throw new ArgumentNullException("repository");
			Repository = repository;
		}
		#endregion Constructors

		#region ExecuteAsync
		public async Task<dynamic> ExecuteAsync(ServiceCardDestroyArguments arguments)
		{
			var item = (await Repository.GetAsync(arguments.Id));

			item.State = ServiceCardState.Destroyed;

			return item;
		}
		#endregion ExecuteAsync
	}
}
