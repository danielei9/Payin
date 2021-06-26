using Microsoft.Practices.Unity;
using PayIn.Application.Dto.Payments.Arguments;
using PayIn.Application.Dto.Payments.Results;
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
	public class ApiEventGetAllHandler : 
		IQueryBaseHandler<ApiEventGetAllArguments, ApiEventGetAllResult>
	{
		[Dependency] public IEntityRepository<Event> Repository { get; set; }
		[Dependency] public IEntityRepository<Ticket> TicketRepository { get; set; }
		[Dependency] public PaymentConcessionGetSelectorHandler PaymentConcessionGetSelectorHandler { get; set; }

		#region ExecuteAsync
		public async Task<ResultBase<ApiEventGetAllResult>> ExecuteAsync(ApiEventGetAllArguments arguments)
		{
			var tickets = (await TicketRepository.GetAsync());

			var paymentConcessions = (await PaymentConcessionGetSelectorHandler.ExecuteInternalAsync(""))
				.ToList();
			var paymentConcession =
				paymentConcessions.Where(x => x.Id == arguments.PaymentConcessionId).FirstOrDefault() ??
				paymentConcessions.FirstOrDefault();

			var events = (await Repository.GetAsync())
				.Where(x =>
					x.PaymentConcessionId == paymentConcession.Id
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
					Name = x.Name.Replace("\"", "'"),
					x.EventStart,
					x.EventEnd,
					x.State,
					x.IsVisible,
					x.Visibility,
					TotalAmount = x.EntranceTypes
						.Where(y => y.State != EntranceTypeState.Deleted)
						.Sum(y => y.Entrances
							.Where(z =>
								z.State == EntranceState.Active ||
								z.State == EntranceState.Validated ||
								z.State == EntranceState.Suspended
							)
							.Sum(z => (decimal?)z.TicketLine.Amount)
						) ?? 0,
					EntrancesTypes = x.EntranceTypes
						.Where(y =>
							y.State != EntranceTypeState.Deleted
						)
						.Count(),
					//HasNotices = x.Notices.Any(j => j.State != NoticeState.Deleted),
					//HasActivities = x.Activities.Any(j => j.Id > 0),
					//HasTickets = tickets.Any(y => y.EventId == x.Id),
					//HasExhibitors = x.Exhibitors.Any(j =>
					//	j.State != ExhibitorState.Deleted &&
					//	j.Events.Any(k => k.Id == x.Id)),
					//Notices = 99,
					//Activities = 99,
					//Tickets = 99,
					//Exhibitors = 99,
					Notices = x.Notices
						.Where(j =>
							j.State != NoticeState.Deleted)
						.Count(),
					Activities = x.Activities.Count(),
					Tickets = tickets // Este tarda mucho
						.Where(y =>
							y.EventId == x.Id)
						.Count(),
					Exhibitors = x.Exhibitors
						.Where(j =>
							j.State != ExhibitorState.Deleted)
						.SelectMany(y =>
							y.Events)
						.Where(z =>
							z.Id == x.Id)
						.Count(),
					TotalExtraPrice = x.EntranceTypes.SelectMany(y => y.TicketLines).Where(z => z.Type == TicketLineType.ExtraPrice)
						.Sum(j => (decimal?)j.Quantity * j.Amount) ?? 0
				})
                .OrderBy(x => x.EventEnd)
				.ToList()
				.Select(x => new ApiEventGetAllResult
				{
					Id = x.Id,
					Name = x.Name,
					EventStart = (x.EventStart == XpDateTime.MinValue) ? (DateTime?)null : x.EventStart.ToUTC(),
					EventEnd = (x.EventEnd == XpDateTime.MaxValue) ? (DateTime?)null : x.EventEnd.ToUTC(),
					State = x.State,
					IsVisible = x.IsVisible,
					Visibility = x.Visibility,
					TotalAmount = x.TotalAmount,
					EntrancesTypes = x.EntrancesTypes,
					Notices = x.Notices,
					Activities = x.Activities,
					Tickets = x.Tickets,
					Exhibitors = x.Exhibitors,
					TotalExtraPrice = x.TotalExtraPrice,	
					//HasNotices = x.HasNotices,
					//HasActivities = x.HasActivities,
					//HasExhibitors = x.HasExhibitors,
					//HasTickets = x.HasTickets
				});

			return new ApiEventGetAllResultBase
			{
				Data = result,
				PaymentConcessionId = paymentConcession?.Id,
				PaymentConcessionName = paymentConcession?.Value ?? "",
				PaymentConcessions = paymentConcessions
			};
		}
		#endregion ExecuteAsync
	}
}
