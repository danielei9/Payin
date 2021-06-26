using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using Xp.Domain;

namespace PayIn.Domain.SmartCity
{
	public class Alert : Entity
	{
		#region Alarms
		[InverseProperty(nameof(Alarm.Alert))]
		public ICollection<Alarm> Alarms { get; set; }
		#endregion Alarms
	}
}
