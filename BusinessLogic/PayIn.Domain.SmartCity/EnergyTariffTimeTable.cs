using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Xp.Domain;

namespace PayIn.Domain.SmartCity
{
	public class EnergyTariffTimeTable : Entity
	{
		[Required(AllowEmptyStrings = false)] public DateTime Since { get; set; }
		[Required(AllowEmptyStrings = false)] public DateTime Until { get; set; }

		#region Schedule
		public int ScheduleId { get; set; }
		[ForeignKey(nameof(EnergyTariffTimeTable.ScheduleId))]
		public EnergyTariffSchedule Schedule { get; set; }
		#endregion Schedule

		#region Period
		public int PeriodId { get; set; }
		[ForeignKey(nameof(EnergyTariffTimeTable.PeriodId))]
		public EnergyTariffPeriod Period { get; set; }
		#endregion Period
	}
}
