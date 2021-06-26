using System.Collections.Generic;

namespace PayIn.Application.Dto.SmartCity.Results
{
    public partial class SentiloDataGetBySensorResult
	{
		public IEnumerable<SentiloDataGetBySensorResult_Observation> Observations { get; set; }
    }
}
