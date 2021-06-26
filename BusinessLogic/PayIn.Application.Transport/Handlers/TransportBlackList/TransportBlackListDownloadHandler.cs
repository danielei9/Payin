using Microsoft.Practices.Unity;
using PayIn.Application.Dto.Transport.Arguments.TransportList;
using PayIn.Application.Dto.Transport.Services;
using PayIn.Application.Transport.Services;
using PayIn.Domain.Transport;
using PayIn.Infrastructure.Public.Db;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xp.Application.Dto;
using Xp.Domain;

namespace PayIn.Application.Transport.Handlers
{
    public class TransportBlackListDownloadHandler :
		IServiceBaseHandler<TransportBlackListDownloadArguments>
	{
        [Dependency] public ServerService ServerService { get; set; }
        [Dependency] public IEntityRepository<BlackList> BlackListRepository { get; set; }
        [Dependency] public IUnitOfWork UnitOfWork { get; set; }

		#region ExecuteAsync
		public async Task<dynamic> ExecuteAsync(TransportBlackListDownloadArguments arguments)
        {
            var now = DateTime.UtcNow;

            var itemsAdded = new List<BlackList>();
            var itemsUpdated = new List<BlackList>();

            // Load from server
            var loadedServer = (await ServerService.BlackListDownload(now))
                .ToList();
            var loadedServerDictionary = loadedServer
                .GroupBy(x => x.Uid)
                .ToDictionary(x => x.Key, x => x.ToList());

            // Load from db
            var loadedDb = (await BlackListRepository.GetAsync())
                .Where(x =>
                    x.State == BlackList.BlackListStateType.Active
                )
                .ToList();
            var loadedDbDictionary = loadedDb
                .GroupBy(x => x.Uid)
                .ToDictionary(x => x.Key, x => x.ToList());

            foreach (var itemServer in loadedServer)
            {
                var itemDb = !loadedDbDictionary.ContainsKey(itemServer.Uid) ?
                    null :
                    loadedDbDictionary[itemServer.Uid]
                        .Where(x => itemServer.Source == x.Source)
                        .FirstOrDefault();

                // Add
                if (itemDb == null)
                {
                    var item = itemServer.GetBlackList(now);

                    itemsAdded.Add(item);
                    // Se añadirán a la BD de forma conjunta
                    //await BlackListRepository.AddAsync(item);
                    continue;
                }

                // Update
                if (
                    (itemDb.Uid != itemServer.Uid) ||
                    (itemDb.Resolved != itemServer.Resolved) ||
                    (itemDb.ResolutionDate != itemServer.ResolutionDate) ||
                    (itemDb.Machine != itemServer.Machine) ||
                    (itemDb.Rejection != itemServer.Rejection)  ||
                    (itemDb.Concession != itemServer.Concession)||
                    (itemDb.Service != itemServer.Service) ||
                    (itemDb.State != itemServer.State) ||
                    (itemServer.RegistrationDate != null && itemDb.RegistrationDate != itemServer.RegistrationDate)
                )
                {
                    itemsUpdated.Add(itemDb);
                    itemDb.Uid = itemServer.Uid;
                    itemDb.Resolved = itemServer.Resolved;
                    itemDb.ResolutionDate = itemServer.ResolutionDate;
                    itemDb.Machine = itemServer.Machine;
                    itemDb.Rejection = itemServer.Rejection;
                    itemDb.Concession = itemServer.Concession;
                    itemDb.Service = itemServer.Service;
                    itemDb.State = itemServer.State;
                    if (itemServer.RegistrationDate != null)
                        itemDb.RegistrationDate = itemServer.RegistrationDate.Value;
                }
            }
            
            var itemsRemoved = loadedDb
                .Where(x =>
                    !loadedServerDictionary.ContainsKey(x.Uid) ||
                    !loadedServerDictionary[x.Uid]
                        .Where(y => y.Source == x.Source)
                        .Any()
                )
                .ToList();
            foreach (var item in itemsRemoved)
                item.State = BlackList.BlackListStateType.Delete;

            await UnitOfWork.SaveAsync();

            PublicContext.BulkInsert(itemsAdded);

            return new BlackListDownloadResult
            {
                ItemsAdded = itemsAdded.Select(x => new BlackListDownloadResult_Item(x)),
                ItemsUpdated = itemsUpdated.Select(x => new BlackListDownloadResult_Item(x)),
                ItemsRemoved = itemsRemoved.Select(x => new BlackListDownloadResult_Item(x))
            };
        }
        #endregion ExecuteAsync
    }
}