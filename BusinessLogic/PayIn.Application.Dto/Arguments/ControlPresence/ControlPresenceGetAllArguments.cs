using Xp.Common.Dto.Arguments;

namespace PayIn.Application.Dto.Arguments.ControlPresence
{
	public partial class ControlPresenceGetAllArguments : IArgumentsBase
	{
		public string Filter     { get; set; }
		public int    TagId      { get; set; }
		public int    ItemId     { get; set; }
		public int    PlanningId { get; set; }

		#region Constructors
		public ControlPresenceGetAllArguments(string filter, int tagId, int itemId, int planningId)
		{
			Filter = filter ?? "";
			TagId = tagId;
			ItemId = itemId;
			PlanningId = planningId;
		}
		#endregion Constructors
	}
}
