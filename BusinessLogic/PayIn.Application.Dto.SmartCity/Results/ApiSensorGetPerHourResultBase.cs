using System.Collections.Generic;
using Xp.Application.Dto;

namespace PayIn.Application.Dto.SmartCity.Results
{
	public class ApiSensorGetPerHourResultBase : ResultBase<ApiSensorGetPerHourResult>
	{
		public IEnumerable<ApiSensorGetPerHourResult> LastDay1;
		public IEnumerable<ApiSensorGetPerHourResult> LastDay2;
		public IEnumerable<ApiSensorGetPerHourResult> LastDay3;
	}
}
