using Xp.Common.Dto.Arguments;

namespace PayIn.Application.Dto.Arguments.ControlPresence
{
	public partial class ControlPresenceDeleteArguments : IArgumentsBase
	{
		public int Id { get; private set; }

		#region Constructors
		public ControlPresenceDeleteArguments(int id)
		{
			Id = id;
		}
		#endregion Constructors
	}
}
