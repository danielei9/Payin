using PayIn.Domain.Bus.Enums;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using Xp.Domain;

namespace PayIn.Domain.Bus
{
	public class Route : Entity
	{
		public RouteSense Sense { get; set; }

		#region Line
		public int LineId { get; set; }
		[ForeignKey(nameof(LineId))]
		public Line Line { get; set; }
		#endregion Line

		#region Links
		[InverseProperty(nameof(Link.Route))]
		public ICollection<Link> Links { get; set; }
		#endregion Links
	}
}
