using Xp.Common.Dto.Arguments;

namespace PayIn.Application.Dto.Arguments.ControlFormAssign
{
	public partial class ControlFormAssignGetArguments : IArgumentsBase
	{
		public int Id { get; set; }

		#region Constructors
		public ControlFormAssignGetArguments(int id)
		{
			Id = id;
		}
		public ControlFormAssignGetArguments()
		{
		}
		#endregion Constructors
	}
}
