using Microsoft.Practices.Unity;
using PayIn.Application.Dto.SmartCity.Arguments;
using PayIn.Domain.SmartCity;
using PayIn.Domain.SmartCity.Enums;
using System;
using System.Linq;
using System.Threading.Tasks;
using Xp.Application.Dto;
using Xp.Domain;

namespace PayIn.Application.SmartCity.Handlers
{
	public class SentiloDataUpdateDataHandler : IServiceBaseHandler<SentiloDataUpdateValueArguments>
	{
		[Dependency] public IEntityRepository<Sensor> SensorRepository { get; set; }
		[Dependency] public IEntityRepository<Data> DataRepository { get; set; }
		[Dependency] public IEntityRepository<EnergyHoliday> EnergyHolidayRepository { get; set; }
		[Dependency] public IEntityRepository<EnergyTariffPrice> EnergyTariffPriceRepository { get; set; }

		#region ExecuteAsync
		public async Task<dynamic> ExecuteAsync(SentiloDataUpdateValueArguments arguments)
		{
			var now = DateTime.UtcNow;
			
			return await CreateDataAsync(arguments.SensorCode, arguments.Value, now);
		}
		#endregion ExecuteAsync

		#region CreateValueAsync
		public async Task<Data> CreateDataAsync(string sensorCode, decimal value, DateTime timestamp, DataSet dataSet = null)
		{
			var sensor = (await SensorRepository.GetAsync("Component.Sensors"))
				.Where(x =>
					x.State == SensorState.Active &&
					x.Component.State == ComponentState.Active &&
					x.Component.Device.State == DeviceState.Active &&
					x.Code == sensorCode
				)
				.FirstOrDefault() ??
				throw new ArgumentNullException(nameof(SentiloDataUpdateSensorArguments.SensorCode));

			return await CreateDataAsync(sensor, value, timestamp, dataSet);
		}
		public async Task<Data> CreateDataAsync(Sensor sensor, decimal value, DateTime timestamp, DataSet dataSet = null)
		{
			if (sensor == null)
				return null;

			if (dataSet == null)
				dataSet = DataSet.Create(sensor.Component, timestamp);
			var data = Data.Create(dataSet, sensor, value);
			await DataRepository.AddAsync(data);

			await CalculatePriceAsync(dataSet, data);

			return data;
		}
		#endregion CreateValueAsync
		
		#region CalculatePriceAsync
		public async Task CalculatePriceAsync(DataSet dataSet, Data data)
		{
			if (
				(data.Sensor.Type == SensorType.EnergyActive) ||
				(data.Sensor.Type == SensorType.PowerActive)
			)
				await CalculatePriceEnergyAsync(dataSet, data);
		}
		#endregion CalculatePriceAsync

		#region CalculatePriceEnergyAsync
		public async Task CalculatePriceEnergyAsync(DataSet dataSet, Data data)
		{
			var localDateTime = data.Sensor.GetDateTimeToLocal(dataSet.Timestamp).Value;
			var isHoliday = (await EnergyHolidayRepository.GetAsync())
				.Any(x =>
					x.Date.Year == localDateTime.Year &&
					x.Date.Month == localDateTime.Month &&
					x.Date.Day == localDateTime.Day
				);

			var hours = dataSet.Timestamp.Hour;
			var minutes = dataSet.Timestamp.Minute;
			var date = dataSet.Timestamp;

			var weekDay =
				// Falta el holiday
				isHoliday ? WeekDays.Holiday :
				localDateTime.DayOfWeek == DayOfWeek.Monday ? WeekDays.Monday :
				localDateTime.DayOfWeek == DayOfWeek.Tuesday ? WeekDays.Tuesday :
				localDateTime.DayOfWeek == DayOfWeek.Wednesday ? WeekDays.Wednesday :
				localDateTime.DayOfWeek == DayOfWeek.Thursday ? WeekDays.Thursday :
				localDateTime.DayOfWeek == DayOfWeek.Friday ? WeekDays.Friday :
				localDateTime.DayOfWeek == DayOfWeek.Saturday ? WeekDays.Saturday :
				WeekDays.Sunday
				;

			// Search price
			var prices = (await EnergyTariffPriceRepository.GetAsync())
				.Where(x =>
					x.State == EnergyTariffPriceState.Active &&
					x.ContractId == data.Sensor.EnergyContractId &&
					x.Period.TimeTables
						.Any(y =>
							(
								(
									// Horas normal
									(
										(y.Since.Hour < y.Until.Hour) ||
										((y.Since.Hour == y.Until.Hour) && (y.Since.Minute < y.Until.Minute))
									) &&
									(
										(y.Since.Hour < hours) ||
										((y.Since.Hour == hours) && (y.Since.Minute <= minutes))
									) &&
									(
										(y.Until.Hour > hours) ||
										((y.Until.Hour == hours) && (y.Until.Minute > minutes))
									)
								) ||
								(
									// Horas iguales o cruzadas
									(
										(y.Since.Hour > y.Until.Hour) ||
										((y.Since.Hour == y.Until.Hour) && (y.Since.Minute >= y.Until.Minute))
									) &&
									(
										(
											(y.Since.Hour < hours) ||
											((y.Since.Hour == hours) && (y.Since.Minute <= minutes))
										) ||
										(
											(y.Until.Hour > hours) ||
											((y.Until.Hour == hours) && (y.Until.Minute > minutes))
										)
									)
								)
							) &&
							y.Schedule.Since <= date &&
							y.Schedule.Until >= date &&
							(y.Schedule.WeekDay & weekDay) == weekDay
						)
				)
				.ToList();
			var price = prices
				.FirstOrDefault();
			if (price == null)
				return;

			// Add price to data
			if (data.Sensor.Type == SensorType.EnergyActive)
				data.Price = price.EnergyPrice;
			data.EnergyTariffPriceId = price.Id;
		}
		#endregion CalculatePriceEnergyAsync
	}
}
