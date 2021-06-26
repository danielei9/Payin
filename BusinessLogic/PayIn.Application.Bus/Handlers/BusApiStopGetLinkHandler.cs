using Microsoft.Practices.Unity;
using PayIn.Application.Dto.Bus.Arguments;
using PayIn.Application.Dto.Bus.Results;
using PayIn.Domain.Bus;
using System.Linq;
using System.Threading.Tasks;
using Xp.Application.Dto;
using Xp.Domain;

namespace PayIn.Application.Bus.Handlers
{
	public class BusApiStopGetLinkHandler : IQueryBaseHandler<BusApiStopGetLinkArguments, BusApiStopGetLinkResult>
	{
		[Dependency] public IEntityRepository<Link> Repository { get; set; }

		#region ExecuteAsync
		public async Task<ResultBase<BusApiStopGetLinkResult>> ExecuteAsync(BusApiStopGetLinkArguments arguments)
		{
			var result = (await Repository.GetAsync())
				.Where(x => 
					x.Id == arguments.Id
				)
				.Select(x => new
				{
					x.Weight,
					x.Time,
				})
				.ToList()
				.Select(x => new BusApiStopGetLinkResult
				 {
					 Weight = x.Weight,
					 Time = x.Time,
					 Time_Seconds = (int)x.Time.TotalSeconds
				 });

			return new ResultBase<BusApiStopGetLinkResult>
			{
				Data = result
			};
		}
		#endregion ExecuteAsync
	}
}
