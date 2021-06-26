using Microsoft.Practices.Unity;
using PayIn.Application.Dto.Arguments.ServiceConcession;
using PayIn.Application.Dto.Results.ServiceConcession;
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
    public class ServiceConcessionGetSelectorHandler :
		IQueryBaseHandler<ServiceConcessionGetSelectorArguments, ServiceConcessionGetSelectorResult>
	{
		[Dependency] public ISessionData SessionData { get; set; }
		[Dependency] public IEntityRepository<ServiceConcession> Repository { get; set; }
		[Dependency] public IEntityRepository<SystemCardMember> SystemCardMemberRepository { get; set; }

		#region ExecuteAsync
		public async Task<ResultBase<ServiceConcessionGetSelectorResult>> ExecuteAsync(ServiceConcessionGetSelectorArguments arguments)
		{
            var systemCardMembers = (await SystemCardMemberRepository.GetAsync())
                .Where(y => y.CanEmit);
            if (arguments.SystemCardId != null)
                systemCardMembers = systemCardMembers
                    .Where(x => x.SystemCardId == arguments.SystemCardId);

            var items = (await Repository.GetAsync());
            if (!arguments.Filter.IsNullOrEmpty())
                items = items
                .Where(x =>
                    x.Name.Contains(arguments.Filter)
                );

			if (!SessionData.Roles.Contains(AccountRoles.Superadministrator)) {
				items = items
				.Where(x =>
					systemCardMembers
					.Any(y =>
						y.Login == x.Supplier.Login ||
						y.SystemCard.SystemCardMembers
							.Any(z => z.Login == SessionData.Login)
					)
				);
			}

			var result = items
				.Select(x => new ServiceConcessionGetSelectorResult
				{
					Id = x.Id,
					Value = x.Name
				});

			return new ResultBase<ServiceConcessionGetSelectorResult> { Data = result };
		}
		#endregion ExecuteAsync
	}
}
