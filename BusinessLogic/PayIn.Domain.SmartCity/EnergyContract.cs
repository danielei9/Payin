using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Xp.Domain;

namespace PayIn.Domain.SmartCity
{
	public class EnergyContract : Entity
	{
		[Required(AllowEmptyStrings = true)] public string Name { get; set; } = "";
		[Required(AllowEmptyStrings = true)] public string Company { get; set; } = "";
		[Required(AllowEmptyStrings = true)] public string Reference { get; set; } = "";
		
		#region Tariff
		public int? TariffId { get; set; }
		[ForeignKey(nameof(EnergyContract.TariffId))]
		public EnergyTariff Tariff { get; set; }
		#endregion Tariff

		#region Sensors
		[InverseProperty(nameof(Sensor.EnergyContract))]
		public ICollection<Sensor> Sensors { get; set; }
		#endregion Sensors

		#region Prices
		[InverseProperty(nameof(EnergyTariffPrice.Contract))]
		public ICollection<EnergyTariffPrice> Prices { get; set; }
		#endregion Prices
	}
}
