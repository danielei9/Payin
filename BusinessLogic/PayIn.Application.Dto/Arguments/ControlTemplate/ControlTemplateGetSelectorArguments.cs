using Xp.Common.Dto.Arguments;

namespace PayIn.Application.Dto.Arguments.ControlTemplate
{
	public partial class ControlTemplateGetSelectorArguments : IArgumentsBase
	{
		public string Filter { get; private set; }

		#region Constructors
		public ControlTemplateGetSelectorArguments(string filter)
		{
			Filter = filter;
		}
		#endregion Constructors
	}
}
