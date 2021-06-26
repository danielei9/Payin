using Microsoft.Practices.Unity;
using PayIn.Application.Dto.Payments.Arguments;
using PayIn.Application.Dto.Payments.Results;
using PayIn.BusinessLogic.Common;
using PayIn.Common;
using PayIn.Domain.Payments;
using PayIn.Domain.Payments.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xp.Application.Dto;
using Xp.Domain;

namespace PayIn.Application.Payments.Handlers
{
    public class MobileTicketGetOrdersHandler :
		IQueryBaseHandler<MobileTicketGetOrdersArguments, MobileTicketGetOrdersResult>
	{
		[Dependency] public ISessionData SessionData { get; set; }
		[Dependency] public IEntityRepository<Ticket> Repository { get; set; }
        [Dependency] public IEntityRepository<PaymentMedia> PaymentMediaRepository { get; set; }
        [Dependency] public IInternalService InternalService { get; set; }

		#region ExecuteAsync
		public async Task<ResultBase<MobileTicketGetOrdersResult>> ExecuteAsync(MobileTicketGetOrdersArguments arguments)
		{
			var result = await ExecuteInternalAsync(null);

			return new ResultBase<MobileTicketGetOrdersResult>
			{
				Data = result
			};
		}
		#endregion ExecuteAsync

		#region ExecuteInternalAsync
		public async Task<IEnumerable<MobileTicketGetOrdersResult>> ExecuteInternalAsync(int? paymentConcessionId)
		{
            var items = (await Repository.GetAsync())
                .Where(x =>
                    (
                        (x.Concession.Concession.Supplier.Login == SessionData.Login) ||
                        (x.Concession.PaymentWorkers.Any(y => y.Login == SessionData.Login))
                    ) &&
                    (
                        (x.Type == TicketType.Order) ||
                        (x.Type == TicketType.Shipment)
                    ) &&
                    (
                        (x.State == TicketStateType.Created) ||
                        (x.State == TicketStateType.Pending) || // Pagado
                        (x.State == TicketStateType.Prepared) || // Listo
                        (x.State == TicketStateType.Active) || // Entregado
                        (x.State == TicketStateType.Cancelled) // Cancelado
                    )
                );
            if (paymentConcessionId != null)
                items = items
                    .Where(x => x.ConcessionId == paymentConcessionId);
            
            var result = items
				.Select(x => new
				{
					Id = x.Id,
					Amount = x.Amount,
					Date = x.Date,
					State = x.State,
					SupplierName = x.SupplierName,
					SupplierAddress = x.TaxAddress,
					SupplierPhone = x.Concession.Phone,
					WorkerName = (x.PaymentWorker != null) ? x.PaymentWorker.Name : "",
					UserName = x.Payments
						.Select(y => y.UserName)
						.FirstOrDefault() ?? ""
				})
				.OrderByDescending(x => x.Date)
				.ToList()
				.Select(x => new MobileTicketGetOrdersResult
                {
					Id = x.Id,
					Amount = x.Amount,
					Date = x.Date.ToUTC(), // En memoria
					State = x.State,
					SupplierName = x.SupplierName,
					SupplierAddress = x.SupplierAddress,
					SupplierPhone = x.SupplierPhone,
					WorkerName = x.WorkerName,
					UserName = x.UserName
				});

			return result;
		}
		#endregion ExecuteInternalAsync
	}
}
