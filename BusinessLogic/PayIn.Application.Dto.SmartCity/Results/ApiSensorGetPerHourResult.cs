using Xp.Common;

namespace PayIn.Application.Dto.SmartCity.Results
{
	public partial class ApiSensorGetPerHourResult
	{
		public int Hour { get; set; }
		public decimal? Day0 { get; set; }
		public decimal? Day1 { get; set; }
		public decimal? Day2 { get; set; }
		public decimal? Day3 { get; set; }
	}
}
