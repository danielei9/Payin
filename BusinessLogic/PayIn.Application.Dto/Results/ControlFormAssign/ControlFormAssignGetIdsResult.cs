using System.Collections.Generic;

namespace PayIn.Application.Dto.Results.ControlFormAssign
{
	public partial class ControlFormAssignGetIdsResult
	{
		public IEnumerable<ControlFormAssignGetIdsResult_Assign> Assigns { get; set; }
	}
}
