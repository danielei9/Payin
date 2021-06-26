using PayIn.Common.Resources;
using System.ComponentModel.DataAnnotations;
using Xp.Common;
using Xp.Common.Dto.Arguments;

namespace PayIn.Application.Dto.Arguments.ControlPlanning
{
    public partial class ControlPlanningUpdateArguments : IArgumentsBase
    {
		                                                          [Required] public int        Id            { get; private set; }
		[Display(Name="resources.controlPlanning.checkDuration")] [Required] public XpDuration CheckDuration { get; private set; }

		#region Constructors
		public ControlPlanningUpdateArguments(int id, XpDuration checkDuration)
		{
			Id            = id;
			CheckDuration = checkDuration;
		}
		#endregion Constructors
	}
}
