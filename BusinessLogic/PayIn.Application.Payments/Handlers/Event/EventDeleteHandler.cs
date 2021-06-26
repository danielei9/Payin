using PayIn.Application.Dto.Payments.Arguments;
using PayIn.Common;
using PayIn.Common.Resources;
using PayIn.Domain.Payments;
using System;
using System.Linq;
using System.Threading.Tasks;
using Xp.Application.Dto;
using Xp.Domain;

namespace PayIn.Application.Payments.Handlers
{
	public class EventDeleteHandler :
		IServiceBaseHandler<EventDeleteArguments>
	{
		private readonly IEntityRepository<Event> Repository;

        #region Constructors
        public EventDeleteHandler(IEntityRepository<Event> repository,
			IEntityRepository<Entrance> entranceRepository
        )
		{
			if (repository == null) throw new ArgumentNullException("repository");

            Repository = repository;
        }
		#endregion Constructors

		#region ExecuteAsync
		public async Task<dynamic> ExecuteAsync(EventDeleteArguments arguments)
		{
			var item = (await Repository.GetAsync())
				.Where(x => x.Id == arguments.Id)
				.Select(x => new {
					Event = x,
					Count = x.EntranceTypes.Sum(y => (int?)y.Entrances.Count())
				})
				.FirstOrDefault();
            if ((item != null) && (item.Count > 0))
                throw new ApplicationException(EventResources.DeleteWithEntrancesException.FormatString(item.Event.Name, item.Count));

            item.Event.State = EventState.Deleted;

			return null;
		}
		#endregion ExecuteAsync
	}
}
