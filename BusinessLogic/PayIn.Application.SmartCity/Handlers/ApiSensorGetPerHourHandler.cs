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
	public class ApiSensorGetPerHourHandler :
        IQueryBaseHandler<ApiSensorGetPerHourArguments, ApiSensorGetPerHourResult>
    {
        [Dependency] public IEntityRepository<Data> Repository { get; set; }
		[Dependency] public IEntityRepository<Sensor> SensorRepository { get; set; }

		#region ExecuteAsync
		public async Task<ResultBase<ApiSensorGetPerHourResult>> ExecuteAsync(ApiSensorGetPerHourArguments arguments)
		{
			var now = DateTime.UtcNow;
			var nowHour = new DateTime(now.Year, now.Month, now.Day, now.Hour, 0, 0);
			var lastDay4 = nowHour.AddDays(-5);

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
				var tomorrow = TimeZoneInfo.ConvertTimeFromUtc(
					nowHour.AddDays(1),
					timeZoneInfo
				).Date;

				var result = (await Repository.GetAsync())
					.Where(x =>
						x.SensorId == arguments.Id &&
						x.DataSet.Timestamp >= lastDay4
					)
					.GroupBy(x => new { x.DataSet.Timestamp.Year, x.DataSet.Timestamp.Month, x.DataSet.Timestamp.Day, x.DataSet.Timestamp.Hour })
					.Select(x => new
					{
						x.Key.Year,
						x.Key.Month,
						x.Key.Day,
						x.Key.Hour,
						Avg = x.Average(y => (decimal?)y.Value)
					})
					.ToList()
					// Pasar a la zona horaria del componente y relativizarla
					.Select(x => new
					{
						Timestamp = TimeZoneInfo.ConvertTimeFromUtc(
							new DateTime(x.Year, x.Month, x.Day, x.Hour, 0, 0, DateTimeKind.Utc),
							timeZoneInfo
						) - tomorrow,
						x.Avg
					})
					// Extraer el dia y la hora
					.Select(x => new
					{
						Hour = (Convert.ToInt32(x.Timestamp.TotalHours) % 24) + 24,
						Day = - x.Timestamp.Days,
						x.Avg
					})
					// Agrupar por hora
					.GroupBy(x => x.Hour)
					.OrderBy(x => x.Key)
					// Generar formato resultado
					.Select(x => new ApiSensorGetPerHourResult
					{
						Hour = x.Key,
						Day0 = x.Where(y => y.Day == 0).Select(y => y.Avg).FirstOrDefault(),
						Day1 = x.Where(y => y.Day == 1).Select(y => y.Avg).FirstOrDefault(),
						Day2 = x.Where(y => y.Day == 2).Select(y => y.Avg).FirstOrDefault(),
						Day3 = x.Where(y => y.Day == 3).Select(y => y.Avg).FirstOrDefault()
					})
					;

				return new ApiSensorGetPerHourResultBase
				{
					Data = result
				};
			}

			return new ApiSensorGetPerHourResultBase
			{
				Data = null
			};
		}
		#endregion ExecuteAsync
	}
}
