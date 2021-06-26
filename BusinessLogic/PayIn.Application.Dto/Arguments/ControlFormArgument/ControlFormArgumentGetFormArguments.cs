using Xp.Common.Dto.Arguments;

namespace PayIn.Application.Dto.Arguments.ControlFormArgument
{
  public class ControlFormArgumentGetFormArguments : IArgumentsBase
  {
		public int    FormId { get; set; }
		public string Filter { get; set; }

		#region Constructors
		public ControlFormArgumentGetFormArguments(int formId, string filter)
		{
			FormId = formId;
			Filter = filter ?? "";
		}
		#endregion Constructors
  }
}
