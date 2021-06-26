using Microsoft.Practices.Unity;
using PayIn.Application.Dto.Arguments;
using PayIn.Application.Dto.Arguments.SystemCard;
using PayIn.Application.Dto.Results;
using PayIn.Domain.Public;
using System.Linq;
using System.Threading.Tasks;
using Xp.Application.Dto;
using Xp.Domain;
using System;


namespace PayIn.Application.Public.Handlers
{
    public class ServiceCardBatchGetAllHandler :
        IQueryBaseHandler<ServiceCardBatchGetAllArguments, ServiceCardBatchGetAllResult>
    {
       
        [Dependency] public IEntityRepository<ServiceCardBatch> Repository { get; set; }

		[Dependency] public IServiceBaseHandler<SystemCardCreateArguments> SystemCardSelector2 { get; set; }
		[Dependency] public IQueryBaseHandler<ApiSystemCardGetSelectorArguments, SelectorResult> SystemCardSelector { get; set; }

		#region ExecuteAsync
		public async Task<ResultBase<ServiceCardBatchGetAllResult>> ExecuteAsync(ServiceCardBatchGetAllArguments arguments)
		{

			var items = (await Repository.GetAsync("SystemCard"));

			if (!arguments.Filter.IsNullOrEmpty())
				items = items
					.Where(x =>
						x.Name.Contains(arguments.Filter) ||
						x.UidFormat.ToString().Contains(arguments.Filter)
					);

			if (!(arguments.SystemCardId == null) || (arguments.SystemCardId > 0))
				items = items
					.Where(x =>
						x.SystemCardId == arguments.SystemCardId
					);

			var systemCardSelector = (await SystemCardSelector.ExecuteAsync(new ApiSystemCardGetSelectorArguments(""))).Data
				.ToList();
			var systemCard = systemCardSelector
				.Where(x => x.Id == arguments.SystemCardId)
				.FirstOrDefault();

			var result = items.Select(
					x => new ServiceCardBatchGetAllResult
					{
						Id = x.Id,
						Name = x.Name,
						SystemCardName = x.SystemCard.Name,
						State = x.State,
						UidFormat = x.UidFormat.ToString()
					}
				);

			return new ServiceCardBatchGetAllResultBase
			{
				SystemCards = systemCardSelector,
				SystemCardId = systemCard?.Id,
				SystemCardName = systemCard?.Value ?? "",
				Data = result
			};
		}
		#endregion ExecuteAsync
	}
}
