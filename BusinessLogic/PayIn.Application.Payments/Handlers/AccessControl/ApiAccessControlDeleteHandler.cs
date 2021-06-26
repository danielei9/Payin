using Microsoft.Practices.Unity;
using PayIn.Application.Dto.Payments.Arguments;
using PayIn.Domain.Payments;
using System.Linq;
using System.Threading.Tasks;
using Xp.Application.Dto;
using Xp.Domain;

namespace PayIn.Application.Payments.Handlers
{
    public class ApiAccessControlDeleteHandler :
        IServiceBaseHandler<ApiAccessControlDeleteArguments>
    {
        [Dependency] public IEntityRepository<AccessControl> Repository { get; set; }
        [Dependency] public IEntityRepository<AccessControlEntrance> RepositoryEntrance { get; set; }
        [Dependency] public IEntityRepository<AccessControlEntry> RepositoryEntry { get; set; }

        #region ExecuteAsync

        public async Task<dynamic> ExecuteAsync(ApiAccessControlDeleteArguments arguments)
        {
            var item = (await Repository.GetAsync())
                .Where(x => x.Id == arguments.Id)
                .FirstOrDefault();

            var entrances = (await RepositoryEntrance.GetAsync())
                .Where(x => x.AccessControlId == arguments.Id);

            foreach (var entrance in entrances)
            {
                var entries = (await RepositoryEntry.GetAsync())
                .Where(x => x.AccessControlEntranceId == entrance.Id);

                foreach (var entry in entries)
                {
                    await RepositoryEntry.DeleteAsync(entry);
                }
                await RepositoryEntrance.DeleteAsync(entrance);
            }

            await Repository.DeleteAsync(item);

            return null;
        }

        #endregion
    }
}
