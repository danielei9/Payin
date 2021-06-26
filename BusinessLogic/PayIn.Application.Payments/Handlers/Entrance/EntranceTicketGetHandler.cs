using Microsoft.Practices.Unity;
using PayIn.Application.Dto.Payments.Arguments;
using PayIn.Application.Dto.Payments.Results;
using PayIn.Domain.Payments;
using System;
using System.Linq;
using System.Threading.Tasks;
using Xp.Application.Dto;
using Xp.Common;
using Xp.Domain;

namespace PayIn.Application.Payments.Handlers
{
	public class EntranceTicketGetHandler :
		IQueryBaseHandler<EntranceTicketGetArguments,EntranceTicketGetResult>
	{
		[Dependency] public IEntityRepository<Entrance> Repository { get; set; }

		#region ExecuteAsync
		public async Task<ResultBase<EntranceTicketGetResult>> ExecuteAsync(EntranceTicketGetArguments arguments)
		{
			var entranceTicket = (await Repository.GetAsync())
				.Where(x =>
					x.Id == arguments.Id &&
					x.TicketLineId == x.TicketLine.Id &&
					x.TicketLine.TicketId == x.TicketLine.Ticket.Id
				)
				.Select(x => new 
				{
					Id = x.Id,
					Amount = x.TicketLine.Ticket.Amount,
					Date = (x.TicketLine.Ticket.Date == XpDateTime.MinValue) ? null : (DateTime?)x.TicketLine.Ticket.Date,
					Since = (x.TicketLine.Ticket.Since == XpDateTime.MinValue) ? null : (DateTime?)x.TicketLine.Ticket.Since,
					Until = (x.TicketLine.Ticket.Until == XpDateTime.MinValue) ? null : (DateTime?)x.TicketLine.Ticket.Until,
					SupplierName = x.TicketLine.Ticket.SupplierName,
					TaxAddress = x.TicketLine.Ticket.TaxAddress,
					TaxName = x.TicketLine.Ticket.TaxName,
					TaxNumber = x.TicketLine.Ticket.TaxNumber
				})
				.ToList()
				.Select(x => new EntranceTicketGetResult
				{
					Id = x.Id,
					Amount = x.Amount,
					Date = x.Date.ToUTC(),
					Since = x.Since.ToUTC(),
					Until = x.Until.ToUTC(),
					SupplierName = x.SupplierName,
					TaxAddress = x.TaxAddress,
					TaxName = x.TaxName,
					TaxNumber = x.TaxNumber
				});

			return new ResultBase<EntranceTicketGetResult> { Data = entranceTicket };
		}
			#endregion ExecuteAsync
	}
}
