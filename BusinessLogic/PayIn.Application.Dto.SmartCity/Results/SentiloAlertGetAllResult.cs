using System.Collections.Generic;

namespace PayIn.Application.Dto.SmartCity.Results
{
    public partial class SentiloAlertGetAllResult
	{
		public IEnumerable<SentiloAlertGetAllResult_Message> Messages { get; set; }
    }
}
