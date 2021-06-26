using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PayIn.Application.Dto.SmartCity.Results
{
	public class ApiSmartCityGetAllResultBase_Sensor
	{
		public ApiSensorGetEnergyResultBase Sensor { get; set; }
		public IEnumerable<ApiSensorGetEnergyResultBase> Subsensors { get; set; }
	}
}
