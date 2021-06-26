using PayIn.Common;
using System.Collections.Generic;
using Xp.Common;

namespace PayIn.Application.Dto.Results.ControlFormAssign
{
	public class ControlFormAssignGetIdsResult_Assign
	{
		public int Id { get; set; }
		public int PresencesCount { get; set; }
		public int FormId { get; set; }
		public string FormName { get; set; }
		public PresenceType Type { get; set; }
		public IEnumerable<ControlFormAssignGetIdsResult_Value> Values { get; set; }
		public XpDateTime DateTime { get; set; }
	}
}
