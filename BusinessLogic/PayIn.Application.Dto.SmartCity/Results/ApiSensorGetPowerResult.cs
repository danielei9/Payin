using Xp.Common;

namespace PayIn.Application.Dto.SmartCity.Results
{
	public partial class ApiSensorGetPowerResult
	{
		public XpDateTime Timestamp { get; set; }
		// Values
		public decimal? P1Value { get; set; }
		public decimal? P2Value { get; set; }
		public decimal? P3Value { get; set; }
		public decimal? P4Value { get; set; }
		public decimal? P5Value { get; set; }
		public decimal? P6Value { get; set; }
		// PowerContracts
		public decimal? PowerContract { get; set; }
		public decimal? PowerContractMin { get; set; }
		public decimal? PowerContractMax { get; set; }
	}
}
