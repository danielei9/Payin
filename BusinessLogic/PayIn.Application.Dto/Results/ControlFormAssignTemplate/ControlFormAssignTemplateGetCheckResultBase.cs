using System;
using Xp.Application.Dto;
using Xp.Common;

namespace PayIn.Application.Dto.Results.ControlFormAssignTemplate
{
	public partial class ControlFormAssignTemplateGetCheckResultBase : ResultBase<ControlFormAssignTemplateGetCheckResult>
	{
		public int	  CheckId     { get; set; }
		public XpTime CheckTime   { get; set; }
		public XpTime CheckTimeXT { set { CheckTime = value; } }
	}
}
