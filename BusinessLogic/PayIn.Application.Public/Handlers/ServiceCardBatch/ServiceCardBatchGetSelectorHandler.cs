using Microsoft.Practices.Unity;
using PayIn.Application.Dto.Arguments;
using PayIn.BusinessLogic.Common;
using PayIn.Domain.Payments;
using PayIn.Domain.Public;
using System;
using System.Linq;
using System.Threading.Tasks;
using Xp.Application.Dto;
using Xp.Domain;

namespace PayIn.Application.Public.Handlers
{
	public class ServiceCardBatchGetSelectorHandler :
		IQueryBaseHandler<ServiceCardBatchGetSelectorArguments, SelectorResult>
	{
		
		[Dependency] public IEntityRepository<ServiceCardBatch> Repository { get; set; }
		

		#region ExecuteAsync
		public async Task<ResultBase<SelectorResult>> ExecuteAsync(ServiceCardBatchGetSelectorArguments arguments)
		{
         
            var items = (await Repository.GetAsync());

			if (!arguments.Filter.IsNullOrEmpty())
				items = items.Where(x =>
					x.Name.Contains(arguments.Filter) &&
					x.State == Common.ServiceCardBatchState.Active
				);

			var result = items
				.Select(x => new SelectorResult
				{
					Id = x.Id,
					Value = x.Name
				});

			return new ResultBase<SelectorResult> { Data = result };
		}
		#endregion ExecuteAsync
	}
}
