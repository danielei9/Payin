using PayIn.Common.Resources;
using System.ComponentModel.DataAnnotations;
using Xp.Common;
using Xp.Common.Dto.Arguments;

namespace PayIn.Application.Dto.Arguments.ControlPlanningItem
{
	public partial class ControlPlanningItemCreateArguments : IArgumentsBase
	{
		                                                      [Required] public int        WorkerId      { get; private set; }
		[Display(Name="resources.controlPlanning.dateSince")] [Required] public XpDate     DateSince     { get; private set; }
		[Display(Name="resources.controlPlanning.dateUntil")] [Required] public XpDate     DateUntil     { get; private set; }
		[Display(Name="resources.controlPlanning.item")]      [Required] public int        ItemId        { get; private set; }
		[Display(Name="resources.controlPlanning.timeSince")] [Required] public XpTime     TimeSince     { get; private set; }
		[Display(Name="resources.controlPlanning.timeUntil")] [Required] public XpTime     TimeUntil     { get; private set; }

		#region Constructors
		public ControlPlanningItemCreateArguments(XpDate dateSince, XpDate dateUntil, XpTime timeSince, XpTime timeUntil, int itemId, int workerId)
		{
			DateSince     = dateSince;
			DateUntil     = dateUntil;
			TimeSince     = timeSince;
			TimeUntil     = timeUntil;
			ItemId        = itemId;
			WorkerId      = workerId;
		}
		#endregion Constructors
	}
}
