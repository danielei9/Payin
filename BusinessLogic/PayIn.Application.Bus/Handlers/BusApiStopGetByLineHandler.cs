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
	public class BusApiStopGetByLineHandler : IQueryBaseHandler<BusApiStopGetByLineArguments, BusApiStopGetByLineResult>
	{
		[Dependency] public IEntityRepository<Stop> Repository { get; set; }

		#region ExecuteAsync
		public async Task<ResultBase<BusApiStopGetByLineResult>> ExecuteAsync(BusApiStopGetByLineArguments arguments)
		{
			var result = (await Repository.GetAsync())
				.Where(x => 
					x.LineId == arguments.LineId
				)
				.OrderBy(x => x.Code)
				.Select(x => new BusApiStopGetByLineResult
                {
					Id = x.Id,
					Code = x.Code,
					MasterCode = x.MasterCode,
					Name = x.Name,
                    Type = x.Type,
                    Location = x.Location,
                    Longitude = x.Longitude,
                    Latitude = x.Latitude,
					WaitingTime = x.WaitingTime,
					Links = x.Exits
						.OrderBy(y => y.To.Code)
						.Select(y => new BusApiStopGetByLineResult_Link
						{
							Id = y.Id,
							Weight = y.Weight,
							Time = y.Time,
							ToCode = y.To.Code,
							ToName = y.To.Name,
							Sense = y.Route.Sense
						})
				});

			return new ResultBase<BusApiStopGetByLineResult>
			{
				Data = result
			};
		}
		#endregion ExecuteAsync
	}
}
