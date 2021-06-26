using PayIn.Application.Dto.Payments.Arguments;
using PayIn.Domain.Payments;
using System;
using System.Linq;
using System.Threading.Tasks;
using Xp.Application.Dto;
using Xp.Domain;

namespace PayIn.Application.Payments.Handlers
{
	public class CampaignRemoveEventHandler :
		IServiceBaseHandler<CampaignRemoveEventArguments>
	{
		private readonly IEntityRepository<Campaign> Repository;
		private readonly IEntityRepository<Event> EventRepository;

		#region Constructors
		public CampaignRemoveEventHandler(
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
		public async Task<dynamic> ExecuteAsync(CampaignRemoveEventArguments arguments)
		{
			var eventRemove = (await EventRepository.GetAsync(arguments.EventId));
			if (eventRemove == null)
				throw new ArgumentNullException("EventId");

            var items = (await Repository.GetAsync("TargetEvents"))
				.Where(x => 
					x.Id == arguments.Id &&
					x.State != Common.CampaignState.Deleted
				);
			
			foreach(var item in items)
				item.TargetEvents.Remove(eventRemove);
            
			return null;
		}
		#endregion ExecuteAsync
	}
}
