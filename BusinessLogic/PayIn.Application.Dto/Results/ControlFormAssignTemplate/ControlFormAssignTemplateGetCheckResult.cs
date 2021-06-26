using System.Collections.Generic;

namespace PayIn.Application.Dto.Results.ControlFormAssignTemplate
{
	public partial class ControlFormAssignTemplateGetCheckResult
	{
		public int Id { get; set; }
		public int FormId { get; set; }
		public string FormName { get; set; }
		public IEnumerable<ControlFormAssignTemplateGetCheckResult_Argument> Arguments { get; set; }
	}
}
