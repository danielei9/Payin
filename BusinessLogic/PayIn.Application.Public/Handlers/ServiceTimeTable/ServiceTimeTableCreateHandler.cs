using PayIn.Application.Dto.Arguments.ServiceTimeTable;
using PayIn.Domain.Public;
using System;
using System.Threading.Tasks;
using Xp.Application.Dto;
using Xp.Domain;

namespace PayIn.Application.Public.Handlers
{
	public class ServiceTimeTableCreateHandler :
		IServiceBaseHandler<ServiceTimeTableCreateArguments>
	{
		private readonly IEntityRepository<ServiceTimeTable> _Repository;

		#region Constructors
		public ServiceTimeTableCreateHandler(IEntityRepository<ServiceTimeTable> repository)
		{
			if (repository == null)
				throw new ArgumentNullException("repository");
			_Repository = repository;
		}
		#endregion Constructors

		#region ExecuteAsync
		async Task<dynamic> IServiceBaseHandler<ServiceTimeTableCreateArguments>.ExecuteAsync(ServiceTimeTableCreateArguments arguments)
		{
			var itemServiceAddress = new ServiceTimeTable
			{
				ZoneId = arguments.ZoneId,
				DurationHour = arguments.DurationHour,
				FromHour = arguments.FromHour,
				FromWeekday = arguments.FromWeekday,
				UntilWeekday = arguments.UntilWeekday
			};
			await _Repository.AddAsync(itemServiceAddress);

			return itemServiceAddress;
		}
		#endregion ExecuteAsync
	}
}
