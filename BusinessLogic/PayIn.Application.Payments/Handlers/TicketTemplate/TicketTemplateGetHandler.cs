using PayIn.Application.Dto.Payments.Arguments.TicketTemplate;
using PayIn.Application.Dto.Payments.Results.TicketTemplate;
using PayIn.Common;
using PayIn.Domain.Payments;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xp.Application.Dto;
using Xp.Domain;
using Xp.Infrastructure;

namespace PayIn.Application.Payments.Handlers
{
	public class TicketTemplateGetHandler :
		IQueryBaseHandler<TicketTemplateGetArguments, TicketTemplateGetResult>
	{
		private readonly IEntityRepository<Ticket> TicketRepository;
		private readonly IBlobRepository BlobRepository;

		#region Constructors
		public TicketTemplateGetHandler(
			IEntityRepository<Ticket> ticketRepository,
			IBlobRepository blobRepository
		)
		{
			if (ticketRepository == null) throw new ArgumentNullException("ticketRepository");
			if (blobRepository == null) throw new ArgumentNullException("blobRepository");
			TicketRepository = ticketRepository;
			BlobRepository = blobRepository;
		}
		#endregion Constructors

		#region ExecuteAsync
		public async Task<ResultBase<TicketTemplateGetResult>> ExecuteAsync(TicketTemplateGetArguments arguments)
		{
			var ticket = (await TicketRepository.GetAsync())
				.Where(x => x.Id == arguments.TicketId)
				.Select(x => new
				 {
					Name = x.Concession.TicketTemplate.Name,
					TextUrl = x.TextUrl,
					TicketTemplatePrivate = x.Concession.TicketTemplate != null && x.Concession.TicketTemplate.Concessions.Count() == 1,
					RegEx = x.Concession.TicketTemplate.RegEx,
					PreviousTextPosition = x.Concession.TicketTemplate.PreviousTextPosition,
					BackTextPosition = x.Concession.TicketTemplate.BackTextPosition,
					AmountPosition = x.Concession.TicketTemplate == null ? 0 : x.Concession.TicketTemplate.AmountPosition,
					DateFormat = x.Concession.TicketTemplate.DateFormat,
					DatePosition = x.Concession.TicketTemplate.DatePosition,
					DecimalCharDelimiter = x.Concession.TicketTemplate == null ? DecimalCharDelimiter.Comma : x.Concession.TicketTemplate.DecimalCharDelimiter,
					ReferencePosition = x.Concession.TicketTemplate.ReferencePosition,
					TitlePosition = x.Concession.TicketTemplate.TitlePosition,
					WorkerPosition = x.Concession.TicketTemplate.WorkerPosition
				})
				 .FirstOrDefault();
			if (ticket == null)
				return new TicketTemplateGetResultBase();
			
			return new TicketTemplateGetResultBase
			{
				Data = new List<TicketTemplateGetResult>
				{
					new TicketTemplateGetResult
					{
						Name = ticket.Name,
						TicketTemplatePrivate = ticket.TicketTemplatePrivate,
						RegEx = ticket.RegEx,
						PreviousTextPosition = ticket.PreviousTextPosition,
						BackTextPosition = ticket.BackTextPosition,
						AmountPosition = ticket.AmountPosition,
						DateFormat = ticket.DateFormat,
						DatePosition = ticket.DatePosition,
						DecimalCharDelimiter = ticket.DecimalCharDelimiter,
						ReferencePosition = ticket.ReferencePosition,
						TitlePosition = ticket.TitlePosition,
						WorkerPosition = ticket.WorkerPosition
					}
                },
                TicketText = ticket.TextUrl == "" ? "" : await BlobRepository.LoadStringUrlAsync(ticket.TextUrl)
			};
		}
		#endregion ExecuteAsync
	}
}