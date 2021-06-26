using Microsoft.Practices.Unity;
using PayIn.Application.Dto.Payments.Arguments;
using PayIn.Application.Dto.Payments.Results;
using PayIn.BusinessLogic.Common;
using PayIn.Common;
using PayIn.Domain.Payments;
using System;
using System.Linq;
using System.Threading.Tasks;
using Xp.Application.Dto;
using Xp.Common;
using Xp.Domain;

namespace PayIn.Application.Payments.Handlers
{
	public class EventGetAllHandler : IQueryBaseHandler<EventGetAllArguments, EventGetAllResult>
	{
		[Dependency] public IEntityRepository<Event> Repository { get; set; }
		[Dependency] public  IEntityRepository<EntranceType> EntranceTypeRepository { get; set; }
        [Dependency] public  IEntityRepository<PaymentConcession> PaymentConcessionRepository { get; set; }
        [Dependency] public  ISessionData SessionData { get; set; }

		#region ExecuteAsync
		public async Task<ResultBase<EventGetAllResult>> ExecuteAsync(EventGetAllArguments arguments)
		{
			var events = (await Repository.GetAsync());
			if (arguments.PaymentConcessionId != null)
				events = events
					.Where(x =>
						x.PaymentConcessionId == arguments.PaymentConcessionId
					);
			if (!arguments.Filter.IsNullOrEmpty())
				events = events
					.Where(x =>
						x.Name.Contains(arguments.Filter) ||
						x.Place.Contains(arguments.Filter)
					);

			var result = events
					.Select(x => new
					{
						x.Id,
						x.Place,
						x.Name,
						x.EventStart,
						x.EventEnd,
						x.State,
						x.IsVisible,
						TotalAmount = x.EntranceTypes
							.Where(y => y.State != EntranceTypeState.Deleted)
							.Sum(y => y.Entrances
								.Where(z => z.State != EntranceState.Deleted)
								.Sum(z => (decimal?) z.TicketLine.Amount)
							) ?? 0//,
						//TotalExtraPrice = x.EntranceTypes
						//.Where(y => y.State != EntranceTypeState.Deleted)
						//.Sum(y => y.Entrances
						//		.Where(z => z.State != EntranceState.Deleted)
						//		.Count() * y.ExtraPrice)
					})
					.ToList()
					.Select(x => new EventGetAllResult
					{
						Id = x.Id,
						Place = x.Place,
						Name = x.Name,
						EventStart = (x.EventStart == XpDateTime.MinValue) ? (DateTime?)null : x.EventStart.ToUTC(),
						EventEnd = (x.EventEnd == XpDateTime.MaxValue) ? (DateTime?)null : x.EventEnd.ToUTC(),
						State = x.State,
						IsVisible = x.IsVisible,
						TotalAmount = x.TotalAmount//,
						//TotalExtraPrice = x.TotalExtraPrice
					});

			return new ResultBase<EventGetAllResult>
			{
				Data = result
			};
		}
		#endregion ExecuteAsync
	}
}
