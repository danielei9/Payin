using System.Collections.Generic;

namespace PayIn.Application.Dto.Results.ControlItem
{
	public class ControlItemMobileGetAllResult_Assign
	{
		public int Id { get; set; }
		public string FormName { get; set; }
		public string FormObservations { get; set; }
		public IEnumerable<ControlItemMobileGetAllResult_Value> Values { get; set; }
	}
}
