using PayIn.Common;

namespace PayIn.Application.Dto.Results.ControlFormAssignCheckPoint
{
	public class ControlFormAssignCheckPointGetResult_Argument
	{
		public int Id { get; set; }
		public string Name { get; set; }
		public string Observations { get; set; }
		public ControlFormArgumentType Type { get; set; }
		public ControlFormArgumentTarget Target { get; set; }
		public int MinOptions { get; set; }
		public int? MaxOptions { get; set; }
	}
}
