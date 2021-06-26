using Xp.Common.Dto.Arguments;

namespace PayIn.Application.Dto.Arguments.SystemCardMember
{
	public partial class SystemCardMemberUnlockArguments : IArgumentsBase
	{
		public int Id { get; set; }

		#region Constructors
		public SystemCardMemberUnlockArguments(int id)
		{
			Id = id;
		}
		#endregion Constructors
	}
}
