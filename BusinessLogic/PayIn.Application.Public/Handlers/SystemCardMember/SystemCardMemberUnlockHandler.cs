using Microsoft.Practices.Unity;
using PayIn.Application.Dto.Arguments.SystemCardMember;
using PayIn.BusinessLogic.Common;
using PayIn.Common;
using PayIn.Domain.Public;
using System;
using System.Linq;
using System.Threading.Tasks;
using Xp.Application.Dto;
using Xp.Domain;
using PayIn.Domain.Security;


namespace PayIn.Application.Public.Handlers
{
	public class SystemCardMemberUnlockHandler :
		IServiceBaseHandler<SystemCardMemberUnlockArguments>
	{
		[Dependency] public IEntityRepository<SystemCardMember> Repository { get; set; }
		[Dependency] public ISessionData SessionData { get; set; }

		#region ExecuteAsync
		public async Task<dynamic> ExecuteAsync(SystemCardMemberUnlockArguments arguments)
		{
			var systemCardMember = (await Repository.GetAsync())
					.Where(x =>
						 (x.Id == arguments.Id) &&
						 (
							(SessionData.Roles.Contains(AccountRoles.Superadministrator)) ||
							(
								x.SystemCard.ConcessionOwner.Supplier.Login == SessionData.Login ||
								x.Login == SessionData.Login
							)
						 )

					)
					.FirstOrDefault();


			if (systemCardMember == null)
				throw new ApplicationException("Ha ocurrido un error en el bloqueo del miembro");


			systemCardMember.State = SystemCardMemberState.Active;

			return systemCardMember;
		}
		#endregion ExecuteAsync
	}
}
