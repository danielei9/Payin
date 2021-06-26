using System.ComponentModel.DataAnnotations.Schema;
using Xp.Domain;

namespace PayIn.Domain.SmartCity
{
	public class Data : Entity
	{
		[Precision(18, 6)] public decimal Value { get; set; }
		[Precision(18, 6)] public decimal? Price { get; set; }

		#region DataSet
		public int DataSetId { get; set; }
		[ForeignKey(nameof(DataSetId))]
		public DataSet DataSet { get; set; }
		#endregion DataSet

		#region Sensor
		public int SensorId { get; set; }
		[ForeignKey(nameof(SensorId))]
		public Sensor Sensor { get; set; }
		#endregion Sensor

		#region EnergyTariffPrice
		public int? EnergyTariffPriceId { get; set; }
		[ForeignKey(nameof(EnergyTariffPriceId))]
		public EnergyTariffPrice EnergyTariffPrice { get; set; }
		#endregion EnergyTariffPrice

		#region Create
		public static Data Create(DataSet dataSet, Sensor sensor, decimal value)
		{
			return new Data
			{
				Value = value,
				Price = 0,
				DataSet = dataSet,
				Sensor = sensor,
				EnergyTariffPriceId = null
			};
		}
		#endregion Create
	}
}
