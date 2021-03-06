using Xp.Common;
using Xp.Common.Dto.Arguments;

namespace PayIn.Application.Dto.Arguments.ControlPlanning
{
	public partial class ControlPlanningGetAllArguments : IArgumentsBase
	{
		public int    WorkerId { get; private set; }
		public XpDate Since    { get; private set; }
		public XpDate Until    { get; private set; }

		#region Constructors
		public ControlPlanningGetAllArguments(int workerId, XpDate since, XpDate until)
		{
			WorkerId = workerId;
			Since = since;
			Until = until;
		}
		#endregion Constructors
	}
}
