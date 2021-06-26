using PayIn.Domain.SmartCity.Enums;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Xp.Domain;

namespace PayIn.Domain.SmartCity
{
	public class Device : Entity
	{
		                                      public DeviceState State { get; set; } = DeviceState.Active;
		[Required(AllowEmptyStrings = false)] public string Name { get; set; } = "";
		[Required(AllowEmptyStrings = true)]  public string ProviderName { get; set; } = "";
		[Required(AllowEmptyStrings = true)]  public string ProviderCode { get; set; } = "";
		[Required(AllowEmptyStrings = true)]  public string Model { get; set; } = "";
		                                      public decimal CO2Factor { get; set; }

		#region Components
		[InverseProperty(nameof(Component.Device))]
		public ICollection<Component> Components { get; set; } = new Collection<Component>();
		#endregion Components

		#region Schedules
		[InverseProperty(nameof(ModelSchedule.Device))]
		public ICollection<ModelSchedule> Schedules { get; set; } = new Collection<ModelSchedule>();
		#endregion Schedules

		#region Concession
		public int? ConcessionId { get; set; }
		[ForeignKey(nameof(Device.ConcessionId))]
		public Concession Concession { get; set; }
		#endregion Concession
	}
}
