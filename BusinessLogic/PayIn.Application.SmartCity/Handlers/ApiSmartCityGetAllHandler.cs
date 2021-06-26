using Microsoft.Practices.Unity;
using PayIn.Application.Dto.SmartCity.Arguments;
using PayIn.Application.Dto.SmartCity.Results;
using System;
using System.Linq;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using Xp.Application.Dto;

namespace PayIn.Application.SmartCity.Handlers
{
	public class ApiSmartCityGetAllHandler :
        IQueryBaseHandler<ApiSmartCityGetAllArguments, ApiSmartCityGetAllResult>
    {
        [Dependency] public ApiSensorGetEnergyHandler ApiSensorGetEnergyHandler { get; set; }

        #region ExecuteAsync
        public async Task<ResultBase<ApiSmartCityGetAllResult>> ExecuteAsync(ApiSmartCityGetAllArguments arguments)
		{
#if VILAMARXANT
			var mainSensorId = 189;
			var subsensorId = new[] {
				215, 176, // Generales
				131, 1, // Futbol
				14, 40, 66, 144, // Gimnasio
				79, 105, // Padel
				176, 27, 53, 118, 150, 163, // Bombas
				215, 202 // Pabellón
			};
#elif CARCAIXENT
			var mainSensorId = 2470;
			var subsensorId = new[] {
				2483, // Climatització
				2496 // Grada Cafeteria
			};
#else
			// FAURA
			var mainSensorId = 2418;
			var subsensorId = new[] {
				/*2418,*/ 2431, 2444, 2457, // Ajuntament
				2600, 2613, 2626, 2639, // Canaleta
				2548, 2561, 2574, 2587, // Pl. Església
				2509, 2522, 2535 // Ronda Dipu
			};
#endif
			var allSensorIds = subsensorId.Union(new[] { mainSensorId });

			var sensors = new Collection<ApiSensorGetEnergyResultBase>();
			foreach (var sensorId in allSensorIds) {
				var sensor = await ApiSensorGetEnergyHandler.ExecuteAsync(
					new ApiSensorGetEnergyArguments(ApiSensorGetEnergyArguments_Period.Last24h, DateTime.UtcNow)
					{
						Id = sensorId
					}
				) as ApiSensorGetEnergyResultBase;

				sensors.Add(sensor);
			}

			return new ApiSmartCityGetAllResultBase
			{
				Sensors = sensors
					.Where(x => x.Id == mainSensorId)
					.Select(x => new ApiSmartCityGetAllResultBase_Sensor
					{
						Sensor = x,
						Subsensors = sensors
							.Where(y => subsensorId.Contains(y.Id))
					}),
				Data = null
			};
		}
#endregion ExecuteAsync
	}
}
