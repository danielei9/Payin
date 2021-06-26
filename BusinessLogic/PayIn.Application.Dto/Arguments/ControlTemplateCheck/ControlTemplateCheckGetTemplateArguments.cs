using Xp.Common.Dto.Arguments;


namespace PayIn.Application.Dto.Arguments.ControlTemplateCheck
{
	public partial class ControlTemplateCheckGetTemplateArguments : IArgumentsBase
    {
	   public int TemplateId { get; private set; }

		#region Constructors
	   public ControlTemplateCheckGetTemplateArguments(int templateId)  
		{
			TemplateId = templateId;
		}
		#endregion Constructors
	}
}
