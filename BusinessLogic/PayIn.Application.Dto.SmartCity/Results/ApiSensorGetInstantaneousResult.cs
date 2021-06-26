using Xp.Common;

namespace PayIn.Application.Dto.SmartCity.Results
{
	public partial class ApiSensorGetInstantaneousResult
	{
		public int Id { get; set; }
		public XpDateTime Timestamp { get; set; }
		public decimal Value { get; set; }
	}
}
