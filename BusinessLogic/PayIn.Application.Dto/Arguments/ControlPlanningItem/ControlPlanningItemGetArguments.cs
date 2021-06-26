using Xp.Common.Dto.Arguments;

namespace PayIn.Application.Dto.Arguments.ControlPlanningItem
{
	public partial class ControlPlanningItemGetArguments : IArgumentsBase
	{
		public int Id { get; private set; }

		#region Constructors
		public ControlPlanningItemGetArguments(int id)
		{
			Id = id;
		}
		#endregion Constructors
	}
}
