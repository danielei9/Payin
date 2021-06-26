using PayIn.Common.Resources;
using System.ComponentModel.DataAnnotations;
using Xp.Common;
using Xp.Common.Dto.Arguments;

namespace PayIn.Application.Dto.Arguments.ControlPlanning
{
	public partial class ControlPlanningCreateArguments : IArgumentsBase
	{
		                                                          [Required] public int        WorkerId      { get; private set; }
		[Display(Name="resources.controlPlanning.item")]          [Required] public int        ItemId        { get; private set; }
		[Display(Name="resources.controlPlanning.dateSince")]     [Required] public XpDate     DateSince     { get; private set; }
		[Display(Name="resources.controlPlanning.dateUntil")]     [Required] public XpDate     DateUntil     { get; private set; }
		[Display(Name="resources.controlPlanning.checkDuration")]            public XpDuration CheckDuration { get; private set; }

		#region Constructors
		public ControlPlanningCreateArguments(int workerId, int itemId, XpDate dateSince, XpDate dateUntil, XpDuration checkDuration)
		{
			WorkerId = workerId;
			ItemId = itemId;
			DateSince = dateSince;
			DateUntil = dateUntil;
			CheckDuration = 
				checkDuration == null ? null :
				checkDuration.Value.TotalMinutes == 0 ? null :
			  checkDuration;
		}
		#endregion Constructors
	}
}
