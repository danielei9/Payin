using PayIn.Common;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using Xp.Domain;

namespace PayIn.Domain.Public
{
	public class ServiceCheckPoint : IEntity
	{
		                 public int            Id           { get; set; }
		[Precision(9,6)] public decimal?       Latitude     { get; set; }
		[Precision(9,6)] public decimal?       Longitude    { get; set; }
		                 public string         Name         { get; set; }
		                 public string         Observations { get; set; }
		                 public CheckPointType Type         { get; set; }

		#region Item
		public int ItemId { get; set; }
		public ControlItem Item { get; set; }
		#endregion Item

		#region Supplier
		public int SupplierId { get; set; }
		public ServiceSupplier Supplier { get; set; }
		#endregion Supplier

		#region Presences
		[InverseProperty("CheckPoint")]
		public ICollection<ControlPresence> Presences { get; set; }
		#endregion Presences

		#region PlanningChecks
		[InverseProperty("CheckPoint")]
		public ICollection<ControlPlanningCheck> PlanningChecks { get; set; }
		#endregion PlanningChecks

		#region TemplateChecks
		[InverseProperty("CheckPoint")]
		public ICollection<ControlTemplateCheck> TemplateChecks { get; set; }
		#endregion TemplateChecks

		#region Tag
		public int? TagId { get; set; }
		public ServiceTag Tag { get; set; }
		#endregion Tag

		#region FormAssigns
		[InverseProperty("CheckPoint")]
		public ICollection<ControlFormAssign> FormAssigns { get; set; }
		#endregion FormAssigns

		#region Constructors
		public ServiceCheckPoint()
		{
			Presences = new List<ControlPresence>();
			PlanningChecks = new List<ControlPlanningCheck>();
			TemplateChecks = new List<ControlTemplateCheck>();
			FormAssigns = new List<ControlFormAssign>();
		}
		#endregion Constructors
	}
}
