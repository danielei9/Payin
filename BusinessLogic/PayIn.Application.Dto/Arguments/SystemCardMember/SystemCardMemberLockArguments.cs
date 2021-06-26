using Xp.Common.Dto.Arguments;

namespace PayIn.Application.Dto.Arguments.SystemCardMember
{
	public partial class SystemCardMemberLockArguments : IArgumentsBase
	{
		public int Id { get; set; }

		#region Constructors
		public SystemCardMemberLockArguments(int id)
		{
			Id = id;
		}
		#endregion Constructors

	}
}
