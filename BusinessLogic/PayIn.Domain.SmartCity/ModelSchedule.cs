using PayIn.Domain.SmartCity.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Xp.Domain;

namespace PayIn.Domain.SmartCity
{
	public class ModelSchedule : Entity
	{
		[Required(AllowEmptyStrings = false)] public string Name { get; set; }
		                                      public WeekDays WeekDay { get; set; }
		[Required]                            public DateTime Since { get; set; }
		[Required]                            public DateTime Until { get; set; }
		
		#region Device
		public int DeviceId { get; set; }
		[ForeignKey(nameof(ModelSchedule.DeviceId))]
		public Device Device { get; set; }
		#endregion Device

		#region TimeTables
		[InverseProperty(nameof(ModelTimeTable.Schedule))]
		public ICollection<ModelTimeTable> TimeTables { get; set; }
		#endregion PrTimeTableicess
	}
}
