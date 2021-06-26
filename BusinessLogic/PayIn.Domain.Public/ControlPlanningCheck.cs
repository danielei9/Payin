using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using Xp.Common;
using Xp.Domain;

namespace PayIn.Domain.Public
{
	public class ControlPlanningCheck : IEntity
	{
		public int    Id    { get; set; }
		public DateTime Date { get; set; }

		#region Planning
		public int PlanningId { get; set; }
		public ControlPlanning Planning { get; set; }
		#endregion Planning

		#region ItemsSince
		[InverseProperty("Since")]
		public ICollection<ControlPlanningItem> ItemsSince { get; set; }
		#endregion ItemsSince

		#region ItemsUntil
		[InverseProperty("Until")]
		public ICollection<ControlPlanningItem> ItemsUntil { get; set; }
		#endregion Until

		#region CheckPoint
		public int? CheckPointId { get; set; }
		public ServiceCheckPoint CheckPoint { get; set; }
		#endregion CheckPoint

		#region FormAssigns
		[InverseProperty("Check")]
		public ICollection<ControlFormAssign> FormAssigns { get; set; }
		#endregion FormAssigns

		#region ControlPresences
		[InverseProperty("PlanningCheck")]
		public ICollection<ControlPresence> ControlPresences { get; set; }
		#endregion ControlPresences

		#region Constructors
		public ControlPlanningCheck()
		{
			FormAssigns = new List<ControlFormAssign>();
			ControlPresences = new List<ControlPresence>();
		}
		#endregion Constructors
	}
}
