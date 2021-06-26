using PayIn.Domain.SmartCity.Enums;

namespace PayIn.Application.Dto.SmartCity.Results
{
	public partial class ApiSensorGetTargetValuesResult
	{
		public int Id { get; set; }
		public string Code { get; set; }
		public decimal TargetValue { get; set; }
		public SensorType Type { get; set; }
	}
}
