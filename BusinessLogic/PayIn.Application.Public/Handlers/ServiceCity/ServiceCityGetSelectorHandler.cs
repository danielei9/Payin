using PayIn.Application.Dto.Arguments.ServiceCity;
using PayIn.Application.Dto.Results.ServiceCity;
using PayIn.Common;
using PayIn.Domain.Public;
using System;
using System.Linq;
using System.Threading.Tasks;
using Xp.Application.Dto;
using Xp.Domain;

namespace PayIn.Application.Public.Handlers
{
	public class ServiceCityGetSelectorHandler :
		IQueryBaseHandler<ServiceCityGetSelectorArguments, ServiceCityGetSelectorResult>
	{
		private readonly IEntityRepository<ServiceCity> _Repository;

		#region Constructors
		public ServiceCityGetSelectorHandler(IEntityRepository<ServiceCity> repository)
		{
			if (repository == null)
				throw new ArgumentNullException("repository");
			_Repository = repository;
		}
		#endregion Constructors

		#region ExecuteAsync
		async Task<ResultBase<ServiceCityGetSelectorResult>> IQueryBaseHandler<ServiceCityGetSelectorArguments, ServiceCityGetSelectorResult>.ExecuteAsync(ServiceCityGetSelectorArguments arguments)
		{
			arguments.Filter = (arguments.Filter ?? "").ToLower();

			var items = await _Repository.GetAsync();
			if (!arguments.Filter.IsNullOrEmpty())
				items = items.Where(x => x.Name.Contains(arguments.Filter));

			var result = items
				.Select(x => new ServiceCityGetSelectorResult
				{
					Id = x.Id,
					Value = x.Name
				});

			return new ResultBase<ServiceCityGetSelectorResult> { Data = result };
		}
		#endregion ExecuteAsync
	}
}
