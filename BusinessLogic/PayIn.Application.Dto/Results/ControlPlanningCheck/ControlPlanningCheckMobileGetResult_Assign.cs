using System.Collections.Generic;

namespace PayIn.Application.Dto.Results.ControlPlanningCheck
{
	public class ControlPlanningCheckMobileGetResult_Assign
	{
		public int Id { get; set; }
		public string FormName { get; set; }
		public string FormObservations { get; set; }
		public IEnumerable<ControlPlanningCheckMobileGetResult_Value> Values { get; set; }
	}
}
