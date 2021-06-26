using PayIn.Application.Dto.Payments.Arguments;
using PayIn.Application.Dto.Payments.Results;
using PayIn.BusinessLogic.Common;
using PayIn.Common;
using PayIn.Domain.Payments;
using PayIn.Domain.Public;
using System;
using System.Linq;
using System.Threading.Tasks;
using Xp.Application.Dto;
using Xp.Common;
using Xp.Domain;

namespace PayIn.Application.Payments.Handlers
{
	class EntranceTypeFormGetAllHandler :
		IQueryBaseHandler<EntranceTypeFormGetAllArguments, EntranceTypeFormGetAllResult>
	{
		private readonly IEntityRepository<EntranceTypeForm> Repository;
		private readonly IEntityRepository<EntranceType> EntranceTypeRepository;
		private readonly IEntityRepository<ControlForm> ControlFormRepository;
		private readonly ISessionData SessionData;

		#region Constructors
		public EntranceTypeFormGetAllHandler(
			IEntityRepository<EntranceTypeForm> repository,
			IEntityRepository<EntranceType> entranceTypeRepository,
			IEntityRepository<ControlForm> controlFormRepository,
			ISessionData sessionData
			)
		{
			if (repository == null) throw new ArgumentNullException("repository");
			if (entranceTypeRepository == null) throw new ArgumentNullException("entranceTypeRepository");
			if (controlFormRepository == null) throw new ArgumentNullException("controlFormRepository");
			if (sessionData == null) throw new ArgumentNullException("sessionData");

			Repository = repository;
			SessionData = sessionData;
			EntranceTypeRepository = entranceTypeRepository;
			ControlFormRepository = controlFormRepository;
		}
		#endregion Constructors

		#region ExecuteAsync
		public async Task<ResultBase<EntranceTypeFormGetAllResult>> ExecuteAsync(EntranceTypeFormGetAllArguments arguments)
		{
			var items = (await Repository.GetAsync())
				.Where(x =>
				    x.EntranceType.Id == arguments.EntranceTypeId 
				);

			var forms = (await ControlFormRepository.GetAsync())
				.Where(x => x.State == ControlFormState.Active);

			var result = items
                .OrderBy(x => x.Order)
				.Select(x => new EntranceTypeFormGetAllResult
                {
					Id = x.Id,
					FormId = x.FormId,
					EntranceTypeId = x.EntranceTypeId,
					Order = x.Order,
					EntranceTypeName = " ( " + x.EntranceType.Name + " )",
					Name = forms
                        .Where(y => y.Id == x.FormId)
                        .Select(y => y.Name)
                        .FirstOrDefault()
				});

			return new ResultBase<EntranceTypeFormGetAllResult> { Data = result };
		}
		#endregion ExecuteAsync
	}
}
