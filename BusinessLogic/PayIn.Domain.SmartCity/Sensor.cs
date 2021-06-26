using PayIn.Domain.SmartCity.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Globalization;
using Xp.Domain;

namespace PayIn.Domain.SmartCity
{
	public class Sensor : Entity
	{
		                                      public SensorState State { get; set; } = SensorState.Active;
		[Required(AllowEmptyStrings = false)] public string Code { get; set; } = "";
		[Required(AllowEmptyStrings = false)] public string Name { get; set; } = "";
		[Required(AllowEmptyStrings = true)]  public string Unit { get; set; } = "";
		                                      public SensorType Type { get; set; }
		                                      public bool IsAcumulative { get; set; } = false;
		                                      public DateTime? LastTimestamp { get; set; }
		[Precision(18, 6)]                    public decimal? LastValue { get; set; }
		[Precision(18, 6)]                    public decimal? TargetValue { get; set; }
		[Required]                            public bool HasMaximeter { get; set; } = false;
		                                      public bool Updatable { get; set; } = false;

		#region Component
		public int ComponentId { get; set; }
		[ForeignKey(nameof(Sensor.ComponentId))]
		public Component Component { get; set; }
		#endregion Component

		#region Datas
		[InverseProperty(nameof(Data.Sensor))]
		public ICollection<Data> Datas { get; set; }
		#endregion Datas

		#region EnergyContract
		public int? EnergyContractId { get; set; }
		[ForeignKey(nameof(Sensor.EnergyContractId))]
		public EnergyContract EnergyContract { get; set; }
		#endregion EnergyContract

		#region GetDateTimeFromSentilo
		public DateTime? GetDateTimeFromSentilo(string dateTime)
		{
			if (dateTime.IsNullOrEmpty())
				return null;

			var remoteTimeStemp = DateTime.ParseExact(dateTime, @"dd/MM/yyyy\THH:mm:ss", CultureInfo.InvariantCulture);
			var utcDateTime = TimeZoneInfo.ConvertTimeToUtc(
				remoteTimeStemp,
				TimeZoneInfo.FindSystemTimeZoneById(this.Component.TimeZone) // "Romance Standard Time"
			);

			return utcDateTime;
		}
		#endregion GetDateTimeFromSentilo

		#region GetDateTimeFromSentilo
		public string GetDateTimeToSentilo(DateTime? dateTime)
		{
			if (dateTime == null)
				return "";

			var localDateTime = DateTime.SpecifyKind(dateTime.Value, DateTimeKind.Utc);
			var utcDateTime = dateTime.Value.ToUniversalTime();
			var sentiloDateTime = TimeZoneInfo.ConvertTimeFromUtc(
				utcDateTime,
				TimeZoneInfo.FindSystemTimeZoneById(this.Component.TimeZone) // "Romance Standard Time"
			);
			var result = dateTime.Value.ToString(@"dd/MM/yyyy\THH:mm:ss", CultureInfo.InvariantCulture);

			return result;
		}
		#endregion GetDateTimeFromSentilo

		#region GetDateTimeToLocal
		public DateTime? GetDateTimeToLocal(DateTime? utcDateTime)
		{
			if (utcDateTime == null)
				return null;
			
			var localDateTime = TimeZoneInfo.ConvertTimeFromUtc(
				utcDateTime.Value,
				TimeZoneInfo.FindSystemTimeZoneById(this.Component.TimeZone) // "Romance Standard Time"
			);

			return localDateTime;
		}
		#endregion GetDateTimeFromSentilo
	}
}
