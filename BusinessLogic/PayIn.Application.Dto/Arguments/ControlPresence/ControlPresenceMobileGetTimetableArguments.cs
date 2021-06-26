using Xp.Common;
using Xp.Common.Dto.Arguments;

namespace PayIn.Application.Dto.Arguments.ControlPresence
{
	public partial class ControlPresenceMobileGetTimetableArguments : IArgumentsBase
	{
		public XpDate Since { get; private set; }
		public XpDate Until { get; private set; }

		#region Constructors
		public ControlPresenceMobileGetTimetableArguments(XpDate since, XpDate until)
		{
			Since = since;
			Until = until;
		}
		#endregion Constructors
	}
}
