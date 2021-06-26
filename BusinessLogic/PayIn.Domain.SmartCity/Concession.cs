using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Xp.Domain;

namespace PayIn.Domain.SmartCity
{
	public class Concession : IEntity
	{
		[Key]
		[DatabaseGenerated(DatabaseGeneratedOption.None)]
		                                      public int    Id    { get; set; }
		[Required(AllowEmptyStrings = false)] public string Login { get; set; }
		[Required(AllowEmptyStrings = false)] public string Name { get; set; }

		#region Devices
		[InverseProperty(nameof(Device.Concession))]
		public ICollection<Device> Devices { get; set; } = new List<Device>();
		#endregion Devices

		#region Holidays
		[InverseProperty(nameof(Holiday.Concession))]
		public ICollection<Holiday> Holidays { get; set; } = new List<Holiday>();
		#endregion Holidays
	}
}
