using PayIn.Application.Dto.Payments.Arguments;
using PayIn.Common;
using PayIn.Domain.Payments;
using System;
using System.Threading.Tasks;
using Xp.Application.Dto;
using Xp.Domain;

namespace PayIn.Application.Payments.Handlers
{
    public class EventSuspendHandler :
		IServiceBaseHandler<EventSuspendArguments>
	{
		private readonly IEntityRepository<Event> Repository;

		#region Constructors
		public EventSuspendHandler(IEntityRepository<Event> repository)
		{
			if (repository == null) throw new ArgumentNullException("repository");
			Repository = repository;
		}
		#endregion Constructors

		#region ExecuteAsync
		public async Task<dynamic> ExecuteAsync(EventSuspendArguments arguments)
		{
			var item = (await Repository.GetAsync(arguments.Id));

			item.State = EventState.Active;

			return item;
		}
		#endregion ExecuteAsync
	}
}
