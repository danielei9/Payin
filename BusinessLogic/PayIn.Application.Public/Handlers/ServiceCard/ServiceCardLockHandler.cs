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
    public class ServiceCardLockHandler :
		IServiceBaseHandler<ServiceCardLockArguments>
	{
		[Dependency] public IEntityRepository<ServiceCard> Repository { get; set; }
		[Dependency] public IEntityRepository<BlackList> BlackListRepository { get; set; }

		#region ExecuteAsync
		public async Task<dynamic> ExecuteAsync(ServiceCardLockArguments arguments)
		{
			var now = DateTime.Now.ToUTC();

            var cards = (await Repository.GetAsync())
                .Where(x => x.Id == arguments.Id)
                .Select(x => new
                {
                    x.Uid,
                    x.SystemCardId
                });

            foreach (var card in cards)
            {
                var blackList = new BlackList
                {
                    Uid = card.Uid,
                    RegistrationDate = now,
                    Machine = BlackListMachineType.All,
                    Resolved = false,
                    ResolutionDate = null,
                    Service = BlackListServiceType.Rejection,
                    IsConfirmed = false,
                    Source = BlackList.BlackListSourceType.Payin,
                    State = BlackList.BlackListStateType.Active,
                    SystemCardId = card.SystemCardId
                };
                await BlackListRepository.AddAsync(blackList);
            }

			return null;
		}
		#endregion ExecuteAsync
	}
}
