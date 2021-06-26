using Microsoft.Practices.Unity;
using PayIn.Application.Dto.Arguments.SystemCard;
using PayIn.Application.Dto.Results.SystemCard;
using PayIn.BusinessLogic.Common;
using PayIn.Domain.Public;
using PayIn.Domain.Security;
using System.Linq;
using System.Threading.Tasks;
using Xp.Application.Dto;
using Xp.Domain;

namespace PayIn.Application.Public.Handlers
{
    public class SystemCardGetAllHandler :
        IQueryBaseHandler<SystemCardGetAllArguments, SystemCardGetAllResult>
    {
        [Dependency] public IEntityRepository<SystemCard> Repository { get; set; }

		[Dependency] public ISessionData SessionData { get; set; }

        #region ExecuteAsync
        public async Task<ResultBase<SystemCardGetAllResult>> ExecuteAsync(SystemCardGetAllArguments arguments)
		{

			// SystemCards
			var systemCards = (await Repository.GetAsync());

			if (!SessionData.Roles.Contains(AccountRoles.Superadministrator) )
			{
				// SystemCards
				systemCards = systemCards
				.Where(x =>
					(x.ConcessionOwner.Supplier.Login == SessionData.Login) ||
					(x.SystemCardMembers
						.Any(z => z.Login == SessionData.Login)
					)
				);
			}
         		
            var result = systemCards
				.Select(x => new 
                {
                    Id = x.Id,
					Name = x.Name,
					Owner = x.ConcessionOwner.Name,
					Profile = x.Profile.Name,
					CardType = x.CardType,
					NumberGenerationType = x.NumberGenerationType,
					MembersCount = x.SystemCardMembers
										.Where(y => y.State != Common.SystemCardMemberState.Deleted)
										.Count()
                }).ToList()
				.Select( x => new SystemCardGetAllResult {
					Id = x.Id,
					Name = x.Name,
					Owner = x.Owner,
					Profile = x.Profile,
					CardType = x.CardType.ToString(),
					NumberGenerationType = x.NumberGenerationType.ToString(),
					MembersCount = x.MembersCount
				});

			return new ResultBase<SystemCardGetAllResult> { Data = result };
		}
		#endregion ExecuteAsync
	}
}
