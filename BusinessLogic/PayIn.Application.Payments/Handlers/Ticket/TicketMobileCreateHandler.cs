using PayIn.Application.Dto.Payments.Arguments;
using PayIn.BusinessLogic.Common;
using PayIn.Common;
using PayIn.Common.Resources;
using PayIn.Domain.Payments;
using PayIn.Domain.Transport;
using System;
using System.Linq;
using System.Threading.Tasks;
using Xp.Application.Attributes;
using Xp.Application.Dto;
using Xp.Domain;

namespace PayIn.Application.Payments.Handlers
{
	[XpLog("Ticket", "Create")]
	[XpAnalytics("Ticket", "Create", Arguments = new[] { "Amount" })]
	public class TicketMobileCreateHandler :
		IServiceBaseHandler<TicketMobileCreateArguments>
	{
		private readonly ISessionData SessionData;
		private readonly IEntityRepository<Ticket> Repository;
		private readonly IEntityRepository<PaymentConcession> PaymentConcessionRepository;
		private readonly IEntityRepository<PaymentWorker> PaymentWorkerRepository;
		private readonly IEntityRepository<TransportConcession> TransportConcessionRepository;
		private readonly IUnitOfWork UnitOfWork;

		#region Contructors
		public TicketMobileCreateHandler(
			ISessionData sessionData,
            IEntityRepository<Ticket> repository,
			IEntityRepository<PaymentConcession> paymentConcessionRepository,
			IEntityRepository<TransportConcession> transportConcessionRepository,
			IEntityRepository<PaymentWorker> paymentWorkerRepository,
			IUnitOfWork unitOfWork
		)
		{
			if (sessionData == null) throw new ArgumentNullException("sessionData");
			if (repository == null) throw new ArgumentNullException("repository");
			if (paymentConcessionRepository == null) throw new ArgumentNullException("paymentsConcessionRepository");
			if (transportConcessionRepository == null) throw new ArgumentNullException("transportConcessionRepository");
			if (paymentWorkerRepository == null) throw new ArgumentNullException("paymentsWorkerRepository");
			if (unitOfWork == null) throw new ArgumentNullException("unitOfWork");

			SessionData = sessionData;
			Repository = repository;
			PaymentConcessionRepository = paymentConcessionRepository;
			TransportConcessionRepository = transportConcessionRepository;
			PaymentWorkerRepository = paymentWorkerRepository;
			UnitOfWork = unitOfWork;
		}
		#endregion Contructors

		#region ExecuteAsync
		public async Task<dynamic> ExecuteAsync(TicketMobileCreateArguments arguments)
		{
			var date = DateTime.Now.ToUTC();

			var concession = (await PaymentConcessionRepository.GetAsync("Concession.Supplier"))
				.Where(x => x.Id == arguments.ConcessionId)
				.FirstOrDefault();

			TransportConcession liquidationConcession = null;

			if (arguments.LiquidationConcession != null)
				 liquidationConcession = (await TransportConcessionRepository.GetAsync("PaymentConcession"))
					.Where(x => x.Id == arguments.LiquidationConcession)
					.FirstOrDefault();

			if (concession.Concession.State != ConcessionState.Active)
				throw new ArgumentException(TicketResources.CreateNonActiveConcessionException, "concessionId");

			var worker = (await PaymentWorkerRepository.GetAsync())
				.Where(x => x.Login == SessionData.Login && x.State == WorkerState.Active)
				.FirstOrDefault();

			var ticket = new Ticket
			{
				Date = arguments.Date.Value.ToUTC(),
				Amount = arguments.Lines.Sum(x => x.Amount),
				Reference = arguments.Reference,
				ConcessionId = concession.Id,
				Concession = concession,
				PaymentWorkerId = (worker != null) ? (int?)worker.Id : null,
				PaymentWorker = (worker == null) ? null : worker,
				SupplierName = concession.Concession.Supplier.Name,
				TaxName = concession.Concession.Supplier.TaxName,
				TaxNumber = concession.Concession.Supplier.TaxNumber,
				TaxAddress = concession.Concession.Supplier.TaxAddress,
				State =
					arguments.Type == TicketType.Order ? TicketStateType.Created :
					TicketStateType.Active, // TicketType.Ticket
				Since = date,
				Until = date.AddHours(6),
				TextUrl = "",
				PdfUrl = "",
				LiquidationConcession = (liquidationConcession == null) ? null : liquidationConcession.PaymentConcession,
				Type = arguments.Type
			};
			if (arguments.Lines != null)
			{
				foreach (var ticketLine in arguments.Lines)
				{
					ticket.Lines.Add(new TicketLine
					{
						Title = ticketLine.Title.IsNullOrEmpty() ? TicketResources.Several : ticketLine.Title,
						Amount = ticketLine.Amount,
						Quantity = ticketLine.Quantity,
						Type = TicketLineType.Buy
					});
				}
			}
			await Repository.AddAsync(ticket);

			return ticket;
		}
		#endregion ExecuteAsync
	}
}
