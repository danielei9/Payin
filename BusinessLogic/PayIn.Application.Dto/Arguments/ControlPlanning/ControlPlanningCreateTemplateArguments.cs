using PayIn.Common.Resources;
using System.ComponentModel.DataAnnotations;
using Xp.Common;
using Xp.Common.Dto.Arguments;

namespace PayIn.Application.Dto.Arguments.ControlPlanning
{
	public partial class ControlPlanningCreateTemplateArguments : IArgumentsBase
	{
		                                                                   [Required] public int        WorkerId      { get; private set; }
		[Display(Name="resources.controlPlanning.dateSince")]              [Required] public XpDate     DateSince     { get; private set; }
		[Display(Name="resources.controlPlanning.dateUntil")]              [Required] public XpDate     DateUntil     { get; private set; }
		[Display(Name="resources.controlPlanning.template")]               [Required] public int        TemplateId    { get; private set; }

		#region Constructors
		public ControlPlanningCreateTemplateArguments(XpDate dateSince, XpDate dateUntil, XpTime timeSince, XpTime timeUntil, int workerId, int templateId)
		{
			DateSince     = dateSince;
			DateUntil     = dateUntil;
			WorkerId      = workerId;
			TemplateId    = templateId;
		}
		#endregion Constructors
	}
}
