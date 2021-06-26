using Microsoft.Practices.Unity;
using PayIn.Application.Dto.Arguments.SystemCardMember;
using PayIn.Domain.Public;
using System.Threading.Tasks;
using Xp.Application.Dto;
using Xp.Domain;

namespace PayIn.Application.Public.Handlers
{
	public class SystemCardMemberUpdateHandler : IServiceBaseHandler<SystemCardMemberUpdateArguments>
	{
		[Dependency] public IEntityRepository<SystemCardMember> Repository { get; set; }

		#region ExecuteAsync
		public async Task<dynamic> ExecuteAsync(SystemCardMemberUpdateArguments arguments)
		{
			var systemCardMember = await Repository.GetAsync(arguments.Id);

			systemCardMember.Name = arguments.Name;
			
			return systemCardMember;
		}
		#endregion ExecuteAsync
	}
}
