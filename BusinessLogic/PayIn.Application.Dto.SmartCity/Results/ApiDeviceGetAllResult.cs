using Xp.Common;

namespace PayIn.Application.Dto.SmartCity.Results
{
	public partial class ApiDeviceGetAllResult
	{
		public int Id { get; set; }
		public string Name { get; set; }
		public XpDateTime LastTimestamp { get; set; }
		public string Model { get; set; }
		public decimal CO2Factor { get; set; }
		public int ComponentsNumber { get; set; }
		public string ProviderName { get; set; }
		public string ProviderCode { get; set; }
		public string ConcessionLogin { get; set; }
	}
}