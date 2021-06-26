using System.Collections.Generic;

namespace PayIn.Application.Dto.Results.ControlFormAssignTemplate
{
	public partial class ControlFormAssignTemplateGetResult
	{
		public int Id { get; set; }
		public IEnumerable<ControlFormAssignTemplateGetResult_Argument> Arguments { get; set; }
	}
}
