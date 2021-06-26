using PayIn.Application.Dto.Transport.Arguments.BlackList;
using PayIn.Application.Dto.Transport.Results.BlackList;
using PayIn.BusinessLogic.Common;
using PayIn.Common;
using PayIn.Domain.Public;
using PayIn.Domain.Transport;
using System;
using System.Linq;
using System.Threading.Tasks;
using Xp.Application.Dto;
using Xp.Domain;

namespace PayIn.Application.Public.Handlers
{
	public class BlackListGetAllHandler :
		IQueryBaseHandler<BlackListGetAllArguments, BlackListGetAllResult>
	{
		private readonly IEntityRepository<BlackList> Repository;
		private readonly IEntityRepository<TransportConcession> TransportRepository;
		private readonly ISessionData SessionData;

		#region Constructor
		public BlackListGetAllHandler(
			IEntityRepository<BlackList> repository,
			IEntityRepository<TransportConcession> transportRepository,
			ISessionData sessionData
		)
		{
			if (repository == null) throw new ArgumentNullException("GreyList");
			if (transportRepository == null) throw new ArgumentNullException("TransportConcession");
			if (sessionData == null) throw new ArgumentNullException("sessionData");

			Repository = repository;
			TransportRepository = transportRepository;
			SessionData = sessionData;
		}
		#endregion Constructor

		#region ExecuteAsync
		public async Task<ResultBase<BlackListGetAllResult>> ExecuteAsync(BlackListGetAllArguments arguments)
		{
			var concession = (await TransportRepository.GetAsync())
				.Where(x => x.Concession.Concession.Supplier.Login == SessionData.Email)
				.FirstOrDefault();

			var items = (await Repository.GetAsync())
				//.Where(x =>x.Concession == concession.ConcessionId);	
				  .Where(x => x.Concession == 0);

			if (!arguments.Filter.IsNullOrEmpty())
			{
				items = items.Where(x => x.Uid.ToString().Contains(arguments.Filter));
			}

			var result = items
					.Select(x => new
					{
						Id = x.Id,
						Uid = x.Uid,
						RegistrationDate = x.RegistrationDate,
						Machine = x.Machine,
						Resolved = x.Resolved,
						ResolutionDate = x.ResolutionDate,
						Rejection = x.Rejection,
						Service = x.Service
					})
					.OrderByDescending(x => x.RegistrationDate)
					.ToList()					
					.Select(x => new BlackListGetAllResult
					{
						Id = x.Id,
						Uid = x.Uid,
						RegistrationDate = x.RegistrationDate,
						Machine = x.Machine,
						MachineAlias = x.Machine.ToEnumAlias(),
						Resolved = x.Resolved,
						ResolutionDate = x.ResolutionDate,
						Rejection = x.Rejection,
						Service = x.Service,
						ServiceAlias = x.Service.ToEnumAlias()
					});

			return new ResultBase<BlackListGetAllResult> { Data = result };

		}
		#endregion ExecuteAsync
	}
}
