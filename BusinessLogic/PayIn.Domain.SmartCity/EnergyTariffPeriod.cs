using PayIn.Domain.SmartCity.Enums;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Xp.Domain;

namespace PayIn.Domain.SmartCity
{
	public class EnergyTariffPeriod : Entity
	{
		                                      public PeriodType Type { get; set; }
		[Required(AllowEmptyStrings = false)] public string Name { get; set; } = "";
		[Required(AllowEmptyStrings = true)]  public string Color { get; set; } = "";

		#region Tariff
		public int TariffId { get; set; }
		[ForeignKey(nameof(EnergyTariffPeriod.TariffId))]
		public EnergyTariff Tariff { get; set; }
		#endregion Tariff

		#region TimeTables
		[InverseProperty(nameof(EnergyTariffTimeTable.Period))]
		public ICollection<EnergyTariffTimeTable> TimeTables { get; set; }
		#endregion PrTimeTableicess

		#region Prices
		[InverseProperty(nameof(EnergyTariffPrice.Period))]
		public ICollection<EnergyTariffPrice> Prices { get; set; }
		#endregion Prices
	}
}
