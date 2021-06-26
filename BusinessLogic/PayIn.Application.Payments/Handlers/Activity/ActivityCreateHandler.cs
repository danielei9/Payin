using PayIn.Application.Dto.Payments.Arguments;
using PayIn.Domain.Payments;
using System;
using System.Threading.Tasks;
using Xp.Application.Dto;
using Xp.Domain;

namespace PayIn.Application.Payments.Handlers
{
	class ActivityCreateHandler :
		IServiceBaseHandler<ActivityCreateArguments>
	{
		private readonly IEntityRepository<Activity> Repository;
		private readonly IEntityRepository<Event> EventRepository;

		#region Constructors
		public ActivityCreateHandler(
			IEntityRepository<Activity> repository,
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
		public async Task<dynamic> ExecuteAsync(ActivityCreateArguments arguments)
		{
			var eventAssing = (await EventRepository.GetAsync(arguments.EventId));
			if (eventAssing == null)
				throw new ArgumentNullException("EventId");

			var activity = new Activity
			{
				Name = arguments.Name,
				Start = arguments.Start,
				End = arguments.End,
				Description = arguments.Description
			};
			eventAssing.Activities.Add(activity);
			await Repository.AddAsync(activity);
			return activity;
		}
		#endregion ExecuteAsync
	}
}
