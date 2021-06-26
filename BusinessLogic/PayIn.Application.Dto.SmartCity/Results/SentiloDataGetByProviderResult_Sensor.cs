using System.Collections.Generic;

namespace PayIn.Application.Dto.SmartCity.Results
{
	public partial class SentiloDataGetByProviderResult_Sensor
	{
		public int Sensor { get; set; }

		public IEnumerable<SentiloDataGetByProviderResult_Observation> Observations { get; set; }
    }
}
