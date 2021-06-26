using System.ComponentModel.DataAnnotations;
using PayIn.Common;
using Xp.Common.Dto.Arguments;
using Xp.Domain.Transport;

namespace PayIn.Application.Dto.Arguments.SystemCardMember
{
	public class SystemCardMemberUpdateArguments : IArgumentsBase
	{
																public int Id { get; set; }
		[Display(Name = "resources.systemCardMember.name")]		public string Name { get; set; }
		[Display(Name = "resources.systemCardMember.email")]	public string Email { get; set; }

		#region Constructor
		public SystemCardMemberUpdateArguments(int id, string name)
		{
			Id = id;
			Name = name;
		}
		#endregion Constructor
	}
}
