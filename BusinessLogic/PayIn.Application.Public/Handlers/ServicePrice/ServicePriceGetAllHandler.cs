using PayIn.Application.Dto.Arguments.ServicePrice;
using PayIn.Application.Dto.Results.ServicePrice;
using PayIn.Domain.Public;
using PayIn.BusinessLogic.Common;
using PayIn.Common;
using System;
using System.Linq;
using System.Threading.Tasks;
using Xp.Application.Dto;
using Xp.Domain;

namespace PayIn.Application.Public.Handlers
{
	public class ServicePriceGetAllHandler :
		IQueryBaseHandler<ServicePriceGetAllArguments, ServicePriceGetAllResult>
	{
		private readonly IEntityRepository<ServicePrice> Repository;
		private readonly ISessionData SessionData;

		#region Constructors
		public ServicePriceGetAllHandler(IEntityRepository<ServicePrice> repository, ISessionData sessionData)
		{
			if (repository == null)
				throw new ArgumentNullException("repository");
			Repository = repository;

			if (sessionData == null)
				throw new ArgumentNullException("sessionData");
			SessionData = sessionData;
		}
		#endregion Constructors

		#region ServicePriceGetAll
		async Task<ResultBase<ServicePriceGetAllResult>> IQueryBaseHandler<ServicePriceGetAllArguments, ServicePriceGetAllResult>.ExecuteAsync(ServicePriceGetAllArguments arguments)
		{
			var items = await Repository.GetAsync();

			if (!arguments.Filter.IsNullOrEmpty())
				items = items.Where(x =>
					x.Zone.Name.Contains(arguments.Filter) ||
					x.Zone.Concession.Name.Contains(arguments.Filter)
				);
			if (arguments.ZoneId != null)
				items = items
					.Where(x => x.Zone.Concession.Supplier.Login == SessionData.Login);

			var result = items
				.Select(x => new ServicePriceGetAllResult
				{
					Id = x.Id,
					Time = x.Time,
					Price = x.Price,
					ZoneId = x.ZoneId,
					ZoneName = x.Zone.Name,
					ConcessionId = x.Zone.ConcessionId,
					ConcessionName = x.Zone.Concession.Name
				})
				.OrderBy(x => new
				{
					x.ConcessionName,
					x.ZoneName,
					x.Time
				});

			return new ResultBase<ServicePriceGetAllResult> { Data = result };
		}
		#endregion ServicePriceGetAll
	}
}
