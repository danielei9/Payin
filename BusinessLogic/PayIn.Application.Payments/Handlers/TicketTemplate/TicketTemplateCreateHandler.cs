using PayIn.Application.Dto.Payments.Arguments.TicketTemplate;
using PayIn.Application.Dto.Payments.Results.TicketTemplate;
using PayIn.BusinessLogic.Common;
using PayIn.Common;
using PayIn.Common.Resources;
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

namespace PayIn.Application.Payments.Handlers
{
    public class TicketTemplateCreateHandler :
		IServiceBaseHandler<TicketTemplateCreateArguments>
	{
		private readonly ISessionData SessionData;
		private readonly IEntityRepository<TicketTemplate>Repository;
		private readonly IEntityRepository<Ticket>TicketRepository;
		private readonly IEntityRepository<PaymentConcession>PaymentConcessionRepository;

		#region Constructor
		public TicketTemplateCreateHandler(
			ISessionData sessionData,
			IEntityRepository<TicketTemplate> repository,
			IEntityRepository<Ticket> ticketRepository,
			IEntityRepository<PaymentConcession> paymentConcessionRepository
		)
		{
			if (sessionData == null) throw new ArgumentNullException("sessionData");
			if (repository == null) throw new ArgumentNullException("repository");
			if (ticketRepository == null) throw new ArgumentNullException("ticketRepository");
			if (paymentConcessionRepository == null) throw new ArgumentNullException("paymentConcessionRepository");

			SessionData = sessionData;
			Repository = repository;
			TicketRepository = ticketRepository;
			PaymentConcessionRepository = paymentConcessionRepository;
		}
		#endregion Constructor

		#region ExecuteAsync
		public async Task<dynamic> ExecuteAsync(TicketTemplateCreateArguments arguments)
		{
			//Convertir en una única subconsulta
			var ticket = (await TicketRepository.GetAsync(arguments.TicketId));
			if (ticket == null)
				throw new ArgumentException(TicketResources.TicketNotFound, "ticketId");

			var concession = (await PaymentConcessionRepository.GetAsync(ticket.ConcessionId, "Tickets.Concession"));
			if (concession == null)
				throw new ArgumentException(PaymentConcessionResources.PaymentConcessionNotFound, "ConcessionId");
			
			var ticketTemplate = new TicketTemplate
			{
				Name = arguments.Name,
				RegEx = arguments.RegEx,
				PreviousTextPosition = arguments.PreviousTextPosition,
				BackTextPosition = arguments.BackTextPosition,
				DateFormat = arguments.DateFormat,
				DecimalCharDelimiter = arguments.DecimalCharDelimiter,
				ReferencePosition = arguments.ReferencePosition ?? null,
				TitlePosition = arguments.TitlePosition ?? null,
				DatePosition = arguments.DatePosition ?? null,
				AmountPosition = arguments.AmountPosition,
				WorkerPosition = arguments.WorkerPosition ?? null,
				IsGeneric = arguments.IsGeneric ? true : false
			};
			ticketTemplate.Tickets.Add(ticket);
			ticketTemplate.Concessions.Add(concession);
			await Repository.AddAsync(ticketTemplate);

			return ticketTemplate;
		}
		#endregion ExecuteAsync
	}
}
