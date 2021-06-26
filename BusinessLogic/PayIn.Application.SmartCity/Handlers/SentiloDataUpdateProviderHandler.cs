using Microsoft.Practices.Unity;
using Microsoft.Practices.Unity.Utility;
using PayIn.Application.Dto.SmartCity.Arguments;
using PayIn.BusinessLogic.Common;
using PayIn.Domain.SmartCity;
using PayIn.Domain.SmartCity.Enums;
using PayIn.Infrastructure.SmartCity;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using Xp.Application.Dto;
using Xp.Domain;

namespace PayIn.Application.SmartCity.Handlers
{
	public class SentiloDataUpdateProviderHandler : IServiceBaseHandler<SentiloDataUpdateProviderArguments>
	{
		public class SensorInfo
		{
			public string Name { get; set; }
			public string Unit { get; set; }
			public bool IsAcumulative { get; set; }
			public ComponentType ComponentType { get; set; }
			public SensorType Type { get; set; }
		}

		[Dependency] public SentiloDataUpdateDataHandler SentiloDataUpdateDataHandler { get; set; }
		[Dependency] public IEntityRepository<Data> DataRepository { get; set; }
		[Dependency] public IEntityRepository<Device> DeviceRepository { get; set; }
		[Dependency] public IEntityRepository<Component> ComponentRepository { get; set; }
		[Dependency] public IEntityRepository<Sensor> SensorRepository { get; set; }
		[Dependency] public SmartCityUnitOrWork SmartCityUnitOfWork { get; set; }
		[Dependency] public IUnitOfWork UnitOfWork { get; set; }

		#region ExecuteAsync
		public async Task<dynamic> ExecuteAsync(SentiloDataUpdateProviderArguments arguments)
		{
			var now = DateTime.UtcNow;
			var cache = new Dictionary<Tuple<int, DateTime>, DataSet>();

			if (arguments.Sensors != null)
			{
				var sensorCodes = arguments.Sensors
					.Select(x => x.Sensor)
					.ToList();
				
				foreach (var item in arguments.Sensors)
				{
					var deviceCode = item.Sensor.Left(item.Sensor.Length - 3);
					var componentCode = item.Sensor.Left(item.Sensor.Length - 3);

					var sensor = await GetOrCreateSensorAsync(arguments.ProviderCode, deviceCode, componentCode, item.Sensor, sensorCodes);

					foreach (var observation in item.Observations)
					{
						var timestamp = sensor.GetDateTimeFromSentilo(observation.Timestamp) ?? now;
						//var locations = (observation.Location ?? item.Location ?? "").SplitString(" ");

						// Dataset
						var dataSetKey = new Tuple<int, DateTime>(sensor.Component.Id, timestamp);
						var dataSet = cache.GetOrDefault(dataSetKey);
						if (dataSet == null)
						{
							dataSet = DataSet.Create(sensor.Component, timestamp);
							cache.Add(dataSetKey, dataSet);
						}

						// Data
						var data = Data.Create(dataSet, sensor, observation.Value);
						await DataRepository.AddAsync(data);

						// Sensor
						if ((sensor.LastTimestamp == null) || (timestamp >= sensor.LastTimestamp))
						{
							sensor.LastTimestamp = timestamp;
							sensor.LastValue = observation.Value;
						}
						
						await SentiloDataUpdateDataHandler.CalculatePriceAsync(dataSet, data);
					}
				}
			}

			return null;
		}
		#endregion ExecuteAsync

		#region GetOrCreateSensorAsync
		private IEnumerable<Sensor> Sensors { get; set; }
		public async Task<Sensor> GetOrCreateSensorAsync(string providerCode, string deviceName, string componentCode, string sensorCode, IEnumerable<String> sensorCodes)
		{
			if (Sensors == null)
			{
				Sensors = (await SensorRepository.GetAsync(
					nameof(Sensor.Component) + "." + nameof(Component.Sensors),
					nameof(Sensor.Component) + "." + nameof(Component.Device)
				))
				.Where(x =>
					x.State == SensorState.Active &&
					x.Component.State == ComponentState.Active &&
					x.Component.Device.State == DeviceState.Active &&
					x.Component.Device.ProviderCode == providerCode &&
					sensorCodes.Contains(x.Code)
				)
				.ToList();
			}

			var sensor = Sensors
				.Where(x => x.Code == sensorCode)
				.FirstOrDefault();
			if (sensor != null)
				return sensor;

			var info = GetSensorInfo(providerCode, sensorCode);

			var component = await GetOrCreateComponentAsync(providerCode, deviceName, componentCode, info.ComponentType);
			sensor = new Sensor()
			{
				Component = component,
				Code = sensorCode,
				EnergyContractId = null,
				IsAcumulative = info.IsAcumulative,
				Name = info.Name,
				Type = info.Type,
				Unit = info.Unit,
				State = SensorState.Active
			};
			await SensorRepository.AddAsync(sensor);
			component.Sensors.Add(sensor);
			await UnitOfWork.SaveAsync();

			return sensor;
		}
		#endregion GetOrCreateSensorAsync

