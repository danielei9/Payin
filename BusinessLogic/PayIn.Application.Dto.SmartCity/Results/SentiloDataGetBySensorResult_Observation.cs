using Xp.Common;

namespace PayIn.Application.Dto.SmartCity.Results
{
	public partial class SentiloDataGetBySensorResult_Observation
	{
		public decimal Value { get; set; }
		public XpDateTime Timestamp { get; set; }
		public string Location { get; set; }
	}
}
