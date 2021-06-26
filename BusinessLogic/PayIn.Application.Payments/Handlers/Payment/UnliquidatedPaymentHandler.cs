using PayIn.Application.Dto.Payments.Arguments.Payment;
using PayIn.Application.Dto.Payments.Results.Payment;
using PayIn.BusinessLogic.Common;
using PayIn.Common;
using PayIn.Domain.Payments;
using PayIn.Infrastructure.Security;
using System;
using System.Linq;
using System.Threading.Tasks;
using Xp.Application.Dto;
using Xp.Domain;

namespace PayIn.Application.Public.Handlers
{
	public class UnliquidatedPaymentHandler :
		IQueryBaseHandler<UnliquidatedPaymentArguments, UnliquidatedPaymentResult>
	{
		private readonly IEntityRepository<Payment> Repository;
		public readonly ISessionData SessionData;

		#region Constructors
		public UnliquidatedPaymentHandler(IEntityRepository<Payment> repository,ISessionData sessionData)
		{
			if (repository == null) throw new ArgumentNullException("repository");
			Repository = repository;
			SessionData = sessionData;

		}
		#endregion Constructors

		#region ExecuteAsync
		public async Task<ResultBase<UnliquidatedPaymentResult>> ExecuteAsync(UnliquidatedPaymentArguments arguments)
		{
			var items = (await Repository.GetAsync())
				.Where(x => 
					x.LiquidationId == null &&
					x.Ticket.Concession.Concession.Supplier.Login == SessionData.Login &&
					x.State == PaymentState.Active
				);

			var result = items
				.Select(x => new
				{
					Id = x.Id,
					Ticket = x.Ticket.Reference,
					SupplierLogin = x.Ticket.Concession.Concession.Supplier.Login,
					UserLogin = x.UserLogin,
					UserName = x.UserName,
					PaymentMedia = x.PaymentMedia.Name,
					NumberHash = x.PaymentMedia.NumberHash,
					CardType = x.PaymentMedia.Type,
					Amount = x.Amount,
					TicketAmount = x.Ticket.Amount,
					TaxName = x.TaxName,
					SupplierName = x.Ticket.SupplierName,
					SupplierAddress = x.Ticket.TaxAddress,
					SupplierTaxNumber = x.Ticket.TaxNumber,
					Date = x.Date,
					State = x.State,
					RefundFromId = x.RefundFromId,
					RefundFromDate = (x.RefundFrom != null) ? (DateTime?)x.RefundFrom.Date : null,
					RefundToId = x.RefundTo.Any() ? (int?)x.RefundTo.FirstOrDefault().Id : null,
					RefundToDate = x.RefundTo.Any() ? (DateTime?)x.RefundTo.FirstOrDefault().Date : null
				})
				.ToList()
				.Select(x => new UnliquidatedPaymentResult
				{
					Id = x.Id,
					Ticket = x.Ticket,
					SupplierLogin = x.SupplierLogin,
					UserLogin = x.UserLogin,
					UserName = x.UserName,
					PaymentMedia = x.PaymentMedia,
					NumberHash = x.NumberHash,
					CardType = x.CardType,
					TaxName = x.TaxName,
					Amount = x.Amount,
					TicketAmount = x.TicketAmount,
					SupplierName = x.SupplierName,
					SupplierTaxNumber = x.SupplierTaxNumber,
					SupplierAddress = x.SupplierAddress,
					PayinCommission = Math.Ceiling((((Math.Abs(x.Amount) / 100) >= 0.02m) ? (Math.Abs(x.Amount) / 100) : 0.02m) * 100) / 100.0m,
					TotalWithoutPayin = x.Amount - Math.Ceiling((((Math.Abs(x.Amount) / 100) >= 0.02m) ? (Math.Abs(x.Amount) / 100) : 0.02m) * 100) / 100.0m,
					Date = x.Date,
					State = (x.RefundFromId != null && x.State == PaymentState.Active) ? PaymentState.Returned : x.State,
					RefundFromId = x.RefundFromId,
					RefundToId = x.RefundToId,
					RefundToDate = x.RefundToDate,
					RefundFromDate = x.RefundFromDate
				})
				.OrderBy(x => x.Date)
				.ToList();

			var securityRepository = new SecurityRepository();
			foreach (var payment in result)
			{
				var user = await securityRepository.GetUserAsync(payment.SupplierLogin);
				payment.fotoUrl = user.PhotoUrl;

				if (payment.fotoUrl != "")
					payment.FotoExists = true;
			}

			return new ResultBase<UnliquidatedPaymentResult> { Data = result };
		}
		#endregion ExecuteAsync
	}
}
