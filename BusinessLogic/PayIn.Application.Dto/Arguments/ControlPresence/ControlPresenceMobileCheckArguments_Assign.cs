using System.Collections.Generic;

namespace PayIn.Application.Dto.Arguments.ControlPresence
{
	public partial class ControlPresenceMobileCheckArguments_Assign
	{
		public int Id { get; set; }
		public IEnumerable<ControlPresenceMobileCheckArguments_Value> Values { get; set; }
	}
}
