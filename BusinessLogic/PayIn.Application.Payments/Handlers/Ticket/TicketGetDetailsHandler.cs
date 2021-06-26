using Microsoft.Practices.Unity;
using PayIn.Application.Dto.Payments.Arguments;
using PayIn.Application.Dto.Payments.Results;
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
	public class TicketGetDetailsHandler :
		IQueryBaseHandler<TicketGetDetailsArguments, TicketGetDetailsResult>
	{
        [Dependency] public IEntityRepository<Ticket> Repository { get; set; }

		#region ExecuteAsync
		public async Task<ResultBase<TicketGetDetailsResult>> ExecuteAsync(TicketGetDetailsArguments arguments)
		{
			var items = (await Repository.GetAsync())
				.Where(x => x.Id == arguments.Id);

			var result = items
				.Select(x => new
				{
                    x.Id,
                    x.Date,
                    x.Since,
                    x.Until,
                    x.State,
                    x.Reference,
                    //Title = x.Title,
                    x.Amount,
					PayedAmount = x.Payments
						.Where(y => y.State == PaymentState.Active)
						.Sum(y => (decimal?)y.Amount) ?? 0m,
					Total = x.Amount,
                    x.SupplierName,
					SupplierTaxAddress = x.TaxAddress,
					SupplierTaxNumber = x.TaxNumber,
					HasShipment = x.Shipment != null,
					SupplierLogin = x.Concession.Concession.Supplier.Login,
					WorkerName = (x.PaymentWorker != null)? x.PaymentWorker.Name : x.SupplierName ,
					Payments = x.Payments
						.Select(y => new
						{
                            y.Id,
                            y.Date,
                            y.State,
                            y.TaxName,
                            y.PaymentMedia.NumberHash,
                            y.Amount,
                            y.Payin,
                            y.RefundFromId,
							RefundFromDate = (y.RefundFrom != null) ? (DateTime?)y.RefundFrom.Date : null,
							RefundToId = (y.RefundTo.Count() != 0) ? (int?)y.RefundTo.FirstOrDefault().Id : null,
							RefundToDate = (y.RefundTo.Count() != 0) ? (DateTime?)y.RefundTo.FirstOrDefault().Date : null
						}),
					Lines = x.Lines
						.Select(y => new{
                            y.Id,
                            y.Title,
                            y.Amount,
                            y.Quantity
						})
				})
				.ToList()
				.Select(x => new TicketGetDetailsResult
				{
					Id = x.Id,
					Date = x.Date.ToUTC(), // Needs to be calculated in memory
					Since = x.Since,
					Until = x.Until,
					State = x.State,
					Reference = x.Reference,
					//Title = x.Title,
					Amount = x.Amount,
					Total = x.Total,
					SupplierName = x.SupplierName,
					SupplierTaxAddress = x.SupplierTaxAddress,
					SupplierTaxNumber = x.SupplierTaxNumber,
					SupplierLogin = x.SupplierLogin,
					PayedAmount = x.PayedAmount,
					WorkerName = x.WorkerName,
					HasShipment = x.HasShipment,
					Payments = x.Payments
						.Select(y => new TicketGetDetailsResult_Payment
						{
							Id = y.Id,
							Date = y.Date.ToUTC(), // Needs to be calculated in memory
							State = y.State,
							StateAlias = y.State.ToEnumAlias(),
							TaxName = y.TaxName,
							NumberHash = y.NumberHash,
							Amount = y.Amount,
							Payin = y.Payin,
							Total = y.Amount - y.Payin,
							RefundFromId = y.RefundFromId,
							RefundFromDate = y.RefundFromDate,
							RefundToId = y.RefundToId,
							RefundToDate = y.RefundToDate
						}),
					Lines = x.Lines
					    .Select( y => new TicketGetDetailsResult_TicketLine
						{
							Id = y.Id,
							Title = y.Title,
							Amount = y.Amount,
							Quantity = y.Quantity
						})
				});

			//var pepe = result.ToList();

			var securityRepository = new SecurityRepository();
			foreach (var ticket in result)
			{
				var user = await securityRepository.GetUserAsync(ticket.SupplierLogin);
				if (user != null)
					ticket.SupplierFotoUrl = user.PhotoUrl;
			}
			return new ResultBase<TicketGetDetailsResult> { Data = result };
		}
		#endregion ExecuteAsync
	}
 }
