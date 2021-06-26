using System.ComponentModel.DataAnnotations;
using Xp.Common;
using Xp.Common.Dto.Arguments;

namespace PayIn.Application.Dto.Arguments.ControlPlanningItem
{
	public partial class ControlPlanningItemUpdateArguments : IArgumentsBase
	{
		[Required] public int      Id    { get; private set; }
		[Required] public XpTime   Since { get; private set; }
		[Required] public XpTime   Until { get; private set; }

		#region Constructors
		public ControlPlanningItemUpdateArguments(int id, XpTime since, XpTime until)
		{
			Id = id;
			Since = since;
			Until = until;
		}
		#endregion Constructors
	}
}
