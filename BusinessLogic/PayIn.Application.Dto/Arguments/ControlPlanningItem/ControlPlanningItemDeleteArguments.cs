using Xp.Common.Dto.Arguments;

namespace PayIn.Application.Dto.Arguments.ControlPlanningItem
{
	public partial class ControlPlanningItemDeleteArguments : IArgumentsBase
	{
		public int Id { get; private set; }

		#region Constructors
		public ControlPlanningItemDeleteArguments(int id)
		{
			Id = id;
		}
		#endregion Constructors
	}
}
