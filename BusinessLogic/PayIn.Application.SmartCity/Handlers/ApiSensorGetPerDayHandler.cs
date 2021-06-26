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
	public class ApiSensorGetPerDayHandler :
        IQueryBaseHandler<ApiSensorGetPerDayArguments, ApiSensorGetPerDayResult>
    {
        [Dependency] public IEntityRepository<Data> Repository { get; set; }
		[Dependency] public IEntityRepository<Sensor> SensorRepository { get; set; }

		#region ExecuteAsync
		public async Task<ResultBase<ApiSensorGetPerDayResult>> ExecuteAsync(ApiSensorGetPerDayArguments arguments)
		{
			var now = DateTime.UtcNow;
			var year = now.Year;
			//var nowHour = new DateTime(now.Year, now.Month, now.Day, now.Hour, 0, 0);
			//var lastDay4 = nowHour.AddDays(-5);

			var sensor = (await SensorRepository.GetAsync())
				.Where(x => x.Id == arguments.Id)
				.Select(x => new
				{
					x.Component.TimeZone
				})
				.FirstOrDefault();
			if (sensor != null)
			{
				var timeZoneInfo = TimeZoneInfo.FindSystemTimeZoneById(sensor.TimeZone); // "Romance Standard Time"
				//var tomorrow = TimeZoneInfo.ConvertTimeFromUtc(
				//	nowHour.AddDays(1),
				//	timeZoneInfo
				//).Date;

				var result = (await Repository.GetAsync())
					.Where(x =>
						x.SensorId == arguments.Id &&
						//x.DataSet.Timestamp >= lastDay4 &&
						x.DataSet.Timestamp.Year >= year &&
						x.Value > 0
					)
					.GroupBy(x => new { x.DataSet.Timestamp.Year, x.DataSet.Timestamp.Month, x.DataSet.Timestamp.Day, x.DataSet.Timestamp.Hour })
					.Select(x => new
					{
						x.Key.Year,
						x.Key.Month,
						x.Key.Day,
						x.Key.Hour,
						Max = x.Max(y => (decimal?)y.Value),
						Min = x.Min(y => (decimal?)y.Value),
						Avg = x.Average(y => (decimal?)y.Value)
					})
					.ToList()
					// Pasar a la zona horaria del componente y relativizarla
					.Select(x => new
					{
						Timestamp = TimeZoneInfo.ConvertTimeFromUtc(
							new DateTime(x.Year, x.Month, x.Day, x.Hour, 0, 0, DateTimeKind.Utc),
							timeZoneInfo
						),
						x.Max,
						x.Min,
						x.Avg
					})
					// Agrupar por di
					.GroupBy(x => new { x.Timestamp.Date })
					.OrderBy(x => x.Key.Date)
					// Generar formato resultado
					.Select(x => new ApiSensorGetPerDayResult
					{
						Date = x.Key.Date,
						Max = x.Max(y => y.Max),
						Min = x.Min(y => y.Min),
						Avg = x.Average(y => y.Avg)
					})
					;

				return new ResultBase<ApiSensorGetPerDayResult>
				{
					Data = result
				};
			}

			return new ResultBase<ApiSensorGetPerDayResult>
			{
				Data = null
			};
		}
		#endregion ExecuteAsync
	}
}
