using PayIn.Domain.SmartCity.Enums;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Xp.Domain;

namespace PayIn.Domain.SmartCity
{
	public class Component : Entity
	{
		                                      public ComponentState State { get; set; } = ComponentState.Active;
		[Required(AllowEmptyStrings = false)] public string Name { get; set; } = "";
		[Required(AllowEmptyStrings = false)] public string Code { get; set; } = "";
		[Required(AllowEmptyStrings = true)]  public string Model { get; set; } = "";
		[Required(AllowEmptyStrings = false)] public string TimeZone { get; set; } = "";
		                                      public ComponentType Type { get; set; }
		[Precision(9, 6)]                     public decimal? Longitude { get; set; }
		[Precision(9, 6)]                     public decimal? Latitude { get; set; }
		[Required(AllowEmptyStrings = true)]  public string ProviderName { get; set; } = "";

		#region Sensors
		[InverseProperty(nameof(Sensor.Component))]
		public ICollection<Sensor> Sensors { get; set; } = new Collection<Sensor>();
		#endregion Sensors

		#region Device
		public int DeviceId { get; set; }
		[ForeignKey(nameof(DeviceId))]
		public Device Device { get; set; }
		#endregion Device

		#region DataSets
		[InverseProperty(nameof(DataSet.Component))]
		public ICollection<DataSet> DataSets { get; set; } = new Collection<DataSet>();
		#endregion DataSets

		#region GetLongitude
		public decimal? GetLongitude(string position = null)
		{
			if (position == null)
				return Longitude;

			var locations = position.SplitString(" ");
			if (locations.Length == 2)
				return Convert.ToDecimal(locations[0]);

			return Longitude;
		}
		#endregion GetLongitude

		#region GetLatitude
		public decimal? GetLatitude(string position = null)
		{
			if (position == null)
				return null;

			var locations = position.SplitString(" ");
			if (locations.Length == 2)
				return Convert.ToDecimal(locations[1]);

			return this.Latitude;
		}
		#endregion GetLatitude
	}
}
