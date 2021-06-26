using PayIn.Application.Dto.Transport.Arguments.TransportSimultaneousTitleCompatibilities;
using PayIn.BusinessLogic.Common;
using PayIn.Domain.Transport;
using System;
using System.Threading.Tasks;
using Xp.Application.Dto;
using Xp.Domain;

namespace PayIn.Application.Transport.Handlers
{
	class TransportSimultaneousTitleCompatibilitiesCreateHandler : IServiceBaseHandler<TransportSimultaneousTitleCompatibilitiesCreateArguments>
	{
		private readonly ISessionData SessionData;
		private readonly IEntityRepository<TransportSimultaneousTitleCompatibility> Repository;

		#region Contructors
		public TransportSimultaneousTitleCompatibilitiesCreateHandler(
			ISessionData sessionData,
			IEntityRepository<TransportSimultaneousTitleCompatibility> repository
		)
		{
			if (sessionData == null) throw new ArgumentNullException("sessionData");
			if (repository == null) throw new ArgumentNullException("repository");

			SessionData = sessionData;
			Repository = repository;
		}
		#endregion Contructors

		#region ExecuteAsync
		public async Task<dynamic> ExecuteAsync(TransportSimultaneousTitleCompatibilitiesCreateArguments arguments)
		{
			var system = new TransportSimultaneousTitleCompatibility
			{
				TransportTitle2Id = arguments.TransportTitle2Id,
				TransportTitleId = arguments.TitleId
			};
			await Repository.AddAsync(system);
			return system;
		}
		#endregion ExecuteAsync
	}

}
