using PayIn.Domain.Bus.Enums;
using System.ComponentModel.DataAnnotations.Schema;
using Xp.Domain;

namespace PayIn.Domain.Bus
{
	public class Vehicle : Entity
	{
		public string Code { get; set; }
		public RouteSense CurrentSense { get; set; }

		#region Line
		public int? LineId { get; set; }
		[ForeignKey(nameof(LineId))]
		public Line Line { get; set; }
		#endregion Line

		#region CurrentStop
		public int? CurrentStopId { get; set; }
		[ForeignKey(nameof(CurrentStopId))]
		public Stop CurrentStop { get; set; }
		#endregion CurrentStop
	}
}