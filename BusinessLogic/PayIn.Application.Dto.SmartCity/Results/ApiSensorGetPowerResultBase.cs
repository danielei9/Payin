using Xp.Application.Dto;

namespace PayIn.Application.Dto.SmartCity.Results
{
	public class ApiSensorGetPowerResultBase : ResultBase<ApiSensorGetPowerResult>
	{
		// Values
		public decimal P1MaxValue { get; set; }
		public decimal P2MaxValue { get; set; }
		public decimal P3MaxValue { get; set; }
		public decimal P4MaxValue { get; set; }
		public decimal P5MaxValue { get; set; }
		public decimal P6MaxValue { get; set; }
		// Colores
		public string P1Color { get; set; }
		public string P2Color { get; set; }
		public string P3Color { get; set; }
		public string P4Color { get; set; }
		public string P5Color { get; set; }
		public string P6Color { get; set; }

		public string Unit { get; set; }
	}
}
