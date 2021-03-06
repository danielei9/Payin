using PayIn.Application.Dto.Arguments.ServiceTimeTable;
using PayIn.Application.Dto.Results.ServiceTimeTable;
using PayIn.BusinessLogic.Common;
using PayIn.Domain.Public;
using System;
using System.Linq;
using System.Threading.Tasks;
using Xp.Application.Dto;
using Xp.Domain;

namespace PayIn.Application.Public.Handlers
{
	public class ServiceTimeTableGetBySupplierHandler :
		IQueryBaseHandler<ServiceTimeTableGetAllArguments, ServiceTimeTableGetAllResult>
	{
		private readonly IEntityRepository<ServiceTimeTable> _Repository;
		private readonly IEntityRepository<ServiceSupplier>  _RepositorySupplier;

		#region Constructors
		public ServiceTimeTableGetBySupplierHandler(
			IEntityRepository<ServiceTimeTable> repository, 
			IEntityRepository<ServiceSupplier>  repositorySupplier
		)
		{
			if (repository == null)
				throw new ArgumentNullException("repository");
			_Repository = repository;
			if (repositorySupplier == null)
				throw new ArgumentNullException("repositorySupplier");
			_RepositorySupplier = repositorySupplier;
		}
		#endregion Constructors

		#region ExecuteAsync
		async Task<ResultBase<ServiceTimeTableGetAllResult>> IQueryBaseHandler<ServiceTimeTableGetAllArguments, ServiceTimeTableGetAllResult>.ExecuteAsync(ServiceTimeTableGetAllArguments arguments)
		{
			var items = (await _Repository.GetAsync("Zone.Concession"));

			if (!arguments.Filter.IsNullOrEmpty())
				items = items.Where(x => (
					x.Zone.Name.Contains(arguments.Filter) ||
					x.Zone.Concession.Name.Contains(arguments.Filter)
				));
			
			var result = items
				.Select(x => new
				{
					Id = x.Id,
					FromHour = x.FromHour,
					DurationHour = x.DurationHour,
					FromWeekday = x.FromWeekday,
					FromWeekdayLabel = x.FromWeekday.ToString(),
					UntilWeekday = x.UntilWeekday,
					UntilWeekdayLabel = x.UntilWeekday.ToString(),
					ZoneId = x.ZoneId,
					ZoneName = x.Zone.Name,
					ConcessionId = x.Zone.ConcessionId,
					ConcessionName = x.Zone.Concession.Name,
					
				})
				.ToList()
				.Select(x => new ServiceTimeTableGetAllResult
				{

					Id = x.Id,
					/*FromHour = x.FromHour,
					DurationHour = x.DurationHour,
					UntilHour = x.FromHour + x.DurationHour,
					FromWeekday = x.FromWeekday,
					FromWeekdayLabel = x.FromWeekday.ToString(),
					UntilWeekday = x.UntilWeekday,
					UntilWeekdayLabel = x.UntilWeekday.ToString(),
					ZoneId = x.ZoneId,
					ZoneName = x.ZoneName,
					ConcessionId = x.ConcessionId,
					ConcessionName = x.ConcessionName,*/
					Title= x.ConcessionName,
					Start = new DateTime(2015, 02, 16),
					End= new DateTime(2015,02,18),
					Location=x.ConcessionName,
					Info=x.ZoneName,
					


				});
			return new ResultBase<ServiceTimeTableGetAllResult> { Data = result };
		}
		#endregion ExecuteAsync
	}
}
