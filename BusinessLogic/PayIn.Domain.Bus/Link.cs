using System;
using System.ComponentModel.DataAnnotations.Schema;
using Xp.Domain;

namespace PayIn.Domain.Bus
{
	public class Link : Entity
	{
		public int Weight { get; set; }
		public TimeSpan Time { get; set; }

		#region Route
		public int RouteId { get; set; }
		[ForeignKey(nameof(RouteId))]
		public Route Route { get; set; }
		#endregion Route

		#region From
		public int FromId { get; set; }
		[ForeignKey(nameof(FromId))]
		public Stop From { get; set; }
		#endregion From

		#region To
		public int ToId { get; set; }
		[ForeignKey(nameof(ToId))]
		public Stop To { get; set; }
		#endregion To
	}
}
