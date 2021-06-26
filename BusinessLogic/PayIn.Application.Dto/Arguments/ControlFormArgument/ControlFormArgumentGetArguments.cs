using PayIn.Common;
using Xp.Common.Dto.Arguments;

namespace PayIn.Application.Dto.Arguments.ControlFormArgument
{
    public class ControlFormArgumentGetArguments : IArgumentsBase
    {
		public int Id { get; set; }
		public ControlFormArgumentType   Type         { get; set; }

		#region Constructors
		public ControlFormArgumentGetArguments(int id, ControlFormArgumentType type)
		{
			Id = id;
			Type = type;
		}
		#endregion Constructors
    }
}
