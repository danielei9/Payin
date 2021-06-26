using PayIn.Domain.SmartCity.Enums;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Xp.Domain;

namespace PayIn.Domain.SmartCity
{
	public class EnergyTariffPrice : Entity
	{
		[Precision(18, 6)]                   public decimal? EnergyPrice { get; set; }
		[Precision(18, 6)]                   public decimal? PowerPrice { get; set; }
		                                     public EnergyTariffPriceState State { get; set; } = EnergyTariffPriceState.Active;
		// PowerMax
		                                     public PowerManagementType PowerManagement { get; set; }
		                                     public decimal? PowerContract { get; set; }
		[Required(AllowEmptyStrings = true)] public string   PowerContractUnit { get; set; } = "";
		                                     public decimal  PowerContractFactor { get; set; } = 1;

		#region Period
		public int PeriodId { get; set; }
		[ForeignKey(nameof(EnergyTariffPrice.PeriodId))]
		public EnergyTariffPeriod Period { get; set; }
		#endregion Period

		#region Datas
		[InverseProperty(nameof(Data.EnergyTariffPrice))]
		public ICollection<Data> Datas { get; set; } = new List<Data>();
		#endregion Datas

		#region Contract
		public int ContractId { get; set; }
		[ForeignKey(nameof(EnergyTariffPrice.ContractId))]
		public EnergyContract Contract { get; set; }
		#endregion Contract
	}
}
