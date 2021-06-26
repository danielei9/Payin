using Microsoft.Practices.Unity;
using PayIn.Application.Dto.Arguments.SystemCardMember;
using PayIn.Application.Dto.Results.SystemCardMember;
using PayIn.BusinessLogic.Common;
using PayIn.Domain.Public;
using PayIn.Domain.Security;
using System;
using System.Linq;
using System.Threading.Tasks;
using Xp.Application.Dto;
using Xp.Domain;

namespace PayIn.Application.Public.Handlers
{
    public class SystemCardMemberGetHandler :
        IQueryBaseHandler<SystemCardMemberGetArguments, SystemCardMemberGetResult>
    {
		[Dependency] public IEntityRepository<SystemCard> SystemCardRepository { get; set; }
		[Dependency] public IEntityRepository<SystemCardMember> Repository { get; set; }
        [Dependency] public ISessionData SessionData { get; set; }

        #region ExecuteAsync
        public async Task<ResultBase<SystemCardMemberGetResult>> ExecuteAsync(SystemCardMemberGetArguments arguments)
		{

			if (!SessionData.Roles.Contains(AccountRoles.Superadministrator))
			{
				// SystemCards
				var systemCards = (await SystemCardRepository.GetAsync());

				var isAuthorized = systemCards
					.Where(x =>
						((x.ConcessionOwner.Supplier.Login == SessionData.Login) ||
						(x.ConcessionOwner.Supplier.Workers
							.Any(z => z.Login == SessionData.Login)
						)) &&
						(x.SystemCardMembers.Any(z => z.Id == arguments.Id))
					)
					.Any();
				if (!isAuthorized)
					throw new UnauthorizedAccessException();
			}

            // Items
            var items = (await Repository.GetAsync());
				items = items
                .Where(x =>
					x.Id == arguments.Id
				);

            var result = items
				.Select(x => new SystemCardMemberGetResult
                {
                    Id = x.Id,
                    SystemCardId = x.SystemCardId,
                    SystemCardName = x.SystemCard.Name,
					State = x.State,
                    // Información comercial
                    Name = x.Name,
					Email = x.Login					
                });

			return new ResultBase<SystemCardMemberGetResult> { Data = result };
			
        }
        #endregion ExecuteAsync
    }
}
