using Microsoft.Practices.Unity;
using PayIn.Application.Dto.SmartCity.Arguments;
using PayIn.Domain.SmartCity;
using System.Threading.Tasks;
using System.Linq;
using Xp.Application.Dto;
using Xp.Domain;
using System.Linq;
using System;

namespace PayIn.Application.Bus.Handlers
{
	public class ApiSmartCitySetTargetValueHandler : IServiceBaseHandler<ApiSmartCitySetTargetValueArguments>
	{
        [Dependency] public IEntityRepository<Sensor> SensorRepository { get; set; }

        #region ExecuteAsync
        public async Task<dynamic> ExecuteAsync(ApiSmartCitySetTargetValueArguments arguments)
		{
            var sensor = (await SensorRepository.GetAsync(arguments.Id));
            if (sensor == null)
            	throw new ApplicationException("Sensor no encontrado");

            sensor.TargetValue = arguments.TargetValue;

            return sensor;
		}
		#endregion ExecuteAsync
	}
}
