using Xp.Common.Dto.Arguments;

namespace PayIn.Application.Dto.Arguments.ControlTemplateItem
{
	public partial class ControlTemplateItemGetTemplateArguments : IArgumentsBase
	{
		public int TemplateId { get; private set; }

		#region Constructors
		public ControlTemplateItemGetTemplateArguments(int templateId)
		{
			TemplateId = templateId;
		}
		#endregion Constructors
	}
}
