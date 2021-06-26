using PayIn.Application.Dto.Payments.Arguments;
using PayIn.Application.Dto.Payments.Results;
using PayIn.BusinessLogic.Common;
using PayIn.Common;
using PayIn.Domain.Payments;
using PayIn.Domain.Payments.Infrastructure;
using System;
using System.Linq;
using System.Threading.Tasks;
using Xp.Application.Dto;
using Xp.Domain;

namespace PayIn.Application.Payments.Handlers
{
	public class TicketMobileGetHandler :
		IQueryBaseHandler<TicketMobileGetArguments, TicketMobileGetResult>
	{
		private readonly ISessionData SessionData;
		private readonly IEntityRepository<Ticket> Repository;
		private readonly IEntityRepository<PaymentMedia> PaymentMediaRepository;
		private readonly IInternalService InternalService;

		#region Constructors
		public TicketMobileGetHandler(
			ISessionData sessionData,
			IEntityRepository<Ticket> repository,
			IEntityRepository<PaymentMedia> paymentMediaRepository,
			IInternalService internalService
		)
		{
			if (sessionData == null) throw new ArgumentNullException("sessionData");
			if (repository == null) throw new ArgumentNullException("repository");
			if (paymentMediaRepository == null) throw new ArgumentNullException("paymentMediaRepository");
			if (internalService == null) throw new ArgumentNullException("internalService");

			SessionData = sessionData;
			Repository = repository;
			PaymentMediaRepository = paymentMediaRepository;
			InternalService = internalService;
		}
		#endregion Constructors

		#region ExecuteAsync
		public async Task<ResultBase<TicketMobileGetResult>> ExecuteAsync(TicketMobileGetArguments arguments)
		{
			var hasPayment = await InternalService.UserHasPaymentAsync();

			var result = (await Repository.GetAsync())
				.Where(x => x.Id == arguments.Id)
				.Select(x => new
				{
					Id = x.Id,
					Reference = x.Reference,
					Amount = x.Amount,
					PayedAmount = x.Payments
						.Where(y =>
							y.State != PaymentState.Error &&
							y.State != PaymentState.Pending
						)
						.Sum(y => (decimal?)y.Amount) ?? 0m,
					Date = x.Date,
					State = x.State,
					CanReturn = ((x.Concession.Concession.Supplier.Login == SessionData.Login) || (x.PaymentWorker.Login == SessionData.Login)),
					SupplierName = x.SupplierName,
					SupplierTaxName = x.TaxName,
					SupplierAddress = x.TaxAddress,
					SupplierNumber = x.TaxNumber,
					SupplierPhone = x.Concession.Phone,
					WorkerName = (x.PaymentWorker != null) ? x.PaymentWorker.Name : "",
					Type = x.Type,
					Payments = x.Payments
						.Where(y =>
							(
								(
									x.Concession.Concession.Supplier.Login == SessionData.Login ||
									(
										x.PaymentWorker.Login == SessionData.Login &&
										x.PaymentWorker.State == WorkerState.Active
									)
								) &&
								y.State != PaymentState.Error
							) ||
							y.UserLogin == SessionData.Login)
						.Select(y => new
						{
							Id = y.Id,
							Amount = y.Amount,
							UserName = y.TaxName,
							PaymentMediaName = y.PaymentMedia.Type == PaymentMediaType.Purse ? y.PaymentMedia.Name : y.PaymentMedia.NumberHash,
							Date = y.Date,
							State = y.State,
							CanBeReturned = ((y.RefundTo.Sum(z => (decimal?)z.Amount) ?? 0) < y.Amount) && (y.RefundFromId == null) && (y.LiquidationId == null)
						}).OrderByDescending(y => y.Date),
					Lines = x.Lines
						.Select(y => new
						{
							Id = y.Id,
							Title = y.Title,
							Amount = y.Amount,
							Quantity = y.Quantity
						})
				})
				.ToList()
				.Select(x => new TicketMobileGetResult
				{
					Id = x.Id,
					Reference = x.Reference,
					Amount = x.Amount,
					PayedAmount = x.PayedAmount,
					Date = x.Date.ToUTC(),
					State = x.State,
					CanReturn = x.CanReturn,
					SupplierName = x.SupplierName,
					SupplierTaxName = x.SupplierTaxName,
					SupplierAddress = x.SupplierAddress,
					SupplierNumber = x.SupplierNumber,
					SupplierPhone = x.SupplierPhone,
					WorkerName = x.WorkerName,
					Type = x.Type,
					Payments = x.Payments
						.Select(y => new TicketMobileGetResult_Payment
						{
							Id = y.Id,
							Amount = y.Amount,
							UserName = y.UserName,
							PaymentMediaName = y.PaymentMediaName,
							Date = y.Date.ToUTC(),
							State = y.State == PaymentState.Pending ? PaymentState.Error : y.State,
							CanBeReturned = y.CanBeReturned
						}),
					Lines = x.Lines
						.Select(y => new TicketMobileGetResult_TicketLine
						{
							Id = y.Id,
							Title = y.Title,
							Amount = y.Amount,
							Quantity = y.Quantity
						})
				});

			var paymentMedias = (await PaymentMediaRepository.GetAsync())
				.Where(x => x.State == PaymentMediaState.Active && x.Login == SessionData.Login)
				.Select(x => new TicketMobileGetResultBase.PaymentMedia
				{
					Id = x.Id,
					Title = x.Name,
					Subtitle = x.Type.ToString(),
					VisualOrder = x.VisualOrder,
					NumberHash = x.NumberHash,
					ExpirationMonth = x.ExpirationMonth,
					ExpirationYear = x.ExpirationYear,
					Type = x.Type,
					State = x.State,
					BankEntity = x.BankEntity
				}).ToList();

			foreach (var pMedia in paymentMedias)
			{
				if (pMedia.Type == PaymentMediaType.Purse)
				{
					var res = await InternalService.PaymentMediaGetBalanceAsync(pMedia.Id);
					if (res != null)
						pMedia.Balance = res.Balance;
				}
			}

			return new TicketMobileGetResultBase
			{
				//HasPayment = hasPayment,
				Data = result,
				PaymentMedias = paymentMedias.Where(x => (x.Balance > 0 && x.Type == PaymentMediaType.Purse) || x.Type != PaymentMediaType.Purse) // Solo devuelve aquellos monederos cuyo balance es superior a 0
			};
		}
		#endregion ExecuteAsync
	}
}
