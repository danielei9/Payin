using Microsoft.Practices.Unity;
using PayIn.Application.Dto.SmartCity.Arguments;
using PayIn.Application.Dto.SmartCity.Results;
using PayIn.BusinessLogic.Common;
using PayIn.Domain.SmartCity;
using PayIn.Domain.SmartCity.Enums;
using System;
using System.Linq;
using System.Threading.Tasks;
using Xp.Application.Dto;
using Xp.Domain;

namespace PayIn.Application.SmartCity.Handlers
{
	public class ApiSensorGetPowerHandler :
		IQueryBaseHandler<ApiSensorGetPowerArguments, ApiSensorGetPowerResult>
	{
		[Dependency] public IEntityRepository<Data> Repository { get; set; }
		[Dependency] public ISessionData SessionData { get; set; }

		#region ExecuteAsync
		public async Task<ResultBase<ApiSensorGetPowerResult>> ExecuteAsync(ApiSensorGetPowerArguments arguments)
		{
			var endDateTime =
				arguments.Period == ApiSensorGetPowerArguments_Period.Day ?
					arguments.Date.Value.AddDays(1) :
				arguments.Period == ApiSensorGetPowerArguments_Period.Month ?
					arguments.Date.Value.FloorMonth().AddMonths(1) :
					DateTime.UtcNow;
			var quarter = endDateTime
				.GetQuarter();
			endDateTime = endDateTime
				.FloorHour()
				.AddMinutes(quarter * 15)
				.ToUTC();
			var startDate =
				arguments.Period == ApiSensorGetPowerArguments_Period.Last30d ? endDateTime.AddDays(-30).ToUTC() :
				arguments.Period == ApiSensorGetPowerArguments_Period.Month ? arguments.Date.Value.FloorMonth() :
					endDateTime.AddDays(-1).ToUTC();

			var data = (await Repository.GetAsync())
				.Where(x =>
					(x.SensorId == arguments.Id) &&
					(x.DataSet.Timestamp >= startDate) &&
					(x.DataSet.Timestamp <= endDateTime) &&
					(x.Sensor.State != SensorState.Delete) &&
					(x.Sensor.Component.State != ComponentState.Delete) &&
					(x.Sensor.Component.Device.State != DeviceState.Delete) &&
					(x.Sensor.Component.Device.Concession.Login == SessionData.Login)
				)
				.Select(x => new
				{
					x.Id,
					x.DataSet.Timestamp,
					Value = x.Value / 1000,
					x.Sensor.IsAcumulative,
					x.Sensor.Unit,
					Type = (PeriodType?) x.EnergyTariffPrice.Period.Type,
					x.EnergyTariffPrice.Period.Color,
					PowerContract = x.EnergyTariffPrice.PowerContract * x.EnergyTariffPrice.PowerContractFactor
				})
				.OrderBy(x => x.Timestamp)
				.ToList();

			var result = data
				.Select(x => new ApiSensorGetPowerResult
				{
					Timestamp = (arguments.Period == ApiSensorGetPowerArguments_Period.Last30d || arguments.Period == ApiSensorGetPowerArguments_Period.Month) ?
						x.Timestamp.ToUTC().FloorHour() :
						x.Timestamp.ToUTC(),
					P1Value = x.Type == PeriodType.P1 ? x.Value : (decimal?)null,
					P2Value = x.Type == PeriodType.P2 ? x.Value : (decimal?)null,
					P3Value = x.Type == PeriodType.P3 ? x.Value : (decimal?)null,
					P4Value = x.Type == PeriodType.P4 ? x.Value : (decimal?)null,
					P5Value = x.Type == PeriodType.P5 ? x.Value : (decimal?)null,
					P6Value = x.Type == PeriodType.P6 ? x.Value : (decimal?)null,
					PowerContract = x.PowerContract,
					//PowerContractCost = x.PowerContractCost,
					PowerContractMin = x.PowerContract * 0.85m,
					//PowerContractMinCost = x.PowerContractCost * 0.85m,
					PowerContractMax = x.PowerContract * 1.05m,
					//PowerContractMaxCost = x.PowerContractCost * 1.05m,
				})
				.ToList();

			// Cuando es mensual se acumula cada hora
			if (arguments.Period == ApiSensorGetPowerArguments_Period.Last30d || arguments.Period == ApiSensorGetPowerArguments_Period.Month)
			{
				result = result
					.GroupBy(x => new
					{
						x.Timestamp.Value.Year,
						x.Timestamp.Value.Month,
						x.Timestamp.Value.Day,
						x.Timestamp.Value.Hour
					})
					.Select(x => new ApiSensorGetPowerResult
					{
						Timestamp = x.Select(y => y.Timestamp).FirstOrDefault(),
						P1Value = x.Max(y => y.P1Value),
						P2Value = x.Max(y => y.P2Value),
						P3Value = x.Max(y => y.P3Value),
						P4Value = x.Max(y => y.P4Value),
						P5Value = x.Max(y => y.P5Value),
						P6Value = x.Max(y => y.P6Value),
						PowerContract = x.Max(y => y.PowerContract),
						//PowerContractCost = x.PowerContractCost,
						PowerContractMin = x.Max(y => y.PowerContractMin),
						//PowerContractMinCost = x.PowerContractCost * 0.85m,
						PowerContractMax = x.Max(y => y.PowerContractMax),
						//PowerContractMaxCost = x.PowerContractCost * 1.05m,
					})
					.ToList()
				;
			}

			return new ApiSensorGetPowerResultBase
			{
				Data = result,
				Unit = data.FirstOrDefault()?.Unit ?? "",
				// MaxValues
				P1MaxValue = data.Where(x => x.Type == PeriodType.P1).Select(x => (decimal?)x.Value).Max() ?? 0,
				P2MaxValue = data.Where(x => x.Type == PeriodType.P2).Select(x => (decimal?)x.Value).Max() ?? 0,
				P3MaxValue = data.Where(x => x.Type == PeriodType.P3).Select(x => (decimal?)x.Value).Max() ?? 0,
				P4MaxValue = data.Where(x => x.Type == PeriodType.P4).Select(x => (decimal?)x.Value).Max() ?? 0,
				P5MaxValue = data.Where(x => x.Type == PeriodType.P5).Select(x => (decimal?)x.Value).Max() ?? 0,
				P6MaxValue = data.Where(x => x.Type == PeriodType.P6).Select(x => (decimal?)x.Value).Max() ?? 0,
				// Colors
				P1Color = data.Where(x => x.Color != null && x.Type == PeriodType.P1).Select(x => x.Color).FirstOrDefault(),
				P2Color = data.Where(x => x.Color != null && x.Type == PeriodType.P2).Select(x => x.Color).FirstOrDefault(),
				P3Color = data.Where(x => x.Color != null && x.Type == PeriodType.P3).Select(x => x.Color).FirstOrDefault(),
				P4Color = data.Where(x => x.Color != null && x.Type == PeriodType.P4).Select(x => x.Color).FirstOrDefault(),
				P5Color = data.Where(x => x.Color != null && x.Type == PeriodType.P5).Select(x => x.Color).FirstOrDefault(),
				P6Color = data.Where(x => x.Color != null && x.Type == PeriodType.P6).Select(x => x.Color).FirstOrDefault()
			};
		}
		#endregion ExecuteAsync
	}
}
