using Xp.Common;

namespace PayIn.Application.Dto.SmartCity.Results
{
	public class ApiEnergyTariffGetAllResult_TimeTable
	{
		public int Id { get; set; }
		public XpTime Since { get; set; }
		public XpTime Until { get; set; }
		public int PeriodId { get; set; }
		public string PeriodName { get; set; }
		public string PeriodColor { get; set; }
	}
}
