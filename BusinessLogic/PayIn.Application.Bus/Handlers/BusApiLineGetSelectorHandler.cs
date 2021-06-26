using Microsoft.Practices.Unity;
using PayIn.Application.Dto.Bus.Arguments;
using PayIn.Domain.Bus;
using System.Linq;
using System.Threading.Tasks;
using Xp.Application.Dto;
using Xp.Domain;

namespace PayIn.Application.Bus.Handlers
{
	public class BusApiLineGetSelectorHandler :
		IQueryBaseHandler<BusApiLineGetSelectorArguments, SelectorResult>
	{
		[Dependency] public IEntityRepository<Line> Repository { get; set; }

		#region ExecuteAsync
		public async Task<ResultBase<SelectorResult>> ExecuteAsync(BusApiLineGetSelectorArguments arguments)
		{
            var items = (await Repository.GetAsync());
            if (!(arguments.Filter == null || arguments.Filter == "" ))
                items = items
                .Where(x => x.Name.Contains(arguments.Filter));

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
