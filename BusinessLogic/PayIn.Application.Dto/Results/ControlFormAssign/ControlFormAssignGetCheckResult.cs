using System.Collections.Generic;

namespace PayIn.Application.Dto.Results.ControlFormAssign
{
	public partial class ControlFormAssignGetCheckResult
	{
		public int    Id       { get; set; }
		public int    FormId   { get; set; }
		public string FormName { get; set; }
		public IEnumerable<ControlFormAssignGetCheckResult_Value> Values { get; set; }
	}
}
