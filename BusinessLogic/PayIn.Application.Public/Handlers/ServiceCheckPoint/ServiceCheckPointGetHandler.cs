using PayIn.Application.Dto.Arguments.ServiceCheckPoint;
using PayIn.Application.Dto.Results.ServiceCheckPoint;
using PayIn.Domain.Public;
using System;
using System.Linq;
using System.Threading.Tasks;
using Xp.Application.Dto;
using Xp.Domain;

namespace PayIn.Application.Public.Handlers
{
	public class ServiceCheckPointGetHandler :
		IQueryBaseHandler<ServiceCheckPointGetArguments, ServiceCheckPointGetResult>
	{
		private readonly IEntityRepository<ServiceCheckPoint> _Repository;

		#region Constructors
		public ServiceCheckPointGetHandler(IEntityRepository<ServiceCheckPoint> repository)
		{
			if (repository == null)
				throw new ArgumentNullException("repository");
			_Repository = repository;
		}
		#endregion Constructors

		#region ExecuteAsync
		async Task<ResultBase<ServiceCheckPointGetResult>> IQueryBaseHandler<ServiceCheckPointGetArguments, ServiceCheckPointGetResult>.ExecuteAsync(ServiceCheckPointGetArguments arguments)
		{
			var items = await _Repository.GetAsync();

			var result = items
				.Where(x => x.Id == arguments.Id)
				.Select(x => new ServiceCheckPointGetResult
				{
					Id           = x.Id,
					TagReference = x.TagId != null ? x.Tag.Reference : "",
					TagId        = x.TagId,
					Name         = x.Name,
					Type		 = x.Type,
					Observations = x.Observations,
					Latitude     = x.Latitude,
					Longitude    = x.Longitude
				});

			return new ResultBase<ServiceCheckPointGetResult> { Data = result };
		}
		#endregion ExecuteAsync
	}
}
