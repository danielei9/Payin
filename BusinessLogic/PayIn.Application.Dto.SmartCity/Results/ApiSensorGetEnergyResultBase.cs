using PayIn.Application.Dto.SmartCity.Arguments;
using Xp.Application.Dto;

namespace PayIn.Application.Dto.SmartCity.Results
{
	public class ApiSensorGetEnergyResultBase : ResultBase<ApiSensorGetEnergyResult>
	{
		public string Unit { get; set; }
		public int Id { get; set; }
		public string Name { get; set; }
		public string ComponentName { get; set; }
		public string DeviceName { get; set; }
		public ApiSensorGetEnergyArguments_Period Period { get; set; }
		public string PeriodName { get; set; }
		// Consumption
		public decimal Consumption { get; set; }
		public decimal ConsumptionCO2 { get; set; }
		public decimal? ConsumptionCost { get; set; }
		public decimal P1Consumption { get; set; }
		public decimal? P1ConsumptionCost { get; set; }
		public decimal P2Consumption { get; set; }
		public decimal? P2ConsumptionCost { get; set; }
		public decimal P3Consumption { get; set; }
		public decimal? P3ConsumptionCost { get; set; }
		public decimal P4Consumption { get; set; }
		public decimal? P4ConsumptionCost { get; set; }
		public decimal P5Consumption { get; set; }
		public decimal? P5ConsumptionCost { get; set; }
		public decimal P6Consumption { get; set; }
		public decimal? P6ConsumptionCost { get; set; }
		// PowerLack
		public decimal PowerLack { get; set; }
		public decimal PowerLackCost { get; set; }
		public decimal P1PowerLack { get; set; }
		public decimal P1PowerLackCost { get; set; }
		public decimal P2PowerLack { get; set; }
		public decimal P2PowerLackCost { get; set; }
		public decimal P3PowerLack { get; set; }
		public decimal P3PowerLackCost { get; set; }
		public decimal P4PowerLack { get; set; }
		public decimal P4PowerLackCost { get; set; }
		public decimal P5PowerLack { get; set; }
		public decimal P5PowerLackCost { get; set; }
		public decimal P6PowerLack { get; set; }
		public decimal P6PowerLackCost { get; set; }
		// PowerExcess
		public decimal PowerExcess { get; set; }
		public decimal PowerExcessCost { get; set; }
		public decimal P1PowerExcess { get; set; }
		public decimal P1PowerExcessCost { get; set; }
		public decimal P2PowerExcess { get; set; }
		public decimal P2PowerExcessCost { get; set; }
		public decimal P3PowerExcess { get; set; }
		public decimal P3PowerExcessCost { get; set; }
		public decimal P4PowerExcess { get; set; }
		public decimal P4PowerExcessCost { get; set; }
		public decimal P5PowerExcess { get; set; }
		public decimal P5PowerExcessCost { get; set; }
		public decimal P6PowerExcess { get; set; }
		public decimal P6PowerExcessCost { get; set; }
		// Color
		public string P1Color { get; set; }
		public string P2Color { get; set; }
		public string P3Color { get; set; }
		public string P4Color { get; set; }
		public string P5Color { get; set; }
		public string P6Color { get; set; }
	}
}
