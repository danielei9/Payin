using PayIn.Common;
using PayIn.Domain.Public;

namespace PayIn.Application.Dto.Results
{
	public class ControlPlanningGetAllResult_Item : CalendarItemResult
	{
		public int Id { get; set; }
		public bool CheckTimetable { get; set; }
		public CheckType EntranceCheckType { get; set; }
		public CheckType ExitCheckType { get; set; }
		public int WorkerId { get; set; }
		public string WorkerName { get; set; }
	}
}
