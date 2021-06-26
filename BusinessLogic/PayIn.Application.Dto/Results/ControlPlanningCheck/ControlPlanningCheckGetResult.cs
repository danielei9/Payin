using System;
using Xp.Common;

namespace PayIn.Application.Dto.Results.ControlPlanningCheck
{
	public partial class ControlPlanningCheckGetResult
	{
		public int        Id         { get; set; }
		public XpDateTime Date       { get; set; }
		public DateTime   DateDT     { set { Date = value; } }
		public int        FormsCount { get; set; }
		public int        WorkerId   { get; set; }
		public string     WorkerName   { get; set; }
	}
}
