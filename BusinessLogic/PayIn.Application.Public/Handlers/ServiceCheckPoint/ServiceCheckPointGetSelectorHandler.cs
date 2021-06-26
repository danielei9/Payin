using PayIn.Application.Dto.Arguments.ServiceCheckPoint;
using PayIn.Application.Dto.Results.ServiceCheckPoint;
using PayIn.Domain.Public;
using System;
using System.Linq;
using System.Threading.Tasks;
using Xp.Application;
using Xp.Application.Dto;
using Xp.Domain;

namespace PayIn.Application.Public.Handlers
{
	public class ServiceCheckPointGetSelectorHandler :
		IQueryBaseHandler<ServiceCheckPointGetSelectorArguments, ServiceCheckPointGetSelectorResult>
	{
		private readonly IEntityRepository<ServiceCheckPoint> _Repository;

		#region Constructors
		public ServiceCheckPointGetSelectorHandler(IEntityRepository<ServiceCheckPoint> repository)
		{
			if (repository == null)
				throw new ArgumentNullException("repository");
			_Repository = repository;
		}
		#endregion Constructors

		#region ExecuteAsync
		public async Task<ResultBase<ServiceCheckPointGetSelectorResult>> ExecuteAsync(ServiceCheckPointGetSelectorArguments arguments)
		{
			var items = await _Repository.GetAsync();

			var result = items
				.Where(x =>
					(
						(arguments.ItemId == null) ||
						(x.ItemId == arguments.ItemId)
					) && (
						(x.Name.Contains(arguments.Filter)) ||
						(x.Observations.Contains(arguments.Filter)) ||
						((x.Tag != null) && (x.Tag.Reference.Contains(arguments.Filter)))
					)
				)
				.Select(x => new ServiceCheckPointGetSelectorResult
				{
					Id = x.Id,
					Value = x.Name + (x.Tag != null && x.Tag.Reference != "" ? " (" + x.Tag.Reference + ")" : "")
				});

			return new ResultBase<ServiceCheckPointGetSelectorResult> { Data = result };
		}
		#endregion ExecuteAsync
	}
}
