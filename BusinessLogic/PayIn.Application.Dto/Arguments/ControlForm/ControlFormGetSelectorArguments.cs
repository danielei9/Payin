using Xp.Common.Dto.Arguments;

namespace PayIn.Application.Dto.Arguments.ControlForm
{
	public partial class ControlFormGetSelectorArguments : IArgumentsBase
	{
		public string Filter { get; private set; }

		#region Constructors
		public ControlFormGetSelectorArguments(string filter)
		{
			Filter = filter;
		}
		#endregion Constructors
	}
}

