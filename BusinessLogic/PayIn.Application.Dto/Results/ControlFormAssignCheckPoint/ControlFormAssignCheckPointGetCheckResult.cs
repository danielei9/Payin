using System.Collections.Generic;

namespace PayIn.Application.Dto.Results.ControlFormAssignCheckPoint
{
	public partial class ControlFormAssignCheckPointGetCheckResult
	{
		public int Id { get; set; }
		public int FormId { get; set; }
		public string FormName { get; set; }
		public string CheckPointName { get; set; }
		public IEnumerable<ControlFormAssignCheckPointGetCheckResult_Argument> Arguments { get; set; }
	}
}
