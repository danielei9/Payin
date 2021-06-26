using System.Collections.Generic;

namespace PayIn.Application.Dto.Arguments.ControlFormAssign
{
	public class ControlFormAssignUpdateArguments_Assign
	{
		public int Id { get; set; }
		public IEnumerable<ControlFormAssignUpdateArguments_Value> Values { get; set; }
	}
}
