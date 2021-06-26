using PayIn.Application.Dto.Payments.Arguments;
using PayIn.Domain.Payments;
using System;
using System.Threading.Tasks;
using Xp.Application.Dto;
using Xp.Domain;

namespace PayIn.Application.Payments.Handlers
{
	public class CampaignLineRemoveEntranceTypeHandler :
		IServiceBaseHandler<CampaignLineRemoveEntranceTypeArguments>
	{
		private readonly IEntityRepository<CampaignLine> Repository;
		private readonly IEntityRepository<EntranceType> EntranceTypeRepository;

		#region Constructors
		public CampaignLineRemoveEntranceTypeHandler(
			IEntityRepository<CampaignLine> repository,
			IEntityRepository<EntranceType> entranceTypeRepository
		)
		{
			if (repository == null) throw new ArgumentNullException("repository");
			if (entranceTypeRepository == null) throw new ArgumentNullException("entranceTypeRepository");

			Repository = repository;
			EntranceTypeRepository = entranceTypeRepository;
		}
		#endregion Constructors

		#region ExecuteAsync
		public async Task<dynamic> ExecuteAsync(CampaignLineRemoveEntranceTypeArguments arguments)
		{
			var entranceType = (await EntranceTypeRepository.GetAsync(arguments.EntranceTypeId));
			if (entranceType == null)
				throw new ArgumentNullException("EntranceTypeId");

			var item = (await Repository.GetAsync(arguments.Id, "EntranceTypes"));
			if (item == null)
				throw new ArgumentNullException("Id");

			item.EntranceTypes.Remove(entranceType);

			return item;
		}
		#endregion ExecuteAsync
	}
}
