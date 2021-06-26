using PayIn.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using Xp.Domain;

namespace PayIn.Domain.Public
{
	public class ControlPresence : IEntity
	{
		                                   public int           Id              { get; set; }
										   public DateTime      CreatedAt       { get; set; }
		                                   public DateTime      Date            { get; set; }
		[Precision(9, 6)]                  public decimal?      Latitude        { get; set; }
		[Precision(9, 6)]                  public decimal?      Longitude       { get; set; }
		[Precision(9, 6)]                  public decimal?      LatitudeWanted  { get; set; }
		[Precision(9, 6)]                  public decimal?      LongitudeWanted { get; set; }
		                                   public string        Observations    { get; set; }
		                                   public string        Image           { get; set; }

		public ControlTrack TrackSince { get; set; }
		public ControlTrack TrackUntil { get; set; }

		#region CheckPoint
		public int? CheckPointId { get; set; }
		public ServiceCheckPoint CheckPoint { get; set; }
		#endregion CheckPoint

		#region PlanningItem
		public int? PlanningItemId { get; set; }
		public ControlPlanningItem PlanningItem { get; set; }
		#endregion PlanningItem

		#region PlanningCheck
		public int? PlanningCheckId { get; set; }
		public ControlPlanningCheck PlanningCheck { get; set; }
		#endregion PlanningCheck

	}
}
