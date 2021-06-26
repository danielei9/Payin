using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Xp.Domain;

namespace PayIn.Domain.SmartCity
{
	public class EnergyTariff : Entity
	{
		[Required(AllowEmptyStrings = false)] public string Name { get; set; }
		                                      public decimal? VoltageMax { get; set; }
		[Required(AllowEmptyStrings = true)]  public string VoltageMaxUnit { get; set; }
		                                      public decimal VoltageMaxFactor { get; set; }
		                                      public decimal? PowerMax { get; set; }
		[Required(AllowEmptyStrings = true)]  public string PowerMaxUnit { get; set; }
		                                      public decimal PowerMaxFactor { get; set; }

		#region Schedules
		[InverseProperty(nameof(EnergyTariffSchedule.Tariff))]
		public ICollection<EnergyTariffSchedule> Schedules { get; set; }
		#endregion Schedules

		#region Periods
		[InverseProperty(nameof(EnergyTariffPeriod.Tariff))]
		public ICollection<EnergyTariffPeriod> Periods { get; set; }
		#endregion Periods

		#region Contracts
		[InverseProperty(nameof(EnergyContract.Tariff))]
		public ICollection<EnergyContract> Contracts { get; set; }
		#endregion Contracts
	}
}
