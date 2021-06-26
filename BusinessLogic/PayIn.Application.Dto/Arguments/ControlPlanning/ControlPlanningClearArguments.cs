using PayIn.Common.Resources;
using System.ComponentModel.DataAnnotations;
using Xp.Common;
using Xp.Common.Dto.Arguments;

namespace PayIn.Application.Dto.Arguments.ControlPlanning
{
	public partial class ControlPlanningClearArguments : IArgumentsBase
	{
		                                                      [Required] public int     WorkerId { get; private set; }
		[Display(Name="resources.controlPlanning.dateSince")] [Required] public XpDate  Since    { get; private set; }
		[Display(Name="resources.controlPlanning.dateUntil")] [Required] public XpDate  Until    { get; private set; }
		[Display(Name="resources.controlPlanning.item")]      [Required] public int     ItemId   { get; private set; }

		#region Constructors
		public ControlPlanningClearArguments(XpDate since, XpDate until, int itemId, int workerId)
		{
			Since    = since;
			Until    = until;
			ItemId   = itemId;
			WorkerId = workerId;
		}
		#endregion Constructors
	}
}
