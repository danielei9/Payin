using PayIn.Application.Dto.Arguments.ServicePrice;
using PayIn.Application.Dto.Results.ServicePrice;
using PayIn.Common;
using PayIn.Domain.Public;
using System;
using System.Linq;
using System.Threading.Tasks;
using Xp.Application.Dto;
using Xp.Domain;

namespace PayIn.Application.Public.Handlers
{
	public class ServicePriceGetHandler :
		IQueryBaseHandler<ServicePriceGetArguments, ServicePriceGetResult>
	{
		private readonly IEntityRepository<ServicePrice> _Repository;

		#region Constructors
		public ServicePriceGetHandler(IEntityRepository<ServicePrice> repository)
		{
			if (repository == null)
				throw new ArgumentNullException("repository");
			_Repository = repository;
		}
		#endregion Constructors

		#region ServicePriceGet
		async Task<ResultBase<ServicePriceGetResult>> IQueryBaseHandler<ServicePriceGetArguments, ServicePriceGetResult>.ExecuteAsync(ServicePriceGetArguments arguments)
		{
			var items = await _Repository.GetAsync();

			var result = items
				.Where(x => x.Id == arguments.Id)
				.Select(x => new ServicePriceGetResult
				{
					Id = x.Id,
					Time = x.Time,
					Price = x.Price
				});

			return new ResultBase<ServicePriceGetResult> { Data = result };
		}
		#endregion ServicePriceGetAll
	}
}
