using System.ComponentModel.DataAnnotations;
using Xp.Common;
using Xp.Common.Dto.Arguments;

namespace PayIn.Application.Dto.Arguments.ControlTemplateItem
{
	public partial class ControlTemplateItemCreateArguments : IArgumentsBase
	{
		                                                                                [Required] public int    TemplateId { get; private set; }
		[Display(Name="resources.controlTemplateItem.since")] [DataType(DataType.Time)] [Required] public XpTime Since      { get; private set; }
		[Display(Name="resources.controlTemplateItem.until")] [DataType(DataType.Time)] [Required] public XpTime Until      { get; private set; }

		#region Constructors
		public ControlTemplateItemCreateArguments(int templateId, XpTime since, XpTime until)
		{
			TemplateId = templateId;
			Since = since;
			Until = until;
		}
		#endregion Constructors
	}
}
