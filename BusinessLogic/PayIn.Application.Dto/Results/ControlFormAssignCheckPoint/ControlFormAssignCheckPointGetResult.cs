using System.Collections.Generic;

namespace PayIn.Application.Dto.Results.ControlFormAssignCheckPoint
{
	public partial class ControlFormAssignCheckPointGetResult
	{
		public int Id { get; set; }
		public IEnumerable<ControlFormAssignCheckPointGetResult_Argument> Arguments { get; set; }
	}
}
