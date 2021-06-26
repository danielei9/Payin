using PayIn.Application.Dto.Arguments.ServiceTimeTable;
using PayIn.Domain.Public;
using System;
using System.Threading.Tasks;
using Xp.Application.Dto;
using Xp.Domain;

namespace PayIn.Application.Public.Handlers
{
	public class ServiceTimeTableUpdateHandler :
		IServiceBaseHandler<ServiceTimeTableUpdateArguments>
	{
		private readonly IEntityRepository<ServiceTimeTable> _Repository;

		#region Constructors
		public ServiceTimeTableUpdateHandler(IEntityRepository<ServiceTimeTable> repository)
		{
			if (repository == null)
				throw new ArgumentNullException("repository");
			_Repository = repository;
		}
		#endregion Constructors

		#region ExecuteAsync
		async Task<dynamic> IServiceBaseHandler<ServiceTimeTableUpdateArguments>.ExecuteAsync(ServiceTimeTableUpdateArguments arguments)
		{
			var itemServiceAddress = await _Repository.GetAsync(arguments.Id);

			itemServiceAddress.DurationHour = arguments.DurationHour;
			itemServiceAddress.FromHour = arguments.FromHour;
			itemServiceAddress.FromWeekday = arguments.FromWeekday;
			itemServiceAddress.UntilWeekday = arguments.UntilWeekday;
			itemServiceAddress.ZoneId = arguments.ZoneId;

			return itemServiceAddress;
		}
		#endregion ExecuteAsync
	}
}
