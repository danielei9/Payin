using System.Collections.Generic;

namespace PayIn.Application.Dto.SmartCity.Results
{
    public partial class SentiloDataGetByProviderResult
	{
		public IEnumerable<SentiloDataGetByProviderResult_Sensor> Sensors { get; set; }
    }
}
