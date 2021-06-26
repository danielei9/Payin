using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Xp.Domain;

namespace PayIn.Domain.SmartCity
{
	public class ModelTimeTable : Entity
	{
		[Required] public DateTime Since { get; set; }
		[Required] public DateTime Until { get; set; }
		[Required] public decimal Value { get; set; }

		#region Schedule
		public int ScheduleId { get; set; }
		[ForeignKey(nameof(ModelTimeTable.ScheduleId))]
		public ModelSchedule Schedule { get; set; }
		#endregion Schedule

		//#region Period
		//public int PeriodId { get; set; }
		//[ForeignKey(nameof(EnergyTariffTimeTable.PeriodId))]
		//public EnergyTariffPeriod Period { get; set; }
		//#endregion Period
	}
}
