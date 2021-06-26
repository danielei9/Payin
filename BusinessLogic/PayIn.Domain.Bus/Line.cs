using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Xp.Domain;

namespace PayIn.Domain.Bus
{
	public class Line : Entity
	{
		[Required(AllowEmptyStrings = false)] public string Login { get; set; }
		[Required(AllowEmptyStrings = false)] public string Name { get; set; }

		#region TimeTables
		[InverseProperty(nameof(TimeTable.Line))]
		public ICollection<TimeTable> TimeTables { get; set; }
		#endregion TimeTables

		#region Buses
		[InverseProperty(nameof(Vehicle.Line))]
		public ICollection<Vehicle> Buses { get; set; } = new List<Vehicle>();
		#endregion Buses

		#region Routes
		[InverseProperty(nameof(Route.Line))]
		public ICollection<Route> Routes { get; set; }
		#endregion Routes

		#region Stops
		[InverseProperty(nameof(Stop.Line))]
		public ICollection<Stop> Stops { get; set; }
		#endregion Stops
	}
}
