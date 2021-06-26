using Xp.Common.Dto.Arguments;

namespace PayIn.Application.Dto.Arguments.ControlPlanningCheck
{
	public class ControlPlanningCheckMobileGetArguments : IArgumentsBase
	{
		public int ItemId { get; set; }
		public int? PlanningItemId { get; set; }
		public int? PlanningCheckId { get; set; }

		#region Constructors
		public ControlPlanningCheckMobileGetArguments(int itemId, int? planningItemId, int? planningCheckId)
		{
			ItemId = itemId;
			PlanningItemId = planningItemId;
			PlanningCheckId = planningCheckId;
		}
		#endregion Constructors
	}
}
