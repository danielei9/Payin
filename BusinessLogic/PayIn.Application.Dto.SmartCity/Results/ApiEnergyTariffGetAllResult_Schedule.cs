using PayIn.Domain.SmartCity.Enums;
using System.Collections.Generic;
using Xp.Common;

namespace PayIn.Application.Dto.SmartCity.Results
{
	public class ApiEnergyTariffGetAllResult_Schedule
	{
		public int Id { get; set; }
		public string Name { get; set; }
		public XpDateTime Since { get; set; }
		public XpDateTime Until { get; set; }
		public WeekDays WeekDay { get; set; }
		public IEnumerable<ApiEnergyTariffGetAllResult_TimeTable> TimeTables { get; set; }
	}
}
