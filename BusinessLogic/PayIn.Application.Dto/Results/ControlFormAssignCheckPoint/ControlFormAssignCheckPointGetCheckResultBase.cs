using System;
using Xp.Application.Dto;
using Xp.Common;

namespace PayIn.Application.Dto.Results.ControlFormAssignCheckPoint
{
	public partial class ControlFormAssignCheckPointGetCheckResultBase : ResultBase<ControlFormAssignCheckPointGetCheckResult>
	{
		public int CheckId { get; set; }
		public string CheckPointName { get; set; }
	}
}
