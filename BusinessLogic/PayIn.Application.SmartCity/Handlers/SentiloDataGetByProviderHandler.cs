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
	public class SentiloDataGetByProviderHandler :
        IQueryBaseHandler<SentiloDataGetByProviderArguments, SentiloDataGetByProviderResult>
    {
        [Dependency] public IEntityRepository<Data> Repository { get; set; }

        #region ExecuteAsync
        public async Task<ResultBase<SentiloDataGetByProviderResult>> ExecuteAsync(SentiloDataGetByProviderArguments arguments)
		{
			var items = (await Repository.GetAsync())
				.Where(x =>
					x.Sensor.Component.ProviderId == arguments.ProviderId
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
					x.SensorId,
					x.Value,
					x.DataSet.Timestamp,
					//x.Longitude,
					//x.Latitude
				})
				.GroupBy(x => x.SensorId)
				.OrderBy(y => y.Key)
				.ToList()
				.Select(x => new SentiloDataGetByProviderResult_Sensor
				{
					Sensor = x.Key,
					Observations = x
						.OrderByDescending(y => y.Timestamp)
						.Select(y => new SentiloDataGetByProviderResult_Observation
						{
							Value = y.Value,
							Timestamp = y.Timestamp.ToUTC(),
							//Location = y.Latitude + " " + y.Longitude
						})
				});

			return new ResultBase<SentiloDataGetByProviderResult>
			{
				Data = new SentiloDataGetByProviderResult[] {
					new SentiloDataGetByProviderResult
					{
						Sensors = result
					}
				}
			};
		}
		#endregion ExecuteAsync
	}
}
