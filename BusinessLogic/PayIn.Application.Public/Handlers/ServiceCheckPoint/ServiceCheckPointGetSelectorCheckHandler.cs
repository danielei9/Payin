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
	public class ServiceCheckPointGetSelectorCheckHandler :
		IQueryBaseHandler<ServiceCheckPointGetSelectorCheckArguments, ServiceCheckPointGetSelectorCheckResult>
	{
		private readonly IEntityRepository<ServiceCheckPoint> Repository;

		#region Constructors
		public ServiceCheckPointGetSelectorCheckHandler(IEntityRepository<ServiceCheckPoint> repository)
		{
			if (repository == null) throw new ArgumentNullException("repository");
			Repository = repository;
		}
		#endregion Constructors

		#region ExecuteAsync
		public async Task<ResultBase<ServiceCheckPointGetSelectorCheckResult>> ExecuteAsync(ServiceCheckPointGetSelectorCheckArguments arguments)
		{
			var items = await Repository.GetAsync();

			var result = items
				.Where(x =>
					(
						x.Type == Common.CheckPointType.Round
					) && (
						(arguments.ItemId == null) || (x.ItemId == arguments.ItemId)
					) && (
						(x.Name.Contains(arguments.Filter)) ||
						(x.Observations.Contains(arguments.Filter)) ||
						((x.Tag != null) && (x.Tag.Reference.Contains(arguments.Filter)))
					)
				)
				.Select(x => new ServiceCheckPointGetSelectorCheckResult
				{
					Id = x.Id,
					Value = x.Name + (x.Tag != null && x.Tag.Reference != "" ? " (" + x.Tag.Reference + ")" : "")
				});

			return new ResultBase<ServiceCheckPointGetSelectorCheckResult> { Data = result };
		}
		#endregion ExecuteAsync
	}
}
