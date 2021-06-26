using Xp.Common.Dto.Arguments;

namespace PayIn.Application.Dto.Arguments.ControlItem
{
	public partial class ControlItemGetSelectorArguments : IArgumentsBase
	{
		public string Filter { get; private set; }

		#region Constructors
		public ControlItemGetSelectorArguments(string filter)
		{
			Filter = filter ?? "";
		}
		#endregion Constructors
	}
}

