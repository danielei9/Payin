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
    public class TransportGreyListDownloadHandler :
		IServiceBaseHandler<TransportGreyListDownloadArguments>
	{
        [Dependency] public ServerService ServerService { get; set; }
        [Dependency] public IEntityRepository<GreyList> GreyListRepository { get; set; }
        [Dependency] public IUnitOfWork UnitOfWork { get; set; }

		#region ExecuteAsync
		public async Task<dynamic> ExecuteAsync(TransportGreyListDownloadArguments arguments)
		{
            var now = DateTime.UtcNow;

            var itemsAdded = new List<GreyList>();
            var itemsUpdated = new List<GreyList>();

            // Load from server
            var loadedServer = (await ServerService.GreyListDownload(now))
                .ToList();
            var loadedServerDictionary = loadedServer
                .GroupBy(x => x.OperationNumber)
                .ToDictionary(x => x.Key, x => x.ToList());

            // Load from db
            var loadedDb = (await GreyListRepository.GetAsync())
                .Where(x =>
                    x.State == GreyList.GreyListStateType.Active
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
                    var item = itemServer.GetGreyList(now);

                    itemsAdded.Add(item);
                    // Se añadirán a la BD de forma conjunta
                } else if ( // Update
                    (itemDb.Uid != itemServer.Uid) ||
                    (itemDb.Action != itemServer.Action) ||
                    (itemDb.Field != itemServer.Field) ||
                    (itemDb.NewValue != itemServer.NewValue) ||
                    (itemDb.Resolved != itemServer.Resolved) ||
                    (itemDb.ResolutionDate != itemServer.ResolutionDate) ||
                    (itemDb.Machine != itemServer.Machine) ||
                    (itemDb.OldValue != itemServer.OldValue) ||
                    (itemDb.State != itemServer.State) ||
                    (itemServer.RegistrationDate != null && itemDb.RegistrationDate != itemServer.RegistrationDate)
                )
                {
                    itemsUpdated.Add(itemDb);
                    itemDb.Uid = itemServer.Uid;
                    itemDb.Action = itemServer.Action;
                    itemDb.Field = itemServer.Field;
                    itemDb.NewValue = itemServer.NewValue;
                    itemDb.Resolved = itemServer.Resolved;
                    itemDb.ResolutionDate = itemServer.ResolutionDate;
                    itemDb.Machine = itemServer.Machine;
                    itemDb.OldValue = itemServer.OldValue;
                    itemDb.State = itemServer.State;
                    if (itemServer.RegistrationDate != null)
                        itemDb.RegistrationDate = itemServer.RegistrationDate.Value;
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
                item.State = GreyList.GreyListStateType.Delete;

            await UnitOfWork.SaveAsync();

            PublicContext.BulkInsert(itemsAdded);

            return new GreyListDownloadResult
            {
                ItemsAdded = itemsAdded.Select(x => new GreyListDownloadResult_Item(x)),
                ItemsUpdated = itemsUpdated.Select(x => new GreyListDownloadResult_Item(x)),
                ItemsRemoved = itemsRemoved.Select(x => new GreyListDownloadResult_Item(x))
            };
        }
		#endregion ExecuteAsync
	}
}