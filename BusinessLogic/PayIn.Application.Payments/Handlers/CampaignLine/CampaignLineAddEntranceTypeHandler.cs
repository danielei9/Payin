using PayIn.Application.Dto.Payments.Arguments;
using PayIn.Domain.Payments;
using System;
using System.Linq;
using System.Threading.Tasks;
using Xp.Application.Dto;
using Xp.Domain;

namespace PayIn.Application.Payments.Handlers
{
	class CampaignLineAddEntranceTypeHandler : 
		IServiceBaseHandler<CampaignLineAddEntranceTypeArguments>
	{
		private readonly IEntityRepository<CampaignLine> Repository;
		private readonly IEntityRepository<EntranceType> EntranceTypeRepository;

		#region Constructors
		public CampaignLineAddEntranceTypeHandler(
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
		public async Task<dynamic> ExecuteAsync(CampaignLineAddEntranceTypeArguments arguments)
		{
			var entranceType = (await EntranceTypeRepository.GetAsync(arguments.EntranceTypeId));
			if (entranceType == null)
				throw new ArgumentNullException("EntranceTypeId");

			var item = (await Repository.GetAsync(arguments.Id, "EntranceTypes","Campaign.TargetEvents"));
			if (item == null)
				throw new ArgumentNullException("Id");

			if (item.AllEntranceType)
				throw new ApplicationException("No se puede asociar un tipo de entrada a un descuento que aplica a todos los tipos de entrada");
			if (
				(item.Campaign.TargetEvents.Count() != 0) &&
				(!item.Campaign.TargetEvents.Any(x => x.Id == entranceType.EventId))
			)
				throw new ApplicationException("No se puede asociar un tipo de entrada de un evento a un descuento que aplica a otros eventos diferentes");

			item.EntranceTypes.Add(entranceType);

			return item;
		}
		#endregion ExecuteAsync
	}
}
