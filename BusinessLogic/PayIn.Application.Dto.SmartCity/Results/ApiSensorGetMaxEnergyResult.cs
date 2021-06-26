namespace PayIn.Application.Dto.SmartCity.Results
{
	public partial class ApiSensorGetMaxEnergyResult
	{
		public string Label { get; set; }
		public string PeriodName { get; set; }
		// Consumption
		public decimal P1Consumption { get; set; }
		public decimal? P1ConsumptionCost { get; set; }
		public decimal P1ConsumptionCO2 { get; set; }
		public string P1Color { get; set; }
		public decimal P2Consumption { get; set; }
		public decimal? P2ConsumptionCost { get; set; }
		public decimal P2ConsumptionCO2 { get; set; }
		public string P2Color { get; set; }
		public decimal P3Consumption { get; set; }
		public decimal? P3ConsumptionCost { get; set; }
		public decimal P3ConsumptionCO2 { get; set; }
		public string P3Color { get; set; }
		public decimal P4Consumption { get; set; }
		public decimal? P4ConsumptionCost { get; set; }
		public decimal P4ConsumptionCO2 { get; set; }
		public string P4Color { get; set; }
		public decimal P5Consumption { get; set; }
		public decimal? P5ConsumptionCost { get; set; }
		public decimal P5ConsumptionCO2 { get; set; }
		public string P5Color { get; set; }
		public decimal P6Consumption { get; set; }
		public decimal? P6ConsumptionCost { get; set; }
		public decimal P6ConsumptionCO2 { get; set; }
		public string P6Color { get; set; }
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
		// Excess power
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
		// PowerContract
		public decimal? PowerContractMin { get; set; }
		public decimal? PowerContractMinCost { get; set; }
		public decimal? PowerContract { get; set; }
		public decimal? PowerContractCost { get; set; }
		public decimal? PowerContractMax { get; set; }
		public decimal? PowerContractMaxCost { get; set; }
		// ConsumptionSaving
		public decimal P1ConsumptionSaving { get; set; }
		public decimal? P1ConsumptionSavingCost { get; set; }
		public decimal P1ConsumptionSavingCO2 { get; set; }
		public decimal P2ConsumptionSaving { get; set; }
		public decimal? P2ConsumptionSavingCost { get; set; }
		public decimal P2ConsumptionSavingCO2 { get; set; }
		public decimal P3ConsumptionSaving { get; set; }
		public decimal? P3ConsumptionSavingCost { get; set; }
		public decimal P3ConsumptionSavingCO2 { get; set; }
		public decimal P4ConsumptionSaving { get; set; }
		public decimal? P4ConsumptionSavingCost { get; set; }
		public decimal P4ConsumptionSavingCO2 { get; set; }
		public decimal P5ConsumptionSaving { get; set; }
		public decimal? P5ConsumptionSavingCost { get; set; }
		public decimal P5ConsumptionSavingCO2 { get; set; }
		public decimal P6ConsumptionSaving { get; set; }
		public decimal? P6ConsumptionSavingCost { get; set; }
		public decimal P6ConsumptionSavingCO2 { get; set; }
		// ConsumptionOver
		public decimal ConsumptionOver { get; set; }
		public decimal? ConsumptionOverCost { get; set; }
		public decimal ConsumptionOverCO2 { get; set; }
		public string ColorOver { get; set; }
	}
}
