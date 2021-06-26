using PayIn.Common.Resources;
using System.ComponentModel.DataAnnotations;
using Xp.Application.Dto;
using Xp.Common;
using Xp.Common.Dto.Arguments;

namespace PayIn.Application.Dto.Arguments.ControlPlanningCheck
{
	public partial class ControlPlanningCheckCreateArguments : IArgumentsBase
	{
		                                                                            [Required] public int        WorkerId      { get; private set; }
		[Display(Name="resources.controlPlanning.item")]       [Required] public int        ItemId        { get; private set; }
		[Display(Name="resources.controlPlanning.dateSince")]  [Required] public XpDate     DateSince     { get; private set; }
		[Display(Name="resources.controlPlanning.dateUntil")]  [Required] public XpDate     DateUntil     { get; private set; }
		[Display(Name="resources.controlPlanning.time")]       [Required] public XpTime     Time          { get; private set; }
		[Display(Name="resources.controlPlanning.checkPoint")] [Required] public int        CheckPointId  { get; private set; }

		#region Constructors
		public ControlPlanningCheckCreateArguments(int workerId, int itemId, XpDate dateSince, XpDate dateUntil, XpTime time, int checkPointId)
		{
			WorkerId = workerId;
			ItemId = itemId;
			DateSince = dateSince;
			DateUntil = dateUntil;
			Time = time;
			CheckPointId = checkPointId;
		}
		#endregion Constructors
	}
}
