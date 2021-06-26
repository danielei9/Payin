using System.ComponentModel.DataAnnotations;
using Xp.Common.Dto.Arguments;

namespace PayIn.Application.Dto.Arguments.SystemCardMember
{
    public class SystemCardMemberCreateArguments : IArgumentsBase
	{
		[Display(Name = "resources.systemCardMember.systemCard")]	[Required]	public int    SystemCardId { get; set; }
		[Display(Name = "resources.systemCardMember.name")]			[Required]	public string Name { get; set; }
		[Display(Name = "resources.systemCardMember.login")]		[Required]	public string Login { get; set; }
		[Display(Name = "resources.systemCardMember.canEmit")]		            public bool   CanEmit      { get; set; }
		
		#region Constructors
		public SystemCardMemberCreateArguments(int systemcardId, string name, string login, bool canEmit)
		{
			SystemCardId = systemcardId;
			Name = name;
			Login = login;
			CanEmit = canEmit;
		}
		#endregion Constructors
	}
}
