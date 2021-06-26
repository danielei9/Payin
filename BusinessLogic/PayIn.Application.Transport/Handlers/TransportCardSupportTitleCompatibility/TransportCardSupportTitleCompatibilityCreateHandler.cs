using PayIn.Application.Dto.Transport.Arguments.TransportCardSupportTitleCompatibility;
using PayIn.BusinessLogic.Common;
using PayIn.Domain.Transport;
using System;
using System.Threading.Tasks;
using Xp.Application.Dto;
using Xp.Domain;

namespace PayIn.Application.Transport.Handlers
{
	class TransportCardSupportTitleCompatibilityCreateHandler : IServiceBaseHandler<TransportCardSupportTitleCompatibilityCreateArguments>
	{
		private readonly ISessionData SessionData;
		private readonly IEntityRepository<TransportCardSupportTitleCompatibility> Repository;

		#region Contructors
		public TransportCardSupportTitleCompatibilityCreateHandler(
			ISessionData sessionData,
			IEntityRepository<TransportCardSupportTitleCompatibility> repository
		)
		{
			if (sessionData == null) throw new ArgumentNullException("sessionData");
			if (repository == null) throw new ArgumentNullException("repository");

			SessionData = sessionData;
			Repository = repository;
		}
		#endregion Contructors

		#region ExecuteAsync
		public async Task<dynamic> ExecuteAsync(TransportCardSupportTitleCompatibilityCreateArguments arguments)
		{
			var system = new TransportCardSupportTitleCompatibility
            {
				TransportCardSupportId = arguments.TransportCardSupportId,
				TransportTitleId = arguments.TitleId
			};
			await Repository.AddAsync(system);
			return system;
		}
		#endregion ExecuteAsync
	}

}
