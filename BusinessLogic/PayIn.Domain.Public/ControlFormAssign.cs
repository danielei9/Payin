using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using Xp.Domain;

namespace PayIn.Domain.Public
{
	public class ControlFormAssign : Entity
	{
		#region Form
		public int FormId { get; set; }
		public ControlForm Form { get; set; }
		#endregion Form

		#region Check
		public int CheckId { get; set; }
		public ControlPlanningCheck Check { get; set; }
		#endregion Check

		#region CheckPoint
		public int? CheckPointId { get; set; }
		public ServiceCheckPoint CheckPoint { get; set; }
		#endregion CheckPoint

		#region Values
		[InverseProperty("Assign")]
		public ICollection<ControlFormValue> Values { get; set; } = new List<ControlFormValue>();
        #endregion Values
	}
}
