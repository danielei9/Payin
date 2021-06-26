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
    public class TransportWhiteListDownloadHandler :
		IServiceBaseHandler<TransportWhiteListDownloadArguments>
	{
        [Dependency] public ServerService ServerService { get; set; }
        [Dependency] public IEntityRepository<WhiteList> WhiteListRepository { get; set; }
        [Dependency] public IUnitOfWork UnitOfWork { get; set; }

		#region ExecuteAsync
		public async Task<dynamic> ExecuteAsync(TransportWhiteListDownloadArguments arguments)
		{
            var now = DateTime.UtcNow;

            var itemsAdded = new List<WhiteList>();
            var itemsUpdated = new List<WhiteList>();

            // Load from server
            var loadedServer = (await ServerService.WhiteListDownload(now))
                .ToList();
            var loadedServerDictionary = loadedServer
                .GroupBy(x => x.OperationNumber)
                .ToDictionary(x => x.Key, x => x.ToList());

            // Load from db
            var loadedDb = (await WhiteListRepository.GetAsync())
                .Where(x =>
                    x.State == WhiteList.WhiteListStateType.Active
                )
                .ToList();
            var loadedDbDictionary = loadedDb
                .GroupBy(x => x.OperationNumber)
                .ToDictionary(x => x.Key, x => x.ToList());

            foreach (var itemServer in loadedServer)
            {
                var itemDb = !loadedDbDictionary.ContainsKey(itemServer.OperationNumber) ?
                    null :
                    loadedDbDictionary[itemServer.OperationNumber]
                        .Where(x => itemServer.Source == x.Source)
                        .FirstOrDefault();
                
                if (itemDb == null) // Add
                {
                    var item = itemServer.GetWhiteList(now);

                    itemsAdded.Add(item);
                    // Se añadirán a la BD de forma conjunta
                } else if ( // Update
                    (itemDb.Uid != itemServer.Uid) ||
                    (itemDb.State != itemServer.State) ||
                    (itemDb.TitleType != itemServer.TitleType) ||
                    (itemDb.Amount != itemServer.Amount) ||
                    (itemDb.ExclusionDate != itemServer.ExclusionDate) ||
                    (itemServer.InclusionDate != null && itemDb.InclusionDate != itemServer.InclusionDate)
                )
                {
                    itemsUpdated.Add(itemDb);

                    itemDb.Uid = itemServer.Uid;
                    itemDb.State = itemServer.State;
                    itemDb.TitleType = itemServer.TitleType;
                    itemDb.Amount = itemServer.Amount;
                    itemDb.ExclusionDate = itemServer.ExclusionDate;
                    if (itemServer.InclusionDate != null)
                        itemDb.InclusionDate = itemServer.InclusionDate.Value;
                }
            }

            var itemsRemoved = loadedDb
                .Where(x =>
                    !loadedServerDictionary.ContainsKey(x.OperationNumber) ||
                    !loadedServerDictionary[x.OperationNumber]
                        .Where(y => y.Source == x.Source)
                        .Any()
                )
                .ToList();
            foreach (var item in itemsRemoved)
                item.State = WhiteList.WhiteListStateType.Delete;

            await UnitOfWork.SaveAsync();

            PublicContext.BulkInsert(itemsAdded);

            return new WhiteListDownloadResult
            {
                ItemsAdded = itemsAdded.Select(x => new WhiteListDownloadResult_Item(x)),
                ItemsUpdated = itemsUpdated.Select(x => new WhiteListDownloadResult_Item(x)),
                ItemsRemoved = itemsRemoved.Select(x => new WhiteListDownloadResult_Item(x))
            };
        }
		#endregion ExecuteAsync
	}
}