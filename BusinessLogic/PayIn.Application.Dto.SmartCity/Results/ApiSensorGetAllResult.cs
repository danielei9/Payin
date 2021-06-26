using PayIn.Domain.SmartCity.Enums;
using System.Collections.Generic;
using Xp.Common;

namespace PayIn.Application.Dto.SmartCity.Results
{
	public partial class ApiSensorGetAllResult
	{
		public int Id { get; set; }
		public string Name { get; set; }
		public string Code { get; set; }
		public SensorType Type { get; set; }
		public bool Updatable { get; set; }
		public decimal? LastValue { get; set; }
		public decimal? TargetValue { get; set; }
		public string Unit { get; set; }
		public XpDateTime LastTimestamp { get; set; }
		public bool HasMaximeter { get; set; }
		public string TariffName { get; set; }
		public string ContractName { get; set; }
		public string ContractCompany { get; set; }

		public IEnumerable<ApiSensorGetAllResult_Price> Prices { get; set; }
	}
}
