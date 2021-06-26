using PayIn.Application.Dto.Transport.Arguments.TransportCardSupport;
using PayIn.BusinessLogic.Common;
using PayIn.Common;
using PayIn.Domain.Transport;
using System;
using System.Threading.Tasks;
using Xp.Application.Dto;
using Xp.Domain;

namespace PayIn.Application.Transport.Handlers
{
	class TransportCardSupportCreateHandler : IServiceBaseHandler<TransportCardSupportCreateArguments>
	{
		private readonly ISessionData SessionData;
		private readonly IEntityRepository<TransportCardSupport> Repository;

		#region Contructors
		public TransportCardSupportCreateHandler(
			ISessionData sessionData,
			IEntityRepository<TransportCardSupport> repository
		)
		{
			if (sessionData == null) throw new ArgumentNullException("sessionData");
			if (repository == null) throw new ArgumentNullException("repository");

			SessionData = sessionData;
			Repository = repository;
		}
		#endregion Contructors

		#region ExecuteAsync
		public async Task<dynamic> ExecuteAsync(TransportCardSupportCreateArguments arguments)
		{
			var system = new TransportCardSupport
			{
				Name = arguments.Name,
				OwnerCode = arguments.OwnerCode,
				OwnerName = arguments.OwnerName,
				Type = arguments.Type,
				SubType = arguments.SubType,
				State = TransportCardSupportState.Active
			};
			await Repository.AddAsync(system);
			return system;
		}
		#endregion ExecuteAsync
	}

}
