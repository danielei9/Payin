using Xp.Common.Dto.Arguments;

namespace PayIn.Application.Dto.Arguments.SystemCardMember
{
	public class SystemCardMemberDeleteArguments : IArgumentsBase
	{
		public int Id { set; get; }

		#region Constructors
		public SystemCardMemberDeleteArguments(int id)
		{
			Id = id;
		}
		#endregion Constructors 
	}
}
