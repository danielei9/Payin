using Microsoft.Practices.Unity;
using PayIn.Application.Dto.SmartCity.Arguments;
using PayIn.Application.Dto.SmartCity.Results;
using PayIn.Domain.SmartCity;
using System;
using System.Linq;
using System.Threading.Tasks;
using Xp.Application.Dto;
using Xp.Domain;

namespace PayIn.Application.SmartCity.Handlers
{
	public class SentiloDataGetBySensorHandler :
        IQueryBaseHandler<SentiloDataGetBySensorArguments, SentiloDataGetBySensorResult>
    {
        [Dependency] public IEntityRepository<Data> Repository { get; set; }

        #region ExecuteAsync
        public async Task<ResultBase<SentiloDataGetBySensorResult>> ExecuteAsync(SentiloDataGetBySensorArguments arguments)
		{
			var items = (await Repository.GetAsync())
				.Where(x =>
					x.Sensor.Component.ProviderId == arguments.ProviderId &&
					x.SensorId == arguments.SensorId
				);

			// Entre fechas
			if (arguments.From != null)
				items = items
					.Where(x => x.DataSet.Timestamp >= arguments.From);
			if (arguments.To != null)
				items = items
					.Where(x => x.DataSet.Timestamp >= arguments.To);

			// Order
			items = items
				.OrderByDescending(x => x.DataSet.Timestamp);

			// Take
			if (arguments.Limit != null)
				items = items
					.Take(arguments.Limit.Value);

			// Select
			var result = items
				.Select(x => new
				{
					x.Value,
					x.DataSet.Timestamp,
					//x.Latitude,
					//x.Longitude
				})
				.OrderByDescending(x => x.Timestamp)
				.ToList()
				.Select(x => new SentiloDataGetBySensorResult_Observation
				{
					Value = x.Value,
					Timestamp = x.Timestamp.ToUTC(),
					//Location = x.Latitude + " " + x.Longitude
				});

			return new ResultBase<SentiloDataGetBySensorResult>
			{
				Data = new SentiloDataGetBySensorResult[] {
					new SentiloDataGetBySensorResult
					{
						Observations = result
					}
				}
			};
		}
		#endregion ExecuteAsync
	}
}
