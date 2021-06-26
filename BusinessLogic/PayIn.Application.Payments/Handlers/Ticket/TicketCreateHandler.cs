using PayIn.Application.Dto.Payments.Arguments;
using PayIn.BusinessLogic.Common;
using PayIn.Common;
using PayIn.Common.Resources;
using PayIn.Domain.Payments;
using System;
using System.Linq;
using System.Threading.Tasks;
using Xp.Application.Attributes;
using Xp.Application.Dto;
using Xp.Common.Exceptions;
using Xp.Domain;

namespace PayIn.Application.Payments.Handlers
{
	[XpLog("Ticket", "Create")]
	[XpAnalytics("Ticket", "Create")]
	public class TicketCreateHandler :
		IServiceBaseHandler<TicketCreateArguments>
	{
		private readonly ISessionData SessionData;
		private readonly IEntityRepository<Ticket> Repository;
		private readonly IEntityRepository<TicketLine> TicketLineRepository;
		private readonly IEntityRepository<PaymentConcession> PaymentConcessionRepository;
		private readonly IEntityRepository<PaymentWorker> PaymentWorkerRepository;
		private readonly IUnitOfWork UnitOfWork;

		#region Contructors
		public TicketCreateHandler(
			ISessionData sessionData,
			IEntityRepository<Ticket> repository,
			IEntityRepository<TicketLine> ticketLineRepository,
			IEntityRepository<PaymentConcession> paymentConcessionRepository,
			IEntityRepository<PaymentWorker> paymentWorkerRepository, 
			IUnitOfWork unitOfWork
		)
		{
			if (sessionData == null) throw new ArgumentNullException("sessionData");
			if (repository == null) throw new ArgumentNullException("repository");
			if (ticketLineRepository == null) throw new ArgumentNullException("ticketLineRepository");
			if (paymentConcessionRepository == null) throw new ArgumentNullException("paymentsConcessionRepository");
			if (paymentWorkerRepository == null) throw new ArgumentNullException("paymentsWorkerRepository");
			if (unitOfWork == null) throw new ArgumentNullException("unitOfWork");

			SessionData = sessionData;
			Repository = repository;
			TicketLineRepository = ticketLineRepository;
			PaymentConcessionRepository = paymentConcessionRepository;
			PaymentWorkerRepository = paymentWorkerRepository;
			UnitOfWork = unitOfWork;
		}
		#endregion Contructors

		#region ExecuteAsync
		public async Task<dynamic> ExecuteAsync(TicketCreateArguments arguments)
		{
			var concession = (await PaymentConcessionRepository.GetAsync("Concession.Supplier"))
				.Where(x => x.ConcessionId == arguments.ConcessionId)
				.FirstOrDefault();

			var now = DateTime.Now;

			if (arguments.Since != null && arguments.Until != null)
			  if ((arguments.Since.Value >= arguments.Until.Value))
				throw new XpException(ControlPlanningResources.SincePreviousUntilException);

			if (concession == null)
				throw new ArgumentException(TicketResources.CreateNonActiveConcessionException, "concessionId");

			if (concession.Concession.State != ConcessionState.Active)
				throw new ArgumentException(TicketResources.CreateNonActiveConcessionException, "concessionId");

			var worker = (await PaymentWorkerRepository.GetAsync())
				.Where(x => x.Login == SessionData.Login && x.State == WorkerState.Active)
				.FirstOrDefault();

			var date = DateTime.Now;
			var dateSince = (arguments.Since == null) ? DateTime.Now : arguments.Since.Value.ToUTC();
			var dateUntil = date.AddHours(6);			

			var ticket = new Ticket
			{
				Date = date,
				Amount = arguments.Amount,
				Reference = arguments.Reference,
				Concession = concession,
				PaymentWorker = worker,
				SupplierName = concession.Concession.Supplier.Name,
				TaxName = concession.Concession.Supplier.TaxName,
				TaxNumber = concession.Concession.Supplier.TaxNumber,
				TaxAddress = concession.Concession.Supplier.TaxAddress,
				State =
					arguments.Type == TicketType.Order ? TicketStateType.Created :
					TicketStateType.Active, // TicketType.Ticket
				Since = dateSince,
				Until = (arguments.Until == null) ? dateUntil : arguments.Until.Value.ToUTC(),
				TextUrl = "",
				PdfUrl = "",
				Type = arguments.Type
			};
			if (arguments.Lines != null)
			{
				foreach (var line in arguments.Lines)
				{
					var ticketLine = new TicketLine
					{
						Title = line.Title.IsNullOrEmpty() ? TicketResources.Several : line.Title,
						Amount = line.Amount,
						Quantity = line.Quantity,
						Type = TicketLineType.Buy
					};
					ticket.Lines.Add(ticketLine);
					var campaignLines = ticket.Lines
						.SelectMany(x => x.EntranceType.CampaignLines)
						.Union(
							ticket.Lines
								.SelectMany(y => y.Product.CampaignLines)
						)
						.Where(y =>
							y.State == CampaignLineState.Active &&
							y.Campaign.State == CampaignState.Active &&
							(y.SinceTime <= now || y.SinceTime == null) &&
							(y.UntilTime >= now || y.UntilTime == null) &&
							y.Campaign.Since <= now &&
							y.Campaign.Until >= now
						);
					foreach (var campaignLine in campaignLines)
					{
						if (campaignLine.Type == CampaignLineType.DirectDiscount)
						{
							ticket.Lines.Add(new TicketLine
							{
								Title = "Dto. " + ticketLine.Title,
								Amount = -campaignLine.Quantity,
								Quantity = ticketLine.Quantity,
								From = ticketLine,
								Type = TicketLineType.Discount,
								CampaignLineId = campaignLine.Id,
								CampaignLineType = campaignLine.Type,
								CampaignLineQuantity = campaignLine.Quantity
							});
						}
						if (campaignLine.Type == CampaignLineType.DirectPrice)
						{
							ticket.Lines.Add(new TicketLine
							{
								Title = "Dto. " + ticketLine.Title,
								Amount = campaignLine.Quantity - line.Amount,
								Quantity = ticketLine.Quantity,
								From = ticketLine,
								Type = TicketLineType.Discount,
								CampaignLineId = campaignLine.Id,
								CampaignLineType = campaignLine.Type,
								CampaignLineQuantity = campaignLine.Quantity
							});
						}
						if (campaignLine.Type == CampaignLineType.DirectPercentDiscount)
						{
							ticket.Lines.Add(new TicketLine
							{
								Title = "Dto. " + ticketLine.Title,
								Amount = -(Math.Min(1, campaignLine.Quantity / 100) * line.Amount),
								Quantity = ticketLine.Quantity,
								From = ticketLine,
								Type = TicketLineType.Discount,
								CampaignLineId = campaignLine.Id,
								CampaignLineType = campaignLine.Type,
								CampaignLineQuantity = campaignLine.Quantity
							});
						}
					}
				}
			}		
			
			await Repository.AddAsync(ticket);

			return ticket;
		}
		#endregion ExecuteAsync
	}
}
