using PayIn.Application.Dto.Transport.Arguments.TransportTitle;
using PayIn.BusinessLogic.Common;
using PayIn.Common;
using PayIn.Domain.Security;
using PayIn.Domain.Transport;
using System;
using System.Linq;
using System.Threading.Tasks;
using Xp.Application.Dto;
using Xp.Domain;

namespace PayIn.Application.Transport.Handlers
{
	public class TransportTitleCreateHandler : IServiceBaseHandler<TransportTitleCreateArguments>
	{
		private readonly ISessionData SessionData;
		private readonly IEntityRepository<TransportTitle> Repository;
		private readonly IEntityRepository<TransportConcession> RepositoryTransportConcession;

		#region Constructors
		public TransportTitleCreateHandler(
			ISessionData sessionData,
			IEntityRepository<TransportTitle> repository,
			IEntityRepository<TransportConcession> repositoryTransportConcession
		)
		{
			if (sessionData == null) throw new ArgumentNullException("sessionData");
			if (repository == null) throw new ArgumentNullException("repository");
			if (repositoryTransportConcession == null) throw new ArgumentNullException("repositoryTransportConcession");

			SessionData = sessionData;
			Repository = repository;
			RepositoryTransportConcession = repositoryTransportConcession;
		}
		#endregion Constructors

		#region ExecuteAsync
		public async Task<dynamic> ExecuteAsync(TransportTitleCreateArguments arguments)
		{
			var transportConcession = (await RepositoryTransportConcession.GetAsync())
				.Where(x => x.Concession.Concession.Name == arguments.TransportConcessionName)
				.FirstOrDefault();

			var title = new TransportTitle
			{
				Name = arguments.Name,
				Code = arguments.Code,
				OwnerCode = arguments.OwnerCode,
				OwnerName = arguments.OwnerName,
				Image = "",
				HasZone = arguments.HasZone,
				TransportConcessionId = transportConcession.Id,
				MaxQuantity = arguments.MaxQuantity,
				OperateByPayIn = arguments.OperateByPayIn,
				IsYoung = arguments.IsYoung,
				Environment = arguments.Environment,
				State = TransportTitleState.Active,
				IsOverWritable = arguments.IsOverWritable,
				Slot = arguments.Slot,
				ValidityBit = arguments.ValidityBit,
				TableIndex = arguments.TableIndex,
				MaxExternalChanges = arguments.MaxExternalChanges,
				MaxPeopleChanges = arguments.MaxPeopleChanges,
				ActiveTitle = arguments.ActiveTitle,
				Priority = arguments.Priority,
				TemporalType = arguments.TemporalType ?? null,
				TemporalUnit = arguments.TemporalUnit ?? null,
				QuantityType = arguments.QuantityType ?? null,
				Quantity = arguments.Quantity
			};
			await Repository.AddAsync(title);

			return title;
		}
		#endregion ExecuteAsync
	}
}

