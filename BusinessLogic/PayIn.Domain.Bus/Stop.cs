using PayIn.Domain.Bus.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Xp.Domain;

namespace PayIn.Domain.Bus
{
	public class Stop : Entity
	{
		[Required(AllowEmptyStrings = false)] public string Code { get; set; }
		[Required(AllowEmptyStrings = false)] public string MasterCode { get; set; }
		[Required(AllowEmptyStrings = true)]  public string Name { get; set; }
		                                      public NodeType Type { get; set; }
		                                      public TimeSpan WaitingTime { get; set; }
		                                      public int Order { get; set; }

		[Required(AllowEmptyStrings = true)]  public string Location { get; set; }
		[Precision(9, 6)]                     public decimal? Longitude { get; set; }
		[Precision(9, 6)]                     public decimal? Latitude { get; set; }
		                                      public decimal? GeofenceRadious { get; set; }

		                                      public bool IsDefaultStop { get; set; }
		                                      public bool IsLastStopsGo { get; set; }
		                                      public bool IsLastStopsBack { get; set; }

		#region Line
		public int LineId { get; set; }
		[ForeignKey(nameof(LineId))]
		public Line Line { get; set; }
		#endregion Line

		#region Entrances
		[InverseProperty(nameof(Link.To))]
		public ICollection<Link> Entrances { get; set; } = new List<Link>();
		#endregion Entrances

		#region Exits
		[InverseProperty(nameof(Link.From))]
		public ICollection<Link> Exits { get; set; } = new List<Link>();
		#endregion Exits

		#region RequestStops
		[InverseProperty(nameof(RequestStop.Stop))]
		public ICollection<RequestStop> RequestStops { get; set; } = new List<RequestStop>();
		#endregion RequestStops

		#region Buses
		[InverseProperty(nameof(Vehicle.CurrentStop))]
		public ICollection<Vehicle> Buses { get; set; } = new List<Vehicle>();
		#endregion Buses
	}
}
