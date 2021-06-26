using System.Collections.Generic;

namespace PayIn.Application.Dto.Results.ControlFormAssign
{
	public partial class ControlFormAssignGetResult
	{
		public int                Id             { get; set; }
		public int                PresencesCount { get; set; }
		public int                FormId         { get; set; }
		public string             FormName       { get; set; }
		public IEnumerable<ControlFormAssignGetResult_Value> Values         { get; set; }
	}
}
