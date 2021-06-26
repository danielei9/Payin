namespace PayIn.Domain.SmartCity.Enums
{
	public enum SensorType
	{
		Other = 0,
		// Monophasic
		// Active Power
		PowerActive = 3,
		PowerActiveR = 19,
		PowerActiveS = 20,
		PowerActiveT = 21,
		// Reactive Power
		PowerReactive = 4,
		PowerReactiveR = 22,
		PowerReactiveS = 23,
		PowerReactiveT = 24,
		// Aparent Power
		PowerAparent = 5,
		// Energy
		EnergyActive = 6,
		EnergyReactive = 7,
		EnergyAparent = 8,
		// Coseno de Phi
		CosinePhi = 9,
		CosinePhiR = 25,
		CosinePhiS = 26,
		CosinePhiT = 27,
		// Triphasic
		Voltage = 1,
		VoltageR = 18,
		VoltageS = 10,
		VoltageT = 11,
		// Current
		Current = 2,
		CurrentR = 17,
		CurrentS = 12,
		CurrentT = 13,
		// Position
		Longitude = 14,
		Latitude = 15,
		// Price
		//Price = 16 // Desuso
		// Air Clima
		AirTemperature = 16,
		AirHumidity = 17,
		// Water
		WaterTemperature = 18,
		Ph = 19,
		Chloride = 20,
		ConsignedTemperature = 21,
		Valve = 22,
		WaterTemperatureEntry = 23
	}
}
