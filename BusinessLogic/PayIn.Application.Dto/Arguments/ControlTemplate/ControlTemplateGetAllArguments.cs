using Xp.Common.Dto.Arguments;

namespace PayIn.Application.Dto.Arguments.ControlTemplate
{
	public partial class ControlTemplateGetAllArguments : IArgumentsBase
	{
		public string Filter { get; private set; }

		#region Constructors
		public ControlTemplateGetAllArguments(string filter)
		{
			Filter = filter ?? "";
		}
		#endregion Constructors
	}
}