		#region GetSensorInfo
		public SensorInfo GetSensorInfo(string providerCode, string sensorCode)
		{
			if (providerCode.EndsWith("@payin"))
			{
				var sensorInfo = GetSensorInfoDival(sensorCode);
				if (sensorInfo != null)
					return sensorInfo;
			}

			return new SensorInfo { Name = sensorCode, Unit = "", IsAcumulative = false, Type = SensorType.Other, ComponentType = ComponentType.Other };
		}
		#endregion GetSensorInfo

		#region GetSensorInfoDival
		public SensorInfo GetSensorInfoDival(string sensorCode)
		{
			var code = sensorCode.Right(2);
			if (sensorCode.StartsWith("ENERGY_METER"))
			{
				// Energy
				if (code == "01") return new SensorInfo { Name = "Energía Activa",   Unit = "kWh", IsAcumulative = true, Type = SensorType.EnergyActive };
				if (code == "02") return new SensorInfo { Name = "Energía Reactiva", Unit = "kVArh", IsAcumulative = false, Type = SensorType.EnergyReactive };
				if (code == "03") return new SensorInfo { Name = "Energía Aparente", Unit = "kVAh", IsAcumulative = false, Type = SensorType.EnergyAparent };
				// Active Power
				if (code == "04") return new SensorInfo { Name = "Potencia Activa",   Unit = "kW", IsAcumulative = false, Type = SensorType.PowerActive };
				if (code == "52") return new SensorInfo { Name = "Potencia Activa R", Unit = "kW", IsAcumulative = false, Type = SensorType.PowerActiveR };
				if (code == "55") return new SensorInfo { Name = "Potencia Activa S", Unit = "kW", IsAcumulative = false, Type = SensorType.PowerActiveS };
				if (code == "56") return new SensorInfo { Name = "Potencia Activa T", Unit = "kW", IsAcumulative = false, Type = SensorType.PowerActiveT };
				// Reactive Power
				if (code == "05") return new SensorInfo { Name = "Potencia Reactiva",   Unit = "kVAr", IsAcumulative = false, Type = SensorType.PowerReactive };
				if (code == "61") return new SensorInfo { Name = "Potencia Reactiva R", Unit = "kVAr", IsAcumulative = false, Type = SensorType.PowerReactiveR };
				if (code == "64") return new SensorInfo { Name = "Potencia Reactiva S", Unit = "kVAr", IsAcumulative = false, Type = SensorType.PowerReactiveS };
				if (code == "67") return new SensorInfo { Name = "Potencia Reactiva T", Unit = "kVAr", IsAcumulative = false, Type = SensorType.PowerReactiveT };
				// Aparent Power
				if (code == "06") return new SensorInfo { Name = "Potencia Aparente", Unit = "kVA", IsAcumulative = false, Type = SensorType.PowerAparent };
				// Intensidad
				if (code == "08") return new SensorInfo { Name = "Intensidad",        Unit = "A", IsAcumulative = false, Type = SensorType.Current };
				if (code == "30") return new SensorInfo { Name = "Intensidad fase R", Unit = "A", IsAcumulative = false, Type = SensorType.CurrentR };
				if (code == "31") return new SensorInfo { Name = "Intensidad fase S", Unit = "A", IsAcumulative = false, Type = SensorType.CurrentS };
				if (code == "32") return new SensorInfo { Name = "Intensidad fase T", Unit = "A", IsAcumulative = false, Type = SensorType.CurrentT };
				if (code == "79") return new SensorInfo { Name = "Intensidad fase R", Unit = "A", IsAcumulative = false, Type = SensorType.CurrentR };
				if (code == "82") return new SensorInfo { Name = "Intensidad fase S", Unit = "A", IsAcumulative = false, Type = SensorType.CurrentS };
				if (code == "85") return new SensorInfo { Name = "Intensidad fase T", Unit = "A", IsAcumulative = false, Type = SensorType.CurrentT };
				// Tensión
				if (code == "09") return new SensorInfo { Name = "Tensión",        Unit = "V", IsAcumulative = false, Type = SensorType.Voltage };
				if (code == "33") return new SensorInfo { Name = "Tensión fase R", Unit = "V", IsAcumulative = false, Type = SensorType.VoltageR };
				if (code == "34") return new SensorInfo { Name = "Tensión fase S", Unit = "V", IsAcumulative = false, Type = SensorType.VoltageS };
				if (code == "35") return new SensorInfo { Name = "Tensión fase T", Unit = "V", IsAcumulative = false, Type = SensorType.VoltageT };
				if (code == "70") return new SensorInfo { Name = "Tensión fase R", Unit = "V", IsAcumulative = false, Type = SensorType.VoltageR };
				if (code == "73") return new SensorInfo { Name = "Tensión fase S", Unit = "V", IsAcumulative = false, Type = SensorType.VoltageS };
				if (code == "76") return new SensorInfo { Name = "Tensión fase T", Unit = "V", IsAcumulative = false, Type = SensorType.VoltageT };
				// Coseno de Phi
				if (code == "10") return new SensorInfo { Name = "Coseno de Phi 1F", Unit = "", IsAcumulative = false, Type = SensorType.CosinePhi };
				if (code == "40") return new SensorInfo { Name = "Coseno de Phi 3F", Unit = "", IsAcumulative = false, Type = SensorType.CosinePhi };
				if (code == "88") return new SensorInfo { Name = "Coseno de Phi R",  Unit = "", IsAcumulative = false, Type = SensorType.CosinePhiR };
				if (code == "91") return new SensorInfo { Name = "Coseno de Phi S",  Unit = "", IsAcumulative = false, Type = SensorType.CosinePhiS };
				if (code == "94") return new SensorInfo { Name = "Coseno de Phi T",  Unit = "", IsAcumulative = false, Type = SensorType.CosinePhiT };
			}
			else if (
				(sensorCode.StartsWith("AIR_QUALITY")) ||
				(sensorCode.StartsWith("POOL")) ||
				(sensorCode.StartsWith("WATER_HEATER")) ||
				(sensorCode.StartsWith("WATER_QUALITY")) ||
				(sensorCode.StartsWith("PUMP"))
			)
			{
				if (code == "01") return new SensorInfo { Name = "Temperatura",         Unit = "ºC", IsAcumulative = false,  Type = SensorType.AirTemperature };
				if (code == "02") return new SensorInfo { Name = "Humedad",             Unit = "%", IsAcumulative = false,   Type = SensorType.AirHumidity };
				if (code == "03") return new SensorInfo { Name = "Temperatura",         Unit = "ºC", IsAcumulative = false,  Type = SensorType.WaterTemperature };
				if (code == "04") return new SensorInfo { Name = "pH",                  Unit = "",   IsAcumulative = false,  Type = SensorType.Ph };
				if (code == "05") return new SensorInfo { Name = "Cloro",               Unit = "ppm", IsAcumulative = false, Type = SensorType.Chloride };
				if (code == "06") return new SensorInfo { Name = "Consigna",            Unit = "ºC", IsAcumulative = false,  Type = SensorType.ConsignedTemperature };
				if (code == "07") return new SensorInfo { Name = "Válvula",             Unit = "%", IsAcumulative = false,   Type = SensorType.Valve };
				if (code == "08") return new SensorInfo { Name = "Temperatura entrada", Unit = "ºC", IsAcumulative = false,  Type = SensorType.WaterTemperatureEntry };
			}

			return new SensorInfo { Name = sensorCode, Unit = "", IsAcumulative = false, Type = SensorType.Other, ComponentType = ComponentType.Other };
		}
		#endregion GetSensorInfoDival

