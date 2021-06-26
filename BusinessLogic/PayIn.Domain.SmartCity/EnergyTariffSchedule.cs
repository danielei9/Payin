using PayIn.Domain.SmartCity.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Xp.Domain;

namespace PayIn.Domain.SmartCity
{
	public class EnergyTariffSchedule : Entity
	{
		[Required(AllowEmptyStrings = false)] public string Name { get; set; }
		                                      public WeekDays WeekDay { get; set; }
		[Required(AllowEmptyStrings = false)] public DateTime Since { get; set; }
		[Required(AllowEmptyStrings = false)] public DateTime Until { get; set; }
		
		#region Tariff
		public int TariffId { get; set; }
		[ForeignKey(nameof(EnergyTariffSchedule.TariffId))]
		public EnergyTariff Tariff { get; set; }
		#endregion Tariff

		#region TimeTables
		[InverseProperty(nameof(EnergyTariffTimeTable.Schedule))]
		public ICollection<EnergyTariffTimeTable> TimeTables { get; set; }
		#endregion PrTimeTableicess
	}
}
