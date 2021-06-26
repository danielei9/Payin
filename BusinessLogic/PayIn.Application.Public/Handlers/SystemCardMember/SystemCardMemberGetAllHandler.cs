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
    public class SystemCardMemberGetAllHandler :
        IQueryBaseHandler<SystemCardMemberGetAllArguments, SystemCardMemberGetAllResult>
    {
        [Dependency] public IEntityRepository<SystemCard> SystemCardRepository { get; set; }
        [Dependency] public IEntityRepository<SystemCardMember> Repository { get; set; }
        [Dependency] public IEntityRepository<ServiceConcession> ServiceConcessionRepository { get; set; }
        [Dependency] public ISessionData SessionData { get; set; }

        #region ExecuteAsync
        public async Task<ResultBase<SystemCardMemberGetAllResult>> ExecuteAsync(SystemCardMemberGetAllArguments arguments)
		{
			// SystemCards
			var systemCards = (await SystemCardRepository.GetAsync());

			if (!SessionData.Roles.Contains(AccountRoles.Superadministrator))
			{
				systemCards = systemCards
					.Where(x =>
						(x.ConcessionOwner.Supplier.Login == SessionData.Login) ||
						(x.ConcessionOwner.Supplier.Workers
							.Any(z => z.Login == SessionData.Login)
					)
				);
			}

			var systemCardsResult = systemCards
				.Select(x => new SelectorResult
					{
						Id = x.Id,
						Value = x.Name
					});
			
            var systemCard = systemCardsResult
				.Where(x => x.Id == arguments.SystemCardId)
				.FirstOrDefault();
            //var systemCardId = systemCard?.Id;

            // Items
            var items = (await Repository.GetAsync());
			if (!arguments.Filter.IsNullOrEmpty())
				items = items
                    .Where(x =>
						x.SystemCard.Name.Contains(arguments.Filter) ||
						x.Name.Contains(arguments.Filter) ||
						x.Login.Contains(arguments.Filter)
					);
			if (arguments.SystemCardId != null)
				items = items
                .Where(x =>
					x.SystemCardId == arguments.SystemCardId
				);
            var result = items
				.Where(x =>
					(x.State != Common.SystemCardMemberState.Deleted) &&
					(
						systemCards.Any(y => y.Id == arguments.SystemCardId)
					)
				)
				.Select(x => new SystemCardMemberGetAllResult
                {
                    Id = x.Id,
                    SystemCardId = x.SystemCardId,
                    SystemCardName = x.SystemCard.Name,
					State = x.State,
                    // Información comercial
                    Name = x.Name,
					Email = x.Login					
                });


            return new SystemCardMemberGetAllResultBase
            {
                Data = result,
                SystemCardId = systemCard?.Id,
                SystemCardName = systemCard?.Value ?? "",
                SystemCards = systemCardsResult
            };
        }
        #endregion ExecuteAsync
    }
}
