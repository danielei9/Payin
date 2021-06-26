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
	public class BusApiStopGetHandler : IQueryBaseHandler<BusApiStopGetArguments, BusApiStopGetResult>
	{
		[Dependency] public IEntityRepository<Stop> Repository { get; set; }

		#region ExecuteAsync
		public async Task<ResultBase<BusApiStopGetResult>> ExecuteAsync(BusApiStopGetArguments arguments)
		{
			var result = (await Repository.GetAsync())
				.Where(x => 
					x.Id == arguments.Id
				)
				.Select(x => new
				{
					x.Id,
					x.Code,
					x.MasterCode,
					x.Name,
					x.Location,
					x.GeofenceRadious,
					x.Longitude,
					x.Latitude,
					x.WaitingTime
				})
				.ToList()
				.Select(x => new BusApiStopGetResult
				 {
					 Id = x.Id,
					 Code = x.Code,
					 MasterCode = x.MasterCode,
					 Name = x.Name,
					 Location = x.Location,
					 GeofenceRadious = x.GeofenceRadious,
					 Longitude = x.Longitude,
					 Latitude = x.Latitude,
					 WaitingTime = x.WaitingTime,
					 WaitingTime_Seconds = (int)x.WaitingTime.TotalSeconds
				});

			return new ResultBase<BusApiStopGetResult> { Data = result };
		}
		#endregion ExecuteAsync
	}
}
