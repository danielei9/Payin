using Microsoft.Practices.Unity;
using PayIn.Application.Dto.Payments.Arguments;
using PayIn.Domain.Payments;
using System.Linq;
using System.Threading.Tasks;
using Xp.Application.Dto;
using Xp.Domain;

namespace PayIn.Application.Payments.Handlers
{
    public class ApiAccessControlEntranceDeleteHandler :
        IServiceBaseHandler<ApiAccessControlEntranceDeleteArguments>
    {
        [Dependency] public IEntityRepository<AccessControlEntrance> Repository { get; set; }
        [Dependency] public IEntityRepository<AccessControlEntry> RepositoryEntry { get; set; }

        #region ExecuteAsync

        public async Task<dynamic> ExecuteAsync(ApiAccessControlEntranceDeleteArguments arguments)
        {
            var item = (await Repository.GetAsync())
                .Where(x => x.Id == arguments.Id)
                .FirstOrDefault();

            var entries = (await RepositoryEntry.GetAsync())
                .Where(x => x.AccessControlEntranceId == arguments.Id);

            foreach (var entry in entries)
            {
                await RepositoryEntry.DeleteAsync(entry);
            }

            await Repository.DeleteAsync(item);

            return null;
        }

        #endregion
    }
}
