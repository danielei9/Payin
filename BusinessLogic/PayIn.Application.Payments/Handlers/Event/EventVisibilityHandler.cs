using PayIn.Application.Dto.Payments.Arguments;
using PayIn.Domain.Payments;
using System;
using System.Threading.Tasks;
using Xp.Application.Dto;
using Xp.Domain;

namespace PayIn.Application.Payments.Handlers
{
	public class EventVisibilityHandler : 
		IServiceBaseHandler<EventVisibilityArguments>
	{
		private readonly IEntityRepository<Event> Repository;

		#region Constructors
		public EventVisibilityHandler(IEntityRepository<Event> repository)
		{
			if (repository == null) throw new ArgumentNullException("repository");
			Repository = repository;
		}
		#endregion Constructors

		#region ExecuteAsync
		public async Task<dynamic> ExecuteAsync(EventVisibilityArguments arguments)
		{
			var item = (await Repository.GetAsync(arguments.Id));

			item.Visibility = arguments.Visibility;

			return item;
		}
		#endregion ExecuteAsync
	}
}
