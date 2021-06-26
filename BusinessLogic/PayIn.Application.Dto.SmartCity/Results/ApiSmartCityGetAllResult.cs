using PayIn.Domain.SmartCity.Enums;
using System.Collections.Generic;
using Xp.Common;

namespace PayIn.Application.Dto.SmartCity.Results
{
	public partial class ApiSmartCityGetAllResult
	{
		public int Id { get; set; }
		public string Name { get; set; }
		public string Code { get; set; }
		public SensorType Type { get; set; }
		public decimal? LastValue { get; set; }
		public string Unit { get; set; }
		public XpDateTime LastTimestamp { get; set; }
		public string TariffName { get; set; }
		public decimal? PowerMax { get; set; }
		public decimal PowerMaxFactor { get; set; }
		public string PowerMaxUnit { get; set; }

		public IEnumerable<ApiSmartCityGetAllResult_Price> Prices { get; set; }
	}
}
