using Xp.Common.Dto.Arguments;

namespace PayIn.Application.Dto.Arguments.ControlFormAssignCheckPoint
{
	public partial class ControlFormAssignCheckPointGetArguments : IArgumentsBase
	{
		public int Id { get; set; }

		#region Constructors
		public ControlFormAssignCheckPointGetArguments(int id)
		{
			Id = id;
		}
		public ControlFormAssignCheckPointGetArguments()
		{
		}
		#endregion Constructors
	}
}
