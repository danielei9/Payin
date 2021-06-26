using Microsoft.Practices.Unity;
using PayIn.Application.Dto.Arguments;
using PayIn.Domain.Public;
using PayIn.Domain.Transport;
using System;
using System.Linq;
using System.Threading.Tasks;
using Xp.Application.Dto;
using Xp.Domain;

namespace PayIn.Application.Public.Handlers
{
    public class ServiceCardUnlockHandler :
		IServiceBaseHandler<ServiceCardUnlockArguments>
	{
		[Dependency] public IEntityRepository<ServiceCard> Repository { get; set; }
		[Dependency] public IEntityRepository<BlackList> BlackListRepository { get; set; }

		#region ExecuteAsync
		public async Task<dynamic> ExecuteAsync(ServiceCardUnlockArguments arguments)
        {
            var now = DateTime.Now.ToUTC();

            var cards = (await Repository.GetAsync())
                .Where(x => x.Id == arguments.Id);
            var blackList = (await BlackListRepository.GetAsync())
                .Where(x =>
                    x.State == BlackList.BlackListStateType.Active &&
                    !x.Resolved &&
                    cards
                        .Any(y => 
                            x.Uid == y.Uid &&
                            x.SystemCardId == y.SystemCardId
                        )
                );

            foreach(var item in blackList)
            {
                item.Resolved = true;
                item.ResolutionDate = now;
            }

            return null;
        }
		#endregion ExecuteAsync
	}
}
