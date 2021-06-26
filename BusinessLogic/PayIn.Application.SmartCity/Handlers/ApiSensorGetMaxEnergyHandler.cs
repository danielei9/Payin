using Microsoft.Practices.Unity;
using PayIn.Application.Dto.SmartCity.Arguments;
using PayIn.Application.Dto.SmartCity.Results;
using PayIn.BusinessLogic.Common;
using PayIn.Domain.SmartCity;
using PayIn.Domain.SmartCity.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xp.Application.Dto;
using Xp.Domain;

namespace PayIn.Application.SmartCity.Handlers
{
	public class ApiSensorGetMaxEnergyHandler :
        IQueryBaseHandler<ApiSensorGetMaxEnergyArguments, ApiSensorGetMaxEnergyResult>
	{
		public class ResultTemp
		{
			public PeriodType? Type { get; set; }
			public string Name { get; set; }
			public string Color { get; set; }
			public decimal? PowerContract { get; set; }

			public DateTime Timestamp { get; set; }
			public decimal Value { get; set; }
			public decimal? Price { get; set; }
		}

		[Dependency] public IEntityRepository<Sensor> Repository { get; set; }
		[Dependency] public ISessionData SessionData { get; set; }

		#region ExecuteAsync
		public async Task<ResultBase<ApiSensorGetMaxEnergyResult>> ExecuteAsync(ApiSensorGetMaxEnergyArguments arguments)
		{
			decimal? max = null;

			var endDateTime =
				(arguments.Period == ApiSensorGetMaxEnergyArguments_Period.Day || arguments.Period == ApiSensorGetMaxEnergyArguments_Period.Month) ?
					arguments.Date.Value.AddDays(1) :
					DateTime.UtcNow;
			if (arguments.Period == ApiSensorGetMaxEnergyArguments_Period.Month || arguments.Period == ApiSensorGetMaxEnergyArguments_Period.Last30d)
			{
				endDateTime = endDateTime
					.Date
					.ToUTC();
			}
			else
			{
				endDateTime = endDateTime
				.FloorHour()
				.AddMinutes(endDateTime.GetQuarter() * 15)
				.ToUTC();
			}
			var startDate = endDateTime
				.AddDays(
					(arguments.Period == ApiSensorGetMaxEnergyArguments_Period.Month || arguments.Period == ApiSensorGetMaxEnergyArguments_Period.Last30d) ?
						-30 :
						- 1
				)
				.ToUTC();

			var sensor = (await Repository.GetAsync())
				.Where(x =>
					(x.Id == arguments.Id) &&
					(x.State != SensorState.Delete) &&
					(x.Component.State != ComponentState.Delete) &&
					(x.Component.Device.State != DeviceState.Delete) &&
					(x.Component.Device.Concession.Login == SessionData.Login)
				)
				.Select(x => new
				{
					x.IsAcumulative,
					x.Unit,
					x.Component.TimeZone,
					x.Id,
					x.Name,
					ComponentName = x.Component.Name,
					DeviceName = x.Component.Device.Name,
					x.Component.Device.CO2Factor,
					Datas = x.Datas
						.Where(y =>
							//	arguments.Period == PeriodType.LastDay &&
							y.DataSet.Timestamp >= startDate &&
							y.DataSet.Timestamp <= endDateTime
						)
						.Select(y => new
						{
							Type = (PeriodType?)y.EnergyTariffPrice.Period.Type,
							y.EnergyTariffPrice.Period.Color,
							PowerContract = (x.HasMaximeter) ? // && y.EnergyTariffPrice.PowerManagement == PowerManagementType.Maximeter) ?
								y.EnergyTariffPrice.PowerContract :
								null,
							y.DataSet.Timestamp,
							y.Value,
							y.Price
						})
						.OrderBy(y => y.Timestamp)
				})
				.FirstOrDefault();
			if (sensor == null)
				return new ApiSensorGetMaxEnergyResultBase();
			var timeZoneInfo = TimeZoneInfo.FindSystemTimeZoneById(sensor.TimeZone);

			var tempList = sensor.Datas
				.Select(x => new ResultTemp
				{
					Type = x.Type,
					Color = x.Color,
					PowerContract = x.PowerContract,

					Timestamp = x.Timestamp.ToUTC(),
					Value = x.Value,
					Price = x.Price
				})
				.ToList();

			// Add cambios de precio
			// TODO: Hacer

			// Add quarters
			if (arguments.Period == ApiSensorGetMaxEnergyArguments_Period.Month || arguments.Period == ApiSensorGetMaxEnergyArguments_Period.Last30d)
			{
				for (var i = 1; i <= 31; i++)
				{
					var date = endDateTime.AddDays(-1 * i);
					AddIntermediateValue(tempList, new DateTime(date.Year, date.Month, date.Day, 0, 0, 0));
				}
			}
			else
			{
				for (var i = 0; i <= 25; i++)
				{
					var hour = (endDateTime.Hour - i + 48) % 24;
					AddIntermediateValue(tempList, new DateTime(endDateTime.Year, endDateTime.Month, endDateTime.Day, hour, 0, 0));
					AddIntermediateValue(tempList, new DateTime(endDateTime.Year, endDateTime.Month, endDateTime.Day, hour, 15, 0));
					AddIntermediateValue(tempList, new DateTime(endDateTime.Year, endDateTime.Month, endDateTime.Day, hour, 30, 0));
					AddIntermediateValue(tempList, new DateTime(endDateTime.Year, endDateTime.Month, endDateTime.Day, hour, 45, 0));
				}
			}

			// Order 
			tempList = tempList
				.OrderBy(x => x.Timestamp)
				.ToList();

			// Calculate acumulative y calcular cost
			if (sensor.IsAcumulative && tempList.Any())
			{
				decimal previousValue = tempList
					.FirstOrDefault()
					.Value;
				tempList = tempList
					.Skip(1)
					.ToList();

				foreach (var tempItem in tempList)
				{
					var oldValue = tempItem.Value;
					tempItem.Value -= previousValue;
					previousValue = oldValue;
				}
			}

			// Sum values
			var resultPerQuarter2 = tempList
				.Select(x => new
				{
					x.Type,
					x.Color,
					x.PowerContract,
					Timestamp = TimeZoneInfo.ConvertTimeFromUtc(
						x.Timestamp.AddSeconds(-1),
						timeZoneInfo
					),
					x.Value,
					x.Price
				})
				.ToList();
			var resultPerQuarter3 = resultPerQuarter2
				.Select(x => new
				{
					x.Type,
					x.Color,
					x.PowerContract,

					x.Timestamp, // BORRAR
					x.Timestamp
						.Date,
					x.Timestamp
						.Hour,
					Quarter = x.Timestamp
						.GetQuarter(),
					x.Value,
					x.Price
				})
				.ToList();
			var resultPerQuarter4 = resultPerQuarter3
				// Agrupar por cuarto de hora
				.GroupBy(x => new
				{
					x.Type,
					x.Color,
					x.PowerContract,

					x.Timestamp, // BORRAR
					x.Date,
					x.Hour,
					x.Quarter
				})
				.Select(x => new
				{
					x.Key.Type,
					x.Key.Color,
					x.Key.PowerContract,

					x.Key.Timestamp, // BORRAR
					x.Key.Date,
					x.Key.Hour,
					x.Key.Quarter,
					Price = x.Max(z => z.Price),
					Consum = x.Sum(z => z.Value)
				})
				.ToList();
			var resultPerQuarter = resultPerQuarter4
				// El calculo de ahorros y excesos se ha de hacer ya dividido por cuartos
				.Select(x => new
				{
					x.Type,
					x.Color,
					x.PowerContract,

					x.Timestamp, // BORRAR
					x.Date,
					x.Hour,
					x.Quarter,
					x.Consum,
					x.Price,
					Consumption = x.Consum,
					ConsumptionSaving = 0,
						//max == null ? 0 :
						//x.Consum < max ? max.Value - x.Consum :
						//0,
					ConsumptionOver = 0,
						//max == null ? 0 :
						//x.Consum > max ? x.Consum - max.Value :
						//0,
					//PowerLack =
					//	x.PowerContract == null ? 0 :
					//	x.Consum < x.PowerContract * 0.85m / 4 ?
					//		x.PowerContract * 0.85m / 4 - x.Consum :
					//		0,
					//PowerExcess =
					//	x.PowerContract == null ? 0 :
					//	x.Consum > x.PowerContract * 1.05m / 4 ?
					//		2 * (x.Consum - x.PowerContract * 1.05m / 4) :
					//		0,
				})
				.ToList();

			// Separe consumptions, costs and add penalty
			var result2 = resultPerQuarter
				.GroupBy(x => new
				{
					x.Date,
					x.Hour,
					x.Quarter
				})
				.OrderBy(x => x.Key.Date)
				.ThenBy(x => x.Key.Hour)
				.ThenBy(x => x.Key.Quarter)
				.Select(x => new
				{
					Label =
						x.Key.Quarter == 0 ? x.Key.Hour + ":" + "15" :
						x.Key.Quarter == 1 ? x.Key.Hour + ":" + "30" :
						x.Key.Quarter == 2 ? x.Key.Hour + ":" + "45" :
						(x.Key.Hour + 1) + ":" + "00",
					P1 = x.Where(y => y.Type == PeriodType.P1),
					P2 = x.Where(y => y.Type == PeriodType.P2),
					P3 = x.Where(y => y.Type == PeriodType.P3),
					P4 = x.Where(y => y.Type == PeriodType.P4),
					P5 = x.Where(y => y.Type == PeriodType.P5),
					P6 = x.Where(y => y.Type == PeriodType.P6),
					// Over consumption
					ConsumptionOver = x.Sum(y => y.ConsumptionOver),
					ConsumptionOverCost = x.Sum(y => y.ConsumptionOver * y.Price),
					ConsumptionOverCO2 = x.Sum(y => y.ConsumptionOver * sensor.CO2Factor),
					// Default power
					//PowerLack = x.Sum(y => y.PowerLack) ?? 0,
					//PowerLackCost = x.Sum(y => y.PowerLack * y.Price) ?? 0,
					// Default power
					//PowerExcess = x.Sum(y => y.PowerExcess) ?? 0,
					//PowerExcessCost = x.Sum(y => y.PowerExcess * y.Price) ?? 0,
					// PowerContract (mostrar lineas de potencia contratada)
					PowerContract = x.Average(y => (decimal?)y.PowerContract) / 4,
					PowerContractCost = x.Average(y => (decimal?)y.PowerContract * y.Price) / 4,
				});
			var result = result2
				.Select(x => new ApiSensorGetMaxEnergyResult
				{
					Label = x.Label,
					// P1
					P1Consumption = x.P1.Sum(y => y.Consumption),
					P1ConsumptionCost = x.P1.Sum(y => y.Consumption * y.Price),
					P1ConsumptionCO2 = x.P1.Sum(y => y.Consumption * sensor.CO2Factor),
					P1ConsumptionSaving = x.P1.Sum(y => y.ConsumptionSaving),
					P1ConsumptionSavingCost = x.P1.Sum(y => y.ConsumptionSaving* y.Price),
					P1ConsumptionSavingCO2 = x.P1.Sum(y => y.ConsumptionSaving * sensor.CO2Factor),
					P1Color = x.P1.Select(y => y.Color).FirstOrDefault() ?? "",
					// P2
					P2Consumption = x.P2.Sum(y => y.Consumption),
					P2ConsumptionCost = x.P2.Sum(y => y.Consumption * y.Price),
					P2ConsumptionCO2 = x.P2.Sum(y => y.Consumption * sensor.CO2Factor),
					P2ConsumptionSaving = x.P2.Sum(y => y.ConsumptionSaving),
					P2ConsumptionSavingCost = x.P2.Sum(y => y.ConsumptionSaving * y.Price),
					P2ConsumptionSavingCO2 = x.P2.Sum(y => y.ConsumptionSaving * sensor.CO2Factor),
					P2Color = x.P2.Select(y => y.Color).FirstOrDefault() ?? "",
					// P3
					P3Consumption = x.P3.Sum(y => y.Consumption),
					P3ConsumptionCost = x.P3.Sum(y => y.Consumption * y.Price),
					P3ConsumptionCO2 = x.P3.Sum(y => y.Consumption * sensor.CO2Factor),
					P3ConsumptionSaving = x.P3.Sum(y => y.ConsumptionSaving),
					P3ConsumptionSavingCost = x.P3.Sum(y => y.ConsumptionSaving * y.Price),
					P3ConsumptionSavingCO2 = x.P3.Sum(y => y.ConsumptionSaving * sensor.CO2Factor),
					P3Color = x.P3.Select(y => y.Color).FirstOrDefault() ?? "",
					// P4
					P4Consumption = x.P4.Sum(y => y.Consumption),
					P4ConsumptionCost = x.P4.Sum(y => y.Consumption * y.Price),
					P4ConsumptionCO2 = x.P4.Sum(y => y.Consumption * sensor.CO2Factor),
					P4ConsumptionSaving = x.P4.Sum(y => y.ConsumptionSaving),
					P4ConsumptionSavingCost = x.P4.Sum(y => y.ConsumptionSaving * y.Price),
					P4ConsumptionSavingCO2 = x.P4.Sum(y => y.ConsumptionSaving * sensor.CO2Factor),
					P4Color = x.P4.Select(y => y.Color).FirstOrDefault() ?? "",
					// P5
					P5Consumption = x.P5.Sum(y => y.Consumption),
					P5ConsumptionCost = x.P5.Sum(y => y.Consumption * y.Price),
					P5ConsumptionCO2 = x.P5.Sum(y => y.Consumption * sensor.CO2Factor),
					P5ConsumptionSaving = x.P5.Sum(y => y.ConsumptionSaving),
					P5ConsumptionSavingCost = x.P5.Sum(y => y.ConsumptionSaving * y.Price),
					P5ConsumptionSavingCO2 = x.P5.Sum(y => y.ConsumptionSaving * sensor.CO2Factor),
					P5Color = x.P5.Select(y => y.Color).FirstOrDefault() ?? "",
					// P6
					P6Consumption = x.P6.Sum(y => y.Consumption),
					P6ConsumptionCost = x.P6.Sum(y => y.Consumption * y.Price),
					P6ConsumptionCO2 = x.P6.Sum(y => y.Consumption * sensor.CO2Factor),
					P6ConsumptionSaving = x.P6.Sum(y => y.ConsumptionSaving),
					P6ConsumptionSavingCost = x.P6.Sum(y => y.ConsumptionSaving * y.Price),
					P6ConsumptionSavingCO2 = x.P6.Sum(y => y.ConsumptionSaving * sensor.CO2Factor),
					P6Color = x.P6.Select(y => y.Color).FirstOrDefault() ?? "",
					// Over consmuption
					ConsumptionOver = x.ConsumptionOver,
					ConsumptionOverCost = x.ConsumptionOverCost,
					ConsumptionOverCO2 = x.ConsumptionOverCO2,
					// Default power
					//PowerLack = x.PowerLack,
					//PowerLackCost = x.PowerLackCost,
					//P1PowerLack = x.P1.Sum(y => y.PowerLack) ?? 0,
					//P1PowerLackCost = x.P1.Sum(y => y.PowerLack * y.Price) ?? 0,
					//P2PowerLack = x.P2.Sum(y => y.PowerLack) ?? 0,
					//P2PowerLackCost = x.P2.Sum(y => y.PowerLack * y.Price) ?? 0,
					//P3PowerLack = x.P3.Sum(y => y.PowerLack) ?? 0,
					//P3PowerLackCost = x.P3.Sum(y => y.PowerLack * y.Price) ?? 0,
					//P4PowerLack = x.P4.Sum(y => y.PowerLack) ?? 0,
					//P4PowerLackCost = x.P4.Sum(y => y.PowerLack * y.Price) ?? 0,
					//P5PowerLack = x.P5.Sum(y => y.PowerLack) ?? 0,
					//P5PowerLackCost = x.P5.Sum(y => y.PowerLack * y.Price) ?? 0,
					//P6PowerLack = x.P6.Sum(y => y.PowerLack) ?? 0,
					//P6PowerLackCost = x.P6.Sum(y => y.PowerLack * y.Price) ?? 0,
					// Excess power
					//PowerExcess = x.PowerExcess,
					//PowerExcessCost = x.PowerExcessCost,
					//P1PowerExcess = x.P1.Sum(y => y.PowerExcess) ?? 0,
					//P1PowerExcessCost = x.P1.Sum(y => y.PowerExcess * y.Price) ?? 0,
					//P2PowerExcess = x.P2.Sum(y => y.PowerExcess) ?? 0,
					//P2PowerExcessCost = x.P2.Sum(y => y.PowerExcess * y.Price) ?? 0,
					//P3PowerExcess = x.P3.Sum(y => y.PowerExcess) ?? 0,
					//P3PowerExcessCost = x.P3.Sum(y => y.PowerExcess * y.Price) ?? 0,
					//P4PowerExcess = x.P4.Sum(y => y.PowerExcess) ?? 0,
					//P4PowerExcessCost = x.P4.Sum(y => y.PowerExcess * y.Price) ?? 0,
					//P5PowerExcess = x.P5.Sum(y => y.PowerExcess) ?? 0,
					//P5PowerExcessCost = x.P5.Sum(y => y.PowerExcess * y.Price) ?? 0,
					//P6PowerExcess = x.P6.Sum(y => y.PowerExcess) ?? 0,
					//P6PowerExcessCost = x.P6.Sum(y => y.PowerExcess * y.Price) ?? 0,
					// PowerContract (mostrar lineas de potencia contratada)
					PowerContract = x.PowerContract,
					PowerContractCost = x.PowerContractCost,
					PowerContractMin = x.PowerContract * 0.85m,
					PowerContractMinCost = x.PowerContractCost * 0.85m,
					PowerContractMax = x.PowerContract * 1.05m,
					PowerContractMaxCost = x.PowerContractCost * 1.05m,
					// Color
					ColorOver = "black"
				});

			var p1Color = result2
				.Where(x => x.P1.Count() > 0)
				.SelectMany(x => x.P1
					.Select(y => y.Color)
				)
				.FirstOrDefault();
			var p2Color = result2
				.Where(x => x.P2.Count() > 0)
				.SelectMany(x => x.P2
					.Select(y => y.Color)
				)
				.FirstOrDefault();
			var p3Color = result2
				.Where(x => x.P3.Count() > 0)
				.SelectMany(x => x.P3
					.Select(y => y.Color)
				)
				.FirstOrDefault();
			var p4Color = result2
				.Where(x => x.P4.Count() > 0)
				.SelectMany(x => x.P4
					.Select(y => y.Color)
				)
				.FirstOrDefault();
			var p5Color = result2
				.Where(x => x.P5.Count() > 0)
				.SelectMany(x => x.P5
					.Select(y => y.Color)
				)
				.FirstOrDefault();
			var p6Color = result2
				.Where(x => x.P6.Count() > 0)
				.SelectMany(x => x.P6
					.Select(y => y.Color)
				)
				.FirstOrDefault();

			return new ApiSensorGetMaxEnergyResultBase
			{
				Data = result,
				Unit = sensor.Unit ?? "",
				Id = sensor.Id,
				Name = sensor.Name,
				ComponentName = sensor.ComponentName,
				DeviceName = sensor.DeviceName,
				Period = arguments.Period,
				PeriodName = arguments.Period.ToEnumAlias(),
				// P1Consumption
				Consumption = result.Sum(x => x.P1Consumption + x.P2Consumption + x.P3Consumption + x.P4Consumption + x.P5Consumption + x.P6Consumption + x.ConsumptionOver),
				ConsumptionCost = result.Sum(x => x.P1ConsumptionCost + x.P2ConsumptionCost + x.P3ConsumptionCost + x.P4ConsumptionCost + x.P5ConsumptionCost + x.P6ConsumptionCost + x.ConsumptionOverCost),
				ConsumptionCO2 = result.Sum(x => x.P1ConsumptionCO2 + x.P2ConsumptionCO2 + x.P3ConsumptionCO2 + x.P4ConsumptionCO2 + x.P5ConsumptionCO2 + x.P6ConsumptionCO2 + x.ConsumptionOverCO2),
				P1Consumption = result.Sum(x => x.P1Consumption),
				P1ConsumptionCost = result.Sum(x => x.P1ConsumptionCost),
				P2Consumption = result.Sum(x => x.P2Consumption),
				P2ConsumptionCost = result.Sum(x => x.P2ConsumptionCost),
				P3Consumption = result.Sum(x => x.P3Consumption),
				P3ConsumptionCost = result.Sum(x => x.P3ConsumptionCost),
				P4Consumption = result.Sum(x => x.P4Consumption),
				P4ConsumptionCost = result.Sum(x => x.P4ConsumptionCost),
				P5Consumption = result.Sum(x => x.P5Consumption),
				P5ConsumptionCost = result.Sum(x => x.P5ConsumptionCost),
				P6Consumption = result.Sum(x => x.P6Consumption),
				P6ConsumptionCost = result.Sum(x => x.P6ConsumptionCost),
				// PowerLack
				PowerLack = result.Sum(x => x.P1PowerLack + x.P2PowerLack + x.P3PowerLack + x.P4PowerLack + x.P5PowerLack + x.P6PowerLack),
				PowerLackCost = result.Sum(x => x.P1PowerLackCost + x.P2PowerLackCost + x.P3PowerLackCost + x.P4PowerLackCost + x.P5PowerLackCost + x.P6PowerLackCost),
				P1PowerLack = result.Sum(x => x.P1PowerLack),
				P1PowerLackCost = result.Sum(x => x.P1PowerLackCost),
				P2PowerLack = result.Sum(x => x.P1PowerLack),
				P2PowerLackCost = result.Sum(x => x.P2PowerLackCost),
				P3PowerLack = result.Sum(x => x.P1PowerLack),
				P3PowerLackCost = result.Sum(x => x.P3PowerLackCost),
				P4PowerLack = result.Sum(x => x.P1PowerLack),
				P4PowerLackCost = result.Sum(x => x.P4PowerLackCost),
				P5PowerLack = result.Sum(x => x.P1PowerLack),
				P5PowerLackCost = result.Sum(x => x.P5PowerLackCost),
				P6PowerLack = result.Sum(x => x.P1PowerLack),
				P6PowerLackCost = result.Sum(x => x.P6PowerLack),
				// PowerLack
				PowerExcess = result.Sum(x => x.P1PowerExcess + x.P2PowerExcess + x.P3PowerExcess + x.P4PowerExcess + x.P5PowerExcess + x.P6PowerExcess),
				PowerExcessCost = result.Sum(x => x.P1PowerExcessCost + x.P2PowerExcessCost + x.P3PowerExcessCost + x.P4PowerExcessCost + x.P5PowerExcessCost + x.P6PowerExcessCost),
				P1PowerExcess = result.Sum(x => x.P1PowerExcess),
				P1PowerExcessCost = result.Sum(x => x.P1PowerExcessCost),
				P2PowerExcess = result.Sum(x => x.P1PowerExcess),
				P2PowerExcessCost = result.Sum(x => x.P2PowerExcessCost),
				P3PowerExcess = result.Sum(x => x.P1PowerExcess),
				P3PowerExcessCost = result.Sum(x => x.P3PowerExcessCost),
				P4PowerExcess = result.Sum(x => x.P1PowerExcess),
				P4PowerExcessCost = result.Sum(x => x.P4PowerExcessCost),
				P5PowerExcess = result.Sum(x => x.P1PowerExcess),
				P5PowerExcessCost = result.Sum(x => x.P5PowerExcessCost),
				P6PowerExcess = result.Sum(x => x.P1PowerExcess),
				P6PowerExcessCost = result.Sum(x => x.P6PowerExcess),
				// Color
				P1Color = p1Color,
				P2Color = p2Color,
				P3Color = p3Color,
				P4Color = p4Color,
				P5Color = p5Color,
				P6Color = p6Color
			};
		}
		#endregion ExecuteAsync

		#region AddIntermediateValue
		public void AddIntermediateValue(List<ResultTemp> tempList, DateTime datetime)
		{
			if (tempList.Any(x => x.Timestamp == datetime))
				return;
			
			var previous = tempList
				.Where(x =>
					x.Timestamp == tempList
						.Where(y => y.Timestamp < datetime)
						.Max(y => (DateTime?)y.Timestamp)
				)
				.FirstOrDefault();
			var next = tempList
				.Where(x =>
					x.Timestamp == tempList
						.Where(y => y.Timestamp > datetime)
						.Min(y => (DateTime?)y.Timestamp)
				)
				.FirstOrDefault();

			if ((previous != null) && (next != null))
			{
				var minutesFactor = (decimal)(
					(datetime - previous.Timestamp).TotalMinutes /
					(next.Timestamp - previous.Timestamp).TotalMinutes
				);

				var value = previous.Value + (next.Value - previous.Value) * minutesFactor;
				tempList.Add(new ResultTemp
				{
					Type = next.Type,
					Color = next.Color,
					PowerContract = next.PowerContract,

					Timestamp = datetime,
					Value = value,
					Price = next.Price // El precio siempre es el del periodo de después pq se supone que desde el inicio al fin el precio es el final
				});
			}
		}
		#endregion AddIntermediateValue
	}
}
