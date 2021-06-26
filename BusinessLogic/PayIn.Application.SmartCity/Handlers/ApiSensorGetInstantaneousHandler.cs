using Microsoft.Practices.Unity;
using PayIn.Application.Dto.SmartCity.Arguments;
using PayIn.Application.Dto.SmartCity.Results;
using PayIn.BusinessLogic.Common;
using PayIn.Domain.SmartCity;
using System;
using System.Linq;
using System.Threading.Tasks;
using Xp.Application.Dto;
using Xp.Domain;

namespace PayIn.Application.SmartCity.Handlers
{
	public class ApiSensorGetInstantaneousHandler :
        IQueryBaseHandler<ApiSensorGetInstantaneousArguments, ApiSensorGetInstantaneousResult>
	{
		[Dependency] public IEntityRepository<Data> Repository { get; set; }
		[Dependency] public ISessionData SessionData { get; set; }

		#region ExecuteAsync
		public async Task<ResultBase<ApiSensorGetInstantaneousResult>> ExecuteAsync(ApiSensorGetInstantaneousArguments arguments)
		{
			var endDateTime =
				arguments.Period == ApiSensorGetInstantaneousArguments_Period.Day ? arguments.Date.Value.AddDays(1) :
				DateTime.UtcNow;
			//var quarter = endDateTime
			//	.GetQuarter();
			//if (arguments.Period == ApiSensorGetInstantaneousArguments_Period.Day)
			//{
			//	endDateTime = endDateTime
			//		.FloorHour()
			//		.AddMinutes(quarter * 15)
			//		.ToUTC();
			//}
			var startDate = endDateTime
				.AddDays(-1)
				.ToUTC();

			var result = (await Repository.GetAsync())
				.Where(x =>
					(x.SensorId == arguments.Id) &&
					(x.DataSet.Timestamp >= startDate) &&
					(x.DataSet.Timestamp <= endDateTime) //&&
					//(x.Sensor.State != SensorState.Delete) &&
					//(x.Sensor.Component.State != ComponentState.Delete) &&
					//(x.Sensor.Component.Device.State != DeviceState.Delete) &&
					//(x.Sensor.Component.Device.Concession.Login == SessionData.Login)
				)
				.Select(x => new
				{
					x.Id,
					x.DataSet.Timestamp,
					Value = x.Value,
					x.Sensor.IsAcumulative,
					x.Sensor.Unit,
					PowerContract = x.EnergyTariffPrice.PowerContract * x.EnergyTariffPrice.PowerContractFactor
				})
				.OrderBy(x => x.Timestamp)
				.ToList();
			//var result = (await Repository.GetAsync())
			//	.Where(x =>
			//		x.SensorId == arguments.Id &&
			//		(
			//			(
			//				arguments.Period == ApiSensorGetInstantaneousArguments.PeriodType.LastDay &&
			//				x.DataSet.Timestamp >= lastDay
			//			) ||
			//			(
			//				arguments.Period == ApiSensorGetInstantaneousArguments.PeriodType.LastHour &&
			//				x.DataSet.Timestamp >= lastHour
			//			)
			//		)
			//	)
			//	.Select(x => new
			//	{
			//		x.Id,
			//		x.DataSet.Timestamp,
			//		x.Value,
			//		x.Sensor.IsAcumulative,
			//		x.Sensor.Unit
			//	})
			//	.OrderBy(x => x.Timestamp)
			//	.ToList();
			
			return new ApiSensorGetInstantaneousResultBase
			{
				Data = result
					.Select(x => new ApiSensorGetInstantaneousResult
					{
						Id = x.Id,
						Timestamp = x.Timestamp.ToUTC(),
						Value = x.Value
					}),
				Unit = result.FirstOrDefault()?.Unit ?? ""
			};
		}
		#endregion ExecuteAsync
	}
}
