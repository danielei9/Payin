using Microsoft.Practices.Unity;
using PayIn.Application.Dto.Bus.Arguments;
using PayIn.Domain.Bus;
using System.Linq;
using System.Threading.Tasks;
using Xp.Application.Dto;
using Xp.Domain;

namespace PayIn.Application.Bus.Handlers
{
	public class BusApiStopGetSelectorHandler :
		IQueryBaseHandler<BusApiStopGetSelectorArguments, SelectorResult>
	{
		[Dependency] public IEntityRepository<Stop> Repository { get; set; }

		#region ExecuteAsync
		public async Task<ResultBase<SelectorResult>> ExecuteAsync(BusApiStopGetSelectorArguments arguments)
		{
            var items = (await Repository.GetAsync());
            if (!(arguments.Filter == null || arguments.Filter == "" ))
                items = items
					.OrderBy(x =>
						x.Code
					)
					.Where(x =>
						x.Code == x.MasterCode &&
						x.Type == Domain.Bus.Enums.NodeType.Stop &&
						(
							x.Name.Contains(arguments.Filter) ||
							x.Code.Contains(arguments.Filter)
						)
					);

			var result = items
				.Select(x => new SelectorResult
				{
					Id = x.Id,
					Value = x.Code + " " + x.Name
				});

			return new ResultBase<SelectorResult> { Data = result };
		}
		#endregion ExecuteAsync
	}
}
