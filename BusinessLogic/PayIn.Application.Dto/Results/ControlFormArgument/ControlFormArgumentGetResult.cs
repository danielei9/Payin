using PayIn.Common;

namespace PayIn.Application.Dto.Results.ControlFormArgument
{
	public class ControlFormArgumentGetResult
    {
		public int                       Id           { get; set; }
		public string                    Name         { get; set; }
		public string                    Observations { get; set; }
		public ControlFormArgumentType   Type         { get; set; }
		public int						 MinOptions   { get; set; }
		public int?						 MaxOptions	  { get; set; }
		public bool						 Required	  { get; set; }
		public int						 Order		  { get; set; }
    }
}
