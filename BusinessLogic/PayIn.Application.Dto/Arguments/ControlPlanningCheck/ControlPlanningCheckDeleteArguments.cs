using Xp.Common.Dto.Arguments;

namespace PayIn.Application.Dto.Arguments.ControlPlanningCheck
{
	public partial class ControlPlanningCheckDeleteArguments : IArgumentsBase
	{
		public int Id { get; private set; }

		#region Constructors
		public ControlPlanningCheckDeleteArguments(int id)
		{
			Id = id;
		}
		#endregion Constructors
	}
}
