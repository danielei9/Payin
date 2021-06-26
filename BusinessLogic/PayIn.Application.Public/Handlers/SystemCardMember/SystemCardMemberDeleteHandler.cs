using Microsoft.Practices.Unity;
using PayIn.Application.Dto.Arguments.SystemCardMember;
using PayIn.BusinessLogic.Common;
using PayIn.Common;
using PayIn.Domain.Public;
using PayIn.Domain.Security;
using System.Linq;
using System.Threading.Tasks;
using Xp.Application.Dto;
using Xp.Domain;

namespace PayIn.Application.Public.Handlers
{
	public class SystemCardMemberDeleteHandler :
			IServiceBaseHandler<SystemCardMemberDeleteArguments>
	{
		[Dependency] public IEntityRepository<SystemCardMember> Repository { get; set; }
		[Dependency] public ISessionData SessionData { get; set; }

		#region SystemCardMemberDelete
		async Task<dynamic> IServiceBaseHandler<SystemCardMemberDeleteArguments>.ExecuteAsync(SystemCardMemberDeleteArguments arguments)
		{

			var systemCardMember = (await Repository.GetAsync())
					.Where(x => x.Id == arguments.Id);

			if (!SessionData.Roles.Contains(AccountRoles.Superadministrator)) {
				systemCardMember = systemCardMember
					.Where(	x =>  SessionData.Login == x.SystemCard.ConcessionOwner.Supplier.Login);
			}

			systemCardMember
				.FirstOrDefault()
				.State = SystemCardMemberState.Deleted;

				return null;
		}
		#endregion SystemCardMemberDelete
	}
}
