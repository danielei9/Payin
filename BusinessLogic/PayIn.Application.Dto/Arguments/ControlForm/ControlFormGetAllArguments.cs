using Xp.Common.Dto.Arguments;

namespace PayIn.Application.Dto.Arguments.ControlForm
{
    public class ControlFormGetAllArguments : IArgumentsBase
    {
		public string Filter { get; set; }

		#region Constructors
		public ControlFormGetAllArguments(string filter)
		{
			Filter = filter ?? "";
		}
		#endregion Constructors
    }
}
