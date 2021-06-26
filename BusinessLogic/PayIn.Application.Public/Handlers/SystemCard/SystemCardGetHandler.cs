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
    public class SystemCardGetHandler :
        IQueryBaseHandler<SystemCardGetArguments, SystemCardGetResult>
    {
        [Dependency] public IEntityRepository<SystemCard> Repository { get; set; }

		[Dependency] public ISessionData SessionData { get; set; }

        #region ExecuteAsync
        public async Task<ResultBase<SystemCardGetResult>> ExecuteAsync(SystemCardGetArguments arguments)
		{

			// SystemCards
			var systemCards = (await Repository.GetAsync())
			.Where(x =>
				(
					(SessionData.Roles.Contains(AccountRoles.Superadministrator)) ||
					(x.ConcessionOwner.Supplier.Login == SessionData.Login) ||
					(x.SystemCardMembers
						.Any(z => z.Login == SessionData.Login)
					)
				) && (
					x.Id == arguments.Id
				)
			);

			var res = systemCards
				.Select(x => new
				{
					Id = x.Id,
					Name = x.Name,
					ConcessionOwnerId = x.ConcessionOwnerId,
					Owner = x.ConcessionOwner.Name,
					ProfileId = x.ProfileId,
					CardType = x.CardType,
					NumberGenerationType = x.NumberGenerationType,
					MembersCount = x.SystemCardMembers.Count(),
					PhotoUrl = x.PhotoUrl,
					AffiliationEmailBody = x.AffiliationEmailBody,
					SynchronizationInterval = x.SynchronizationInterval,
					ClientId = x.ClientId
				}).ToList();

			var result = res
				.Select(x => new SystemCardGetResult {
					Id = x.Id,
					Name = x.Name,
					ConcessionOwnerId = x.ConcessionOwnerId,
					Owner = x.Owner,
					ProfileId = x.ProfileId,
					CardType = x.CardType.GetHashCode(),
					NumberGenerationType = x.NumberGenerationType.GetHashCode(),
					MembersCount = x.MembersCount,
					PhotoUrl = x.PhotoUrl,
					AffiliationEmailBody = x.AffiliationEmailBody,
					SynchronizationInterval = x.SynchronizationInterval,
					ClientId = x.ClientId
				}); //.ToList();

			return new ResultBase<SystemCardGetResult> { Data = result };
        }
        #endregion ExecuteAsync
    }
}
