using System.Collections.Generic;

namespace PayIn.Application.Dto.SmartCity.Results
{
	public partial class ApiEnergyTariffGetAllResult
	{
		public int Id { get; set; }
		public string Name { get; set; }
		public decimal? PowerMax { get; set; }
		public decimal PowerMaxFactor { get; set; }
		public string PowerMaxUnit { get; set; }
		public decimal? VoltageMax { get; set; }
		public decimal VoltageMaxFactor { get; set; }
		public string VoltageMaxUnit { get; set; }

		public IEnumerable<ApiEnergyTariffGetAllResult_Schedule> Schedules { get; set; }
	}
}
