using System;
using Xp.Application.Dto;
using Xp.Common;

namespace PayIn.Application.Dto.Results.ControlFormAssign
{
	public partial class ControlFormAssignGetCheckResultBase : ResultBase<ControlFormAssignGetCheckResult>
	{
		public int        CheckId     { get; set; }
		public XpDateTime CheckDate   { get; set; }
		public DateTime   CheckDateDT { set { CheckDate = value; } }
		public int        WorkerId    { get; set; }
		public string     WorkerName  { get; set; }
	}
}
