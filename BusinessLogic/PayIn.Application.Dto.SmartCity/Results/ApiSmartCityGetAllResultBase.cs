using System.Collections.Generic;
using Xp.Application.Dto;

namespace PayIn.Application.Dto.SmartCity.Results
{
	public class ApiSmartCityGetAllResultBase : ResultBase<ApiSmartCityGetAllResult>
	{
		public IEnumerable<ApiSmartCityGetAllResultBase_Sensor> Sensors { get; set; }
	}
}
