using PayIn.Application.Dto.Arguments.ServiceZone;
using PayIn.Common;
using PayIn.Domain.Public;
using System;
using System.Threading.Tasks;
using Xp.Application.Dto;
using Xp.Domain;

namespace PayIn.Application.Public.Handlers
{
	public class ServiceZoneUpdateHandler :
		IServiceBaseHandler<ServiceZoneUpdateArguments>
	{
		private readonly IEntityRepository<ServiceZone> _Repository;

		#region Constructors
		public ServiceZoneUpdateHandler(IEntityRepository<ServiceZone> repository)
		{
			if (repository == null)
				throw new ArgumentNullException("repository");
			_Repository = repository;
		}
		#endregion Constructors

		#region ExecuteAsync
		async Task<dynamic> IServiceBaseHandler<ServiceZoneUpdateArguments>.ExecuteAsync(ServiceZoneUpdateArguments arguments)
		{
			var itemServiceZone = await _Repository.GetAsync(arguments.Id);
			itemServiceZone.Name = arguments.Name;
			itemServiceZone.CancelationAmount = arguments.CancelationAmount;
			itemServiceZone.ConcessionId = arguments.ConcessionId;

			return itemServiceZone;
		}
		#endregion ExecuteAsync
	}
}