		#region GetOrCreateComponentAsync
		public async Task<Component> GetOrCreateComponentAsync(string providerCode, string deviceName, string componentCode, ComponentType type)
		{
			var component = (await ComponentRepository.GetAsync(
					nameof(Component.Sensors),
					nameof(Component.Device)
				))
				.Where(x =>
					x.State == ComponentState.Active &&
					x.Device.State == DeviceState.Active &&
					x.Device.ProviderCode == providerCode &&
					x.Code == componentCode
				)
				.FirstOrDefault();
			if (component != null)
				return component;

			var device = await CreateDeviceAsync(providerCode, deviceName);
			component = new Component()
			{
				Device = device,
				Code = componentCode,
				Latitude = 0,
				Longitude = 0,
				Model = "",
				Name = componentCode,
				ProviderName = "",
				State = ComponentState.Active,
				TimeZone = "Romance Standard Time",
				Type = type
			};
			await ComponentRepository.AddAsync(component);
			device.Components.Add(component);
			await UnitOfWork.SaveAsync();

			return component;
		}
		#endregion GetOrCreateComponentAsync

		#region CreateDeviceAsync
		public async Task<Device> CreateDeviceAsync(string providerCode, string deviceName)
		{
			var device = new Device()
			{
				State = DeviceState.Active,
				CO2Factor = 0,
				Model = "",
				Name = deviceName,
				ProviderCode = providerCode,
				ProviderName = ""
			};
			await DeviceRepository.AddAsync(device);
			await UnitOfWork.SaveAsync();

			return device;
		}
		#endregion CreateDeviceAsync
	}
}
