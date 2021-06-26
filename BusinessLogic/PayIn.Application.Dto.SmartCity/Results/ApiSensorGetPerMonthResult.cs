using Xp.Common;

namespace PayIn.Application.Dto.SmartCity.Results
{
	public partial class ApiSensorGetPerMonthResult
	{
		public XpDateTime Timestamp { get; set; }
		public decimal? Min { get; set; }
		public decimal? Max { get; set; }
		public decimal? Avg { get; set; }
	}
}
