using Xp.Common.Dto.Arguments;

namespace PayIn.Application.Dto.Arguments.ControlPresence
{
	public partial class ControlPresenceMobileGetTagArguments : IArgumentsBase
	{
		public string Reference { get; private set; }

		#region Constructors
		public ControlPresenceMobileGetTagArguments(string reference)
		{
			Reference = reference;
		}
		#endregion Constructors
	}
}
