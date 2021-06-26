using PayIn.Application.Dto.Arguments.ServiceZone;
using PayIn.Domain.Public;
using System;
using System.Threading.Tasks;
using Xp.Application.Dto;
using Xp.Domain;

namespace PayIn.Application.Public.Handlers
{
	public class ServiceZoneDeleteHandler :
		IServiceBaseHandler<ServiceZoneUpdateArguments>
	{
		private readonly IEntityRepository<ServiceZone> Repository;

		#region Constructors
		public ServiceZoneDeleteHandler(IEntityRepository<ServiceZone> repository)
		{
			if (repository == null) throw new ArgumentNullException("repository");
			Repository = repository;
		}
		#endregion Constructors

		#region ServiceZoneDelete
		async Task<dynamic> IServiceBaseHandler<ServiceZoneUpdateArguments>.ExecuteAsync(ServiceZoneUpdateArguments arguments)
		{
			var item = await Repository.GetAsync(arguments.Id);
			await Repository.DeleteAsync(item);

			return null;
		}
		#endregion ServiceZoneDelete
	}
}
