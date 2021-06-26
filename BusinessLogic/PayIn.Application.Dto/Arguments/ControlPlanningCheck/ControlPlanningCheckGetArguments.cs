using Xp.Common.Dto.Arguments;

namespace PayIn.Application.Dto.Arguments.ControlPlanningCheck
{
	public partial class ControlPlanningCheckGetArguments : IArgumentsBase
	{
		public int Id { get; private set; }

		#region Constructors
		public ControlPlanningCheckGetArguments(int id)
		{
			Id = id;
		}
		#endregion Constructors
	}
}
