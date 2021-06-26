using Microsoft.Practices.Unity;
using PayIn.Application.Dto.Payments.Arguments;
using PayIn.Application.Dto.Payments.Results;
using PayIn.BusinessLogic.Common;
using PayIn.Common;
using PayIn.Domain.Payments;
using PayIn.Domain.Public;
using System;
using System.Linq;
using System.Threading.Tasks;
using Xp.Application.Dto;
using Xp.Domain;

namespace PayIn.Application.Payments.Handlers
{
	public class PaymentGetAllByLiquidationHandler :
		IQueryBaseHandler<PaymentGetAllByLiquidationArguments, PaymentsGetAllByLiquidationResult>
    {
        [Dependency] public ISessionData SessionData { get; set; }
        [Dependency] public IEntityRepository<Payment> Repository { get; set; }
        [Dependency] public IEntityRepository<Liquidation> LiquidationRepository { get; set; }
        [Dependency] public IEntityRepository<SystemCardMember> SystemCardMemberRepository { get; set; }

        private string hexArray = "0123456789ABCDEF";

        #region ExecuteAsync
        public async Task<ResultBase<PaymentsGetAllByLiquidationResult>> ExecuteAsync(PaymentGetAllByLiquidationArguments arguments)
		{
            var systemCardMembers = (await SystemCardMemberRepository.GetAsync());

            var items = (await Repository.GetAsync("Ticket"))
				.Where(x =>
                    x.Ticket.ConcessionId == arguments.ConcessionId &&
                    x.LiquidationId == arguments.LiquidationId &&
					x.State == PaymentState.Active  &&
                    (
                        // Liquiaciones mias
                        (x.Ticket.Concession.Concession.Supplier.Login == SessionData.Login) ||
                        // Liquidaciones de las empresas de mi sistema de tarjetas
                        (
                            systemCardMembers
                                .Any(y =>
                                    y.Login == x.Ticket.Concession.Concession.Supplier.Login &&
                                    y.SystemCard.ConcessionOwner.Supplier.Login == SessionData.Login
                                )
                        )
                    )
                );	
			if (!arguments.Filter.IsNullOrEmpty())
				items = items
					.Where(x => (
						x.TaxName.Contains(arguments.Filter)
					));

			var result = items
				.Select(x => new
				{
					Id = x.Id,
					Amount = x.Amount,
					Commission = x.Payin,
                    Uid = x.Uid,
                    UidText =
                        x.UidFormat == UidFormat.BigEndian ?
                            (
                                hexArray.Substring(((int)x.Uid / 268435456) % 16, 1) +
                                hexArray.Substring(((int)x.Uid / 16777216) % 16, 1) +
                                hexArray.Substring(((int)x.Uid / 1048576) % 16, 1) +
                                hexArray.Substring(((int)x.Uid / 65536) % 16, 1) +
                                hexArray.Substring(((int)x.Uid / 4096) % 16, 1) +
                                hexArray.Substring(((int)x.Uid / 256) % 16, 1) +
                                hexArray.Substring(((int)x.Uid / 16) % 16, 1) +
                                hexArray.Substring(((int)x.Uid / 1) % 16, 1)
                            ).ToString() :
                        x.UidFormat == UidFormat.LittleEndian ?
                            (
                                hexArray.Substring(((int)x.Uid / 16) % 16, 1) +
                                hexArray.Substring(((int)x.Uid / 1) % 16, 1) +
                                hexArray.Substring(((int)x.Uid / 4096) % 16, 1) +
                                hexArray.Substring(((int)x.Uid / 256) % 16, 1) +
                                hexArray.Substring(((int)x.Uid / 1048576) % 16, 1) +
                                hexArray.Substring(((int)x.Uid / 65536) % 16, 1) +
                                hexArray.Substring(((int)x.Uid / 268435456) % 16, 1) +
                                hexArray.Substring(((int)x.Uid / 16777216) % 16, 1)
                            ).ToString() :
                        x.Uid.ToString(),
                    Seq = x.Seq,
                    TaxName = x.TaxName,
					TicketId = x.TicketId,
					Date = x.Date,
					State = (x.RefundFromId != null && x.State == PaymentState.Active) ? PaymentState.Returned : x.State,
					EventName = x.Ticket.Event.Name,
                    TicketConcessionName = x.Ticket.Concession.Concession.Name,
                    Title = x.Ticket.Lines.FirstOrDefault().Title
                })
				.OrderBy(x => x.Date)
				.ToList()
				.Select(x => new PaymentsGetAllByLiquidationResult
				{
					Id = x.Id,
					TaxName = x.TaxName,
					Amount = x.Amount,
					Commission = x.Commission,
                    UidText = x.UidText,
                    Seq = x.Seq,
                    Uid = x.Uid,
                    TicketId = x.TicketId,
					Date = x.Date.ToUTC(),
					State = x.State,
					EventName = x.EventName,
                    TicketConcessionName = x.TicketConcessionName,
                    Title = x.Title
                });

			var liquidation = (await LiquidationRepository.GetAsync())
				.Where(x => x.Id == arguments.LiquidationId)
				.Select(x => new
				{
					x.Since,
					x.Until,
					x.TotalQuantity,
					x.PayinQuantity,
				})
				.FirstOrDefault();

			return new PaymentsGetAllByLiquidationResultBase
			{
				LiquidationSince = liquidation?.Since.ToUTC(),
				LiquidationUntil = liquidation?.Until.ToUTC(),
				LiquidationAmount = liquidation?.TotalQuantity ?? 0,
				LiquidationCommission = liquidation?.PayinQuantity ?? 0,
				TotalAmount = Math.Round(result.Sum(x => (decimal?)x.Amount) ?? 0, 2),
				TotalCommission = Math.Round(result.Sum(x => (decimal?)x.Commission) ?? 0, 2),
				Data = result
			};
		}
		#endregion ExecuteAsync
	}
}

