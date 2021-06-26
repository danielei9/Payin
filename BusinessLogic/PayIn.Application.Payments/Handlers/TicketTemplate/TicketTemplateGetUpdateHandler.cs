using PayIn.Application.Dto.Payments.Arguments.TicketTemplate;
using PayIn.Application.Dto.Payments.Results.TicketTemplate;
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
	public class TicketTemplateGetUpdateHandler :
		IQueryBaseHandler<TicketTemplateGetUpdateArguments, TicketTemplateGetUpdateResult>
	{
		private readonly IEntityRepository<Ticket> TicketRepository;
		private readonly IBlobRepository BlobRepository;

		#region Constructors
		public TicketTemplateGetUpdateHandler(
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
		public async Task<ResultBase<TicketTemplateGetUpdateResult>> ExecuteAsync(TicketTemplateGetUpdateArguments arguments)
		{
			var ticket = (await TicketRepository.GetAsync())
				.Where(x => x.Id == arguments.TicketId)
				.Select(x => new
				 {
					TextUrl = x.TextUrl,
					TemplateId = x.TemplateId,
					TicketTemplatePrivate = x.Concession.TicketTemplate.Concessions.Count() == 1,
					RegEx = x.Concession.TicketTemplate.RegEx,
					PreviousTextPosition = x.Concession.TicketTemplate.PreviousTextPosition,
					BackTextPosition = x.Concession.TicketTemplate.BackTextPosition,
					AmountPosition = x.Concession.TicketTemplate.AmountPosition,
					DateFormat = x.Concession.TicketTemplate.DateFormat,
					DatePosition = x.Concession.TicketTemplate.DatePosition,
					DecimalCharDelimiter = x.Concession.TicketTemplate.DecimalCharDelimiter,
					ReferencePosition = x.Concession.TicketTemplate.ReferencePosition,
					TitlePosition = x.Concession.TicketTemplate.TitlePosition,
					WorkerPosition = x.Concession.TicketTemplate.WorkerPosition
				})
				 .FirstOrDefault();
			if (ticket == null)
				return new TicketTemplateGetUpdateResultBase();
			
			return new TicketTemplateGetUpdateResultBase
			{
				Data = new List<TicketTemplateGetUpdateResult>
				{
					new TicketTemplateGetUpdateResult
					{
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

					//TicketText =
					//	"     * * * CARREFOUR EXPRESS * * *    \r\n" +
					//	"      C/NICOLAS ALCORTA - BILBAO      \r\n" +
					//	"TF.944433866 TF. ATT CLIENTE 902202000\r\n" +
					//	" * * * * * PVP IVA INCLUIDO * * * * * \r\n" +
					//	" 1  T.SIM INICIO C-MOV            5,00\r\n" +
					//	" 1 # TELEREC C_MOVIL 10          10,00\r\n" +
					//	"--------------------------------------\r\n" +
					//	"    2   ART  TOT COMPRA:      1.005,00\r\n" +
					//	"                   PTAS:         2.496\r\n" +
					//	"--------------------------------------\r\n" +
					//	"PAGADO Promo CARREF Movil         5,00\r\n" +
					//	"PAGADO METALICO                  10,00\r\n" +
					//	"CAMBIO RECIBIDO                   0,00\r\n" +
					//	"PLAZO DEVOLUC. 15 DIAS CONSERVE TICKET\r\n" +
					//	"                                      \r\n" +
					//	"                                      \r\n" +
					//	"25/08/08 20:54:23                    \r \n" +
					//	"LE ATENDIO:      THAIS                ";
		}
		#endregion ExecuteAsync
	}
}