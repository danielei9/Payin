using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using Xp.Common;
using Xp.Domain;

namespace PayIn.Domain.Public
{
	public class ControlPlanning : IEntity
	{
		public int       Id            { get; set; }
		public DateTime  Date          { get; set; }
		public DateTime? CheckDuration { get; set; }

		#region Worker
		public int WorkerId { get; set; }
		public ServiceWorker Worker { get; set; }
		#endregion Worker

		#region Item
		public int ItemId { get; set; }
		public ControlItem Item { get; set; }
		#endregion Item

		#region Items
		[InverseProperty("Planning")]
		public ICollection<ControlPlanningItem> Items { get; set; }
		#endregion Items

		#region Checks
		[InverseProperty("Planning")]
		public ICollection<ControlPlanningCheck> Checks { get; set; }
		#endregion Checks

		#region Constructors
		public ControlPlanning()
		{
			Items = new List<ControlPlanningItem>();
			Checks = new List<ControlPlanningCheck>();
		}
		#endregion Constructors
	}
}
