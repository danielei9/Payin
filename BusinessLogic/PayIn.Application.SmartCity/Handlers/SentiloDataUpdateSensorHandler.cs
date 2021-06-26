using Microsoft.Practices.Unity;
using PayIn.Application.Dto.SmartCity.Arguments;
using PayIn.BusinessLogic.Common;
using PayIn.Domain.SmartCity;
using PayIn.Domain.SmartCity.Enums;
using PayIn.Infrastructure.SmartCity.Repositories;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using Xp.Application.Dto;
using Xp.Domain;

namespace PayIn.Application.SmartCity.Handlers
{
	public class SentiloDataUpdateSensorHandler : IServiceBaseHandler<SentiloDataUpdateSensorArguments>
	{
		[Dependency] public IEntityRepository<Sensor> SensorRepository { get; set; }
		[Dependency] public SentiloDataUpdateDataHandler SentiloDataUpdateDataHandler { get; set; }

		#region ExecuteAsync
		public async Task<dynamic> ExecuteAsync(SentiloDataUpdateSensorArguments arguments)
		{
			var now = DateTime.UtcNow;

			var sensor = (await SensorRepository.GetAsync("Component.Sensors"))
				.Where(x =>
					x.State == SensorState.Active &&
					x.Component.State == ComponentState.Active &&
					x.Component.Device.State == DeviceState.Active &&
					x.Code == arguments.SensorCode
				)
				.FirstOrDefault() ??
				throw new ArgumentNullException(nameof(SentiloDataUpdateSensorArguments.SensorCode));

			if (
				(sensor != null) &&
				(arguments.Observations != null)
			)
			{
				var observationGroup = arguments.Observations
					.GroupBy(x => new { x.Timestamp, x.Location });
				foreach (var observations in observationGroup)
				{
					// Timestamp
					var timestamp = sensor.GetDateTimeFromSentilo(observations.Key.Timestamp) ?? now;

					// Location
					var longitude = sensor.Component.GetLongitude(observations.Key.Location);
					var latitude = sensor.Component.GetLatitude(observations.Key.Location);
					
					await CreateObservationsAsync(sensor, latitude, longitude, observations, timestamp);
				}
			}

			return null;
		}
		#endregion ExecuteAsync

		#region CreateObservationsAsync
		private Dictionary<Tuple<int, DateTime>, DataSet> cache = new Dictionary<Tuple<int, DateTime>, DataSet>();
		public async Task CreateObservationsAsync(Sensor sensor, decimal? latitude, decimal? longitude, IEnumerable<SentiloDataUpdateProviderArguments_Observation> observations, DateTime timestamp, DataSet dataSet = null)
		{
			// Dataset
			if (dataSet == null)
			{
				var dataSetKey = new Tuple<int, DateTime>(sensor.ComponentId, timestamp);
				dataSet = cache.GetOrDefault(dataSetKey);
				if (dataSet == null)
				{
					dataSet = DataSet.Create(sensor.Component, timestamp);
					cache.Add(dataSetKey, dataSet);
				}
			}

			// Longitude
			if (longitude != null)
			{
				var sensorLongitude = sensor.Component.Sensors
					.Where(x => x.Type == SensorType.Longitude)
					.FirstOrDefault();
				if (sensorLongitude != null)
					await SentiloDataUpdateDataHandler.CreateDataAsync(sensorLongitude, longitude.Value, timestamp, dataSet);
			}

			// Latitude
			if (latitude != null)
			{
				var sensorLatitude = sensor.Component.Sensors
					.Where(x => x.Type == SensorType.Latitude)
					.FirstOrDefault();

				if (sensorLatitude != null)
					await SentiloDataUpdateDataHandler.CreateDataAsync(sensorLatitude, latitude.Value, timestamp, dataSet);
			}

			// Data
			foreach (var observation in observations)
				await SentiloDataUpdateDataHandler.CreateDataAsync(sensor, observation.Value, timestamp, dataSet);
		}
		#endregion CreateObservationsAsync
	}
}
