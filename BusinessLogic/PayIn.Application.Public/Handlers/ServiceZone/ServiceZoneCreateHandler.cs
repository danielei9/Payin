using PayIn.Application.Dto.Arguments.ServiceZone;
using PayIn.Domain.Public;
using System;
using System.Threading.Tasks;
using Xp.Application.Dto;
using Xp.Domain;

namespace PayIn.Application.Public.Handlers
{
	public class ServiceZoneCreateHandler :
		IServiceBaseHandler<ServiceZoneCreateArguments>
	{
		private readonly IEntityRepository<ServiceZone> _Repository;

		#region Constructors
		public ServiceZoneCreateHandler(IEntityRepository<ServiceZone> repository)
		{
			if (repository == null)
				throw new ArgumentNullException("repository");
			_Repository = repository;
		}
		#endregion Constructors

		#region ServiceZoneCreate
		async Task<dynamic> IServiceBaseHandler<ServiceZoneCreateArguments>.ExecuteAsync(ServiceZoneCreateArguments arguments)
		{
			var itemServiceZone = new ServiceZone
			{
				ConcessionId = arguments.ConcessionId,
				CancelationAmount = arguments.CancelationAmount,
				Name = arguments.Name
			};
			await _Repository.AddAsync(itemServiceZone);

			return itemServiceZone;
		}
		#endregion ServiceZoneCreate
	}
}
