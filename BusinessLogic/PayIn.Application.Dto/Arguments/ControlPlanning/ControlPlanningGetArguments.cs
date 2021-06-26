using Xp.Common.Dto.Arguments;

namespace PayIn.Application.Dto.Arguments.ControlPlanning
{
	public partial class ControlPlanningGetArguments : IArgumentsBase
	{
		public int Id { get; private set; }

		#region Constructors
		public ControlPlanningGetArguments(int id)
		{
			Id = id;
		}
		#endregion Constructors
	}
}
