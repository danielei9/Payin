using PayIn.Common.Resources;
using System.ComponentModel.DataAnnotations;
using Xp.Common;
using Xp.Common.Dto.Arguments;

namespace PayIn.Application.Dto.Arguments.ControlTemplateCheck
{
	public partial class ControlTemplateCheckCreateArguments  : IArgumentsBase
	{
		[Display(Name="resources.controlPlanning.template")]   [Required] public int    TemplateId    { get; private set; }
		[Display(Name="resources.controlPlanning.time")]                  public XpTime Time          { get; private set; }
		[Display(Name="resources.controlPlanning.checkPoint")] [Required] public int    CheckPointId  { get; private set; }

		#region Constructors
		public ControlTemplateCheckCreateArguments(XpTime time, int templateId, int checkPointId)
		{
			Time          = time;
			TemplateId    = templateId;
			CheckPointId  = checkPointId;
		}
		#endregion Constructors
	}
}
