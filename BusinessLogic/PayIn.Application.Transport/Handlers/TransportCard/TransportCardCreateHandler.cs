using PayIn.Application.Dto.Transport.Arguments.TransportCard;
using PayIn.BusinessLogic.Common;
using PayIn.Common;
using PayIn.Domain.Security;
using PayIn.Domain.Transport;
using System;
using System.Linq;
using System.Threading.Tasks;
using Xp.Application.Dto;
using Xp.Domain;
using Xp.Domain.Transport;

namespace PayIn.Application.Transport.Handlers
{
	public class TransportCardCreateHandler : IServiceBaseHandler<TransportCardCreateArguments>
	{		
		private readonly IEntityRepository<TransportCard> Repository;
		private readonly IEntityRepository<TransportConcession> RepositoryTransportConcession;
		private readonly IEntityRepository<TransportSystem> SystemRepository;
		private readonly IEntityRepository<TransportOperation> OperationRepository;

		#region Constructors
		public TransportCardCreateHandler(		
			IEntityRepository<TransportCard> repository,
			IEntityRepository<TransportConcession> repositoryTransportConcession,
			IEntityRepository<TransportSystem> systemRepository,
			IEntityRepository<TransportOperation> operationRepository
		)
		{			
			if (repository == null) throw new ArgumentNullException("repository");
			if (repositoryTransportConcession == null) throw new ArgumentNullException("repositoryTransportConcession");
			if (systemRepository == null) throw new ArgumentNullException("systemRepository");
			if (operationRepository == null) throw new ArgumentNullException("operationRepository");


			Repository = repository;
			RepositoryTransportConcession = repositoryTransportConcession;
			SystemRepository = systemRepository;
			OperationRepository = operationRepository;
		}
		#endregion Constructors

		#region ExecuteAsync
		public async Task<dynamic> ExecuteAsync(TransportCardCreateArguments arguments)
		{
			var now = DateTime.Now.ToUTC();

			// Delete existing cards with same address and entry
			var collection = (await Repository.GetAsync())
				.Where(x =>
					x.DeviceAddress == arguments.DeviceAddress &&
					x.Entry == arguments.Entry &&
					x.State != TransportCardState.Deleted
				);
			foreach (var item in collection)
				item.State = TransportCardState.Deleted;

			var transportConcession = (await RepositoryTransportConcession.GetAsync())
				.Where(x => x.Concession.Concession.Name == "aVM Tesoreria")
				.FirstOrDefault();
			var transportSystem = (await SystemRepository.GetAsync())
				.Where(x => x.Name == "Valencia Classic")
				.FirstOrDefault();

			var transportCard = new TransportCard
			{
				DeviceType = arguments.DeviceType,
				DeviceAddress = arguments.DeviceAddress,
				Name = arguments.Name,
				Entry = arguments.Entry,
				RandomId = arguments.RandomId,
				Uid = arguments.Uid,
				LastUse = now,
				ExpiryDate = null,
				Login = arguments.Login,
				ImageUrl = "",
				TransportSystem = transportSystem,
				TransportConcession = transportConcession,
				State = TransportCardState.Active
			};
			await Repository.AddAsync(transportCard);

			var operation = new TransportOperation
			{
				Uid = arguments.Uid,
				OperationType = OperationType.CreateCard,
				OperationDate = DateTime.UtcNow,
				Device = arguments.DeviceAddress,
				Login = arguments.Login
			};
			await OperationRepository.AddAsync(operation);

			return transportCard;
		}
		#endregion ExecuteAsync
	}
}

