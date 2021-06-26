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
	public class TicketMobileGetPayHandler :
		IQueryBaseHandler<TicketMobileGetPayArguments, TicketMobileGetPayResult>
	{
		private readonly ISessionData SessionData;
		private readonly IEntityRepository<Ticket> Repository;
		private readonly IEntityRepository<PaymentMedia> PaymentMediaRepository;
		private readonly IInternalService InternalService;

		#region Constructors
		public TicketMobileGetPayHandler(
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
		public async Task<ResultBase<TicketMobileGetPayResult>> ExecuteAsync(TicketMobileGetPayArguments arguments)
		{
			var now = DateTime.Now;
			var hasPayment = true;

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
					Until = x.Until,
					State = x.State,
					CanReturn = ((x.Concession.Concession.Supplier.Login == SessionData.Login) || (x.PaymentWorker.Login == SessionData.Login)),
					SupplierName = x.SupplierName,
					SupplierTaxName = x.TaxName,
					SupplierAddress = x.TaxAddress,
					SupplierNumber = x.TaxNumber,
					SupplierPhone = x.Concession.Phone,
					HasUntil = (x.Shipment != null),
					WorkerName = (x.PaymentWorker != null) ? x.PaymentWorker.Name : x.SupplierName,
					Lines = x.Lines
					    .Select( z => new
						{
							Amount = z.Amount,
							Title = z.Title,
							Quantity = z.Quantity
						}),			               
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
							PaymentMediaName = (y.PaymentMedia.Type != PaymentMediaType.Purse) ? y.PaymentMedia.NumberHash : y.PaymentMedia.Name,
							Date = y.Date,
							State = y.State,
							CanBeReturned = ((y.RefundTo.Sum(z => (decimal?)z.Amount) ?? 0) < y.Amount) && (y.RefundFromId == null) && (y.LiquidationId == null)
						}),
					Recharges = x.Recharges
						.Select(y => new
						{
							Id = y.Id,
							Amount = y.Amount,
							UserName = y.TaxName,
							PaymentMediaName = (y.PaymentMedia.Type != PaymentMediaType.Purse) ? y.PaymentMedia.NumberHash : y.PaymentMedia.Name,
							Date = y.Date
						}),
				})
				.ToList()
				.Select(x => new TicketMobileGetPayResult
				{
					Id = x.Id,
					Reference = x.Reference,
					Amount = x.Amount,
					PayedAmount = x.PayedAmount,
					Date = x.Date.ToUTC(),
					Until = x.Until.ToUTC(),
					State = x.State,
					CanReturn = x.CanReturn,
					SupplierName = x.SupplierName,
					SupplierTaxName = x.SupplierTaxName,
					SupplierAddress = x.SupplierAddress,
					SupplierNumber = x.SupplierNumber,
					SupplierPhone = x.SupplierPhone,
					WorkerName = x.WorkerName,
					HasUntil = x.HasUntil,
					Payments = x.Payments.Select(y => new TicketMobileGetPayResult_Payment
					{
						Id = y.Id,
						Amount = y.Amount,
						UserName = y.UserName,
						PaymentMediaName = y.PaymentMediaName,
						Date = y.Date.ToUTC(),
						State = y.State == PaymentState.Pending ? PaymentState.Error : y.State,
						CanBeReturned = y.CanBeReturned
					}),
					Lines = x.Lines.Select(z => new TicketMobileGetPayResult_TicketLine
					{
						Amount = z.Amount,
						Title = z.Title,
						Quantity = z.Quantity
					}),
					Recharges = x.Recharges.Select(y => new TicketMobileGetPayResult_Recharge
					{
						Id = y.Id,
						Amount = y.Amount,
						UserName = y.UserName,
						PaymentMediaName = y.PaymentMediaName,
						Date = y.Date.ToUTC()
					})
				});

			var paymentMedias = (await PaymentMediaRepository.GetAsync("Purse"))
				.Where(x => 
						(x.State == PaymentMediaState.Active) &&
						(x.Login == SessionData.Login) &&
						((x.Purse != null && x.Purse.Expiration != null) ? x.Purse.Expiration >= now : true) &&
						((x.Purse != null && x.Purse.Validity != null) ? x.Purse.Validity >= now : true)
					)
				.ToList()
				.Select(x => new TicketMobileGetPayResultBase.PaymentMedia
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
					BankEntity = x.BankEntity,
					Image = x.Purse != null ? x.Purse.Image : null
				})
				.ToList(); // Necesario porque sino al modificar el valor de Balance Linq no lo hace correctamente al usar otra copia.

			foreach (var pMedia in paymentMedias)
			{
				var res = await InternalService.PaymentMediaGetBalanceAsync(pMedia.Id);
				if (res != null)
					pMedia.Balance = res.Balance;
			}

			return new TicketMobileGetPayResultBase
			{
				HasPayment = hasPayment,  
				Data = result,
				PaymentMedias = paymentMedias
					.Where(x =>
						(x.Type == PaymentMediaType.WebCard) ||
						(x.Type == PaymentMediaType.Purse && x.Balance > 0) // No sea monedero o que lo sea con valor distinto de 0
					)
			};
		}
		#endregion ExecuteAsync
	}
}
