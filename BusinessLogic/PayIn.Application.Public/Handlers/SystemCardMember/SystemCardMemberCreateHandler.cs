using Microsoft.Practices.Unity;
using PayIn.Application.Dto.Arguments.SystemCardMember;
using PayIn.BusinessLogic.Common;
using PayIn.Common;
using PayIn.Domain.Public;
using PayIn.Domain.Security;
using PayIn.Infrastructure.Security;
using System;
using System.Linq;
using System.Threading.Tasks;
using Xp.Application.Dto;
using Xp.Domain;


namespace PayIn.Application.Public.Handlers
{
	public class SystemCardMemberCreateHandler : IServiceBaseHandler<SystemCardMemberCreateArguments>
    {
        [Dependency] public IEntityRepository<ServiceConcession> ServiceConcessionRepository { get; set; }
		[Dependency] public IEntityRepository<SystemCard> SystemCardRepository { get; set; }
		[Dependency] public IEntityRepository<SystemCardMember> SystemCardMemberRepository { get; set; }
		[Dependency] public ISessionData SessionData { get; set; }

		#region ExecuteAsync
		public async Task<dynamic> ExecuteAsync(SystemCardMemberCreateArguments arguments)
		{
			var systemCard = (await SystemCardRepository.GetAsync())
				.Where( x => 
					x.Id == arguments.SystemCardId
				);

			if (!SessionData.Roles.Contains(AccountRoles.Superadministrator))
			{
				systemCard = systemCard
					.Where(x =>
					x.ConcessionOwner.Supplier.Login == SessionData.Login
				);
			}

			var result = systemCard.FirstOrDefault();

			if (result == null)
				throw new ArgumentNullException("SystemCardId");

			var systemCardMember = (await SystemCardMemberRepository.GetAsync())
				.Where(x =>
					   x.SystemCardId == arguments.SystemCardId &&
					   x.Login == arguments.Login &&
					   (x.State == SystemCardMemberState.Active || x.State == SystemCardMemberState.Pending)
					).FirstOrDefault();

			if (systemCardMember != null)
			{
				throw new ApplicationException("This login is already registered on this System Card");
			}

			systemCardMember = new SystemCardMember
				{
					SystemCard = result,
					CanEmit = arguments.CanEmit,
					Name = arguments.Name,
					Login = arguments.Login,
					State = Common.SystemCardMemberState.Pending
				};

			await SystemCardMemberRepository.AddAsync(systemCardMember);
			
				
            var exist = (await ServiceConcessionRepository.GetAsync())
                .Any(x =>
                    x.Supplier.Login == arguments.Login &&
                    x.Type == Common.ServiceType.Charge
                );
            if (!exist)
			{
				// Proceso de invitar a compañia
				var securityRepository = new SecurityRepository();
				var userModel = new SystemCardMemberCreateArguments
				(
					arguments.SystemCardId,
					arguments.Name,
					arguments.Login,
					arguments.CanEmit
				);
				await securityRepository.InviteSystemCardMemberAsync(userModel);
			} else
			{
				// Acivar el miembro
				systemCardMember.State = SystemCardMemberState.Active;
			}

            return systemCardMember;
		}
		#endregion ExecuteAsync
	}
}

