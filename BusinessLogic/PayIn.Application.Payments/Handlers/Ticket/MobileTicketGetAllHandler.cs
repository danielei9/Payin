using Microsoft.Practices.Unity;
using PayIn.Application.Dto.Payments.Arguments;
using PayIn.Application.Dto.Payments.Results;
using PayIn.BusinessLogic.Common;
using PayIn.Common;
using PayIn.Domain.Payments;
using PayIn.Domain.Payments.Infrastructure;
using PayIn.Domain.Security;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xp.Application.Dto;
using Xp.Domain;

namespace PayIn.Application.Payments.Handlers
{
	public class MobileTicketGetAllHandler :
		IQueryBaseHandler<MobileTicketGetAllArguments, MobileTicketGetAllResult>
	{
		[Dependency] public ISessionData SessionData { get; set; }
		[Dependency] public IEntityRepository<Ticket> Repository { get; set; }
        [Dependency] public IEntityRepository<PaymentMedia> PaymentMediaRepository { get; set; }
        [Dependency] public IInternalService InternalService { get; set; }

		#region ExecuteAsync
		public async Task<ResultBase<MobileTicketGetAllResult>> ExecuteAsync(MobileTicketGetAllArguments arguments)
		{
			var result = await ExecuteInternalAsync(arguments.PaymentConcessionId);

			return new ResultBase<MobileTicketGetAllResult>
			{
				Data = result
			};
		}
		#endregion ExecuteAsync

		#region ExecuteInternalAsync
		public async Task<IEnumerable<MobileTicketGetAllResult>> ExecuteInternalAsync(int paymentConcessionId)
		{
            var items = (await Repository.GetAsync())
                .Where(x =>
                    (
                        (
                            (x.PaymentUser.Login == SessionData.Login) ||
                            (x.Payments.Any(y => y.UserLogin == SessionData.Login))
                        ) &&
                        (x.ConcessionId == paymentConcessionId)
                    ) &&
					(
						(x.Type == TicketType.Ticket) ||
						(x.Type == TicketType.Order) //||
                        //(x.Type == TicketType.Shipment)
                    ) &&
                    (
                        (x.State == TicketStateType.Created) ||
                        (x.State == TicketStateType.Pending) || // Pagado
                        (x.State == TicketStateType.Prepared) || // Listo
                        (x.State == TicketStateType.Active) || // Entregado
                        (x.State == TicketStateType.Cancelled) // Cancelado
                    )
                )
				.Select(x => new
				{
					x.Id,
					x.Amount,
					x.Date,
					x.State,
					x.Type,
					SupplierName = x.Concession.Concession.Name,
					WorkerName = x.PaymentWorker.Name ?? "",
					PayedAmount = x.Payments
						.Where(y => y.State == PaymentState.Active)
						.Sum(y => (decimal?)y.Amount) ?? 0
				})
				.OrderByDescending(x => x.Date)
				.ToList()
				.Select(x => new MobileTicketGetAllResult
                {
					Id = x.Id,
					Amount = x.Amount,
					Date = x.Date.ToUTC(), // En memoria
					State = x.State,
					Type = x.Type,
					SupplierName = x.SupplierName,
					WorkerName = x.WorkerName,
					PayedAmount = x.PayedAmount
				});

			return items;
		}
		#endregion ExecuteInternalAsync
	}
}
