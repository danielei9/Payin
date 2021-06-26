using PayIn.Application.Dto.Payments.Arguments.TicketTemplate;
using PayIn.Application.Dto.Payments.Results.TicketTemplate;
using PayIn.Common;
using PayIn.Domain.Payments;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using Xp.Application.Dto;
using Xp.Common;
using Xp.Domain;
using Xp.Infrastructure;

namespace PayIn.Application.Payments.Handlers
{
    public class TicketTemplateCheckHandler :
		IQueryBaseHandler<TicketTemplateCheckArguments, TicketTemplateCheckResult>
	{
		private readonly IEntityRepository<Ticket> TicketRepository;
		private readonly IBlobRepository BlobRepository;
		private readonly TicketTemplateCheckInternalHandler CheckHandler;

		#region Constructors
		public TicketTemplateCheckHandler(
			IEntityRepository<Ticket> ticketRepository,
			IBlobRepository blobRepository,
			TicketTemplateCheckInternalHandler checkHandler
		)
		{
			if (ticketRepository == null) throw new ArgumentNullException("ticketRepository");
			if (blobRepository == null) throw new ArgumentNullException("blobRepository");
			if (checkHandler == null) throw new ArgumentNullException("checkHandler");
			TicketRepository = ticketRepository;
			BlobRepository = blobRepository;
			CheckHandler = checkHandler;
		}
		#endregion Constructors

		#region ExecuteAsync
		public async Task<ResultBase<TicketTemplateCheckResult>> ExecuteAsync(TicketTemplateCheckArguments arguments)
		{
            var result = new TicketTemplateCheckResultBase();

			var ticket = (await TicketRepository.GetAsync(arguments.TicketId));
			if (ticket == null)
				throw new ArgumentNullException("ticketId");
			var text = await BlobRepository.LoadStringUrlAsync(ticket.TextUrl);

			return await CheckHandler.ExecuteAsync(new TicketTemplateCheckInternalArguments(
				text,
				arguments.RegEx,
				arguments.DecimalCharDelimiter,
				arguments.AmountPosition,
				arguments.ReferencePosition,
				arguments.DatePosition,
				arguments.DateFormat,
				arguments.TitlePosition,
				arguments.WorkerPosition
			));
		}
		#endregion ExecuteAsync
	}
}
