using PayIn.Application.Dto.Payments.Arguments;
using PayIn.Domain.Payments;
using System;
using System.Threading.Tasks;
using Xp.Application.Dto;
using Xp.Domain;

namespace PayIn.Application.Payments.Handlers
{
	class CampaignAddEventHandler : 
		IServiceBaseHandler<CampaignAddEventArguments>
	{
		private readonly IEntityRepository<Campaign> Repository;
		private readonly IEntityRepository<Event> EventRepository;

		#region Constructors
		public CampaignAddEventHandler(
			IEntityRepository<Campaign> repository,
			IEntityRepository<Event> eventRepository
		)
		{
			if (repository == null) throw new ArgumentNullException("repository");
			if (eventRepository == null) throw new ArgumentNullException("eventRepository");

			Repository = repository;
            EventRepository = eventRepository;
		}
		#endregion Constructors

		#region ExecuteAsync
		public async Task<dynamic> ExecuteAsync(CampaignAddEventArguments arguments)
		{
			var eventSelected = (await EventRepository.GetAsync(arguments.EventId));
			if (eventSelected == null)
				throw new ArgumentNullException("EventId");

			var item = (await Repository.GetAsync(arguments.Id));
			if (item == null)
				throw new ArgumentNullException("Id");

            item.TargetEvents.Add(eventSelected);

			return item;
		}
		#endregion ExecuteAsync
	}
}
