using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using Xp.Common;
using Xp.Domain;

namespace PayIn.Domain.Public
{
	public class ControlPlanningItem : IEntity
	{
		public int    Id    { get; set; }

		#region Since
		public int SinceId { get; set; }
		public ControlPlanningCheck Since { get; set; }
		#endregion Since

		#region Until
		public int UntilId { get; set; }
		public ControlPlanningCheck Until { get; set; }
		#endregion Until

		#region Planning
		public int PlanningId { get; set; }
		public ControlPlanning Planning { get; set; }
		#endregion Planning

		#region ControlPresences
		[InverseProperty("PlanningItem")]
		public ICollection<ControlPresence> ControlPresences { get; set; }
		#endregion ControlPresences

		#region Constructors
		public ControlPlanningItem()
		{
			ControlPresences = new List<ControlPresence>();
		}
		#endregion Constructors
	}
}
