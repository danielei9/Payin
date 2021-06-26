using Microsoft.Practices.Unity;
using PayIn.Application.Dto.Payments.Arguments;
using PayIn.Application.Dto.Payments.Results;
using PayIn.BusinessLogic.Common;
using PayIn.Common;
using PayIn.Domain.Payments;
using PayIn.Domain.Public;
using System;
using System.Data.Entity.SqlServer;
using System.Linq;
using System.Threading.Tasks;
using Xp.Application.Dto;
using Xp.Domain;

namespace PayIn.Application.Payments
{
	public class LiquidationGetAllHandler :
		IQueryBaseHandler<LiquidationGetAllArguments, LiquidationGetAllResult>
	{
		[Dependency] public ISessionData SessionData { get; set; }
        [Dependency] public IEntityRepository<Liquidation> Repository { get; set; }
        [Dependency] public IEntityRepository<PaymentConcession> PaymentConcessionRepository { get; set; }

		#region ExecuteAsync
		public async Task<ResultBase<LiquidationGetAllResult>> ExecuteAsync(LiquidationGetAllArguments arguments)
        {
            var now = DateTime.UtcNow;
            var since = arguments.Since;
            var until = arguments.Until;
            
            var paymentConcessions = (await PaymentConcessionRepository.GetAsync());
            var myPaymentConcessions = (await PaymentConcessionRepository.GetAsync())
                .Where(x =>
                    (x.Concession.State == ConcessionState.Active) &&
                    (
                        (x.Concession.Supplier.Login == SessionData.Login) ||
                        (x.PaymentWorkers
                            .Any(y =>
                                (y.State == WorkerState.Active) &&
                                (y.Login == SessionData.Login)
                            )
                        )
                    )
                )
                .Select(x => x.Id)
                .ToList();

            var liquidations = (await Repository.GetAsync())
                .Where(x =>
                    (x.Until >= since) &&
                    (x.Since <= until) &&
                    (myPaymentConcessions
                        .Contains(x.LiquidationConcessionId)
                    )
                )
                .Select(x => new
                {
                    x.Id,
                    x.State,
                    Amount = x.AccountLines
                        .Where(y => y.Type != AccountLineType.Liquidation)
                        .Sum(y => (decimal?)y.Amount) ?? 0,
                    Paid = -1 * x.AccountLines
                        .Where(y => y.Type == AccountLineType.Liquidation)
                        .Sum(y => (decimal?)y.Amount) ?? 0,
                    x.Since,
                    x.Until,
                    x.PaymentDate,
                    PaymentsCount = x.Payments
                        .Count(),
                    LinesCount = x.AccountLines
                        .Count(),
                    x.ConcessionId,
                    ConcessionName = x.Concession.Concession.Name
                })
                .OrderByDescending(x => x.Until)
                .ToList()
                .Select(x => new LiquidationGetAllResult
                {
                    Id = x.Id,
                    State = x.State,
                    Amount = x.Amount,
                    Since = x.Since.ToUTC(),
                    Until = x.Until.ToUTC(),
                    PaymentDate = x.PaymentDate?.ToUTC(),
                    PaymentsCount = x.PaymentsCount,
                    LinesCount = x.LinesCount,
                    ConcessionId = x.ConcessionId,
                    ConcessionName = x.ConcessionName
                })
#if DEBUG
                .ToList()
#endif // DEBUG
                ;

            //        var liquidationsAbiertas = (await PaymentConcessionRepository.GetAsync())
            //            .Where(x =>
            //                (
            //                    // Liquiaciones mias
            //                    (x.Concession.Supplier.Login == SessionData.Login) ||
            //                    // Liquidaciones de las empresas de mi sistema de tarjetas
            //                    (
            //                        systemCardMembers
            //                            .Any(y =>
            //                                y.Login == x.Concession.Supplier.Login &&
            //                                y.SystemCard.ConcessionOwner.Supplier.Login == SessionData.Login
            //                            )
            //                    )
            //                )
            //            )
            //            .SelectMany(x =>
            //                x.Tickets
            //                    .SelectMany(y =>
            //                        y.Payments
            //                            .Where(z =>
            //                                (z.State == PaymentState.Active) &&
            //                                (z.LiquidationId == null) &&
            //                                (now >= arguments.Since) &&
            //                                (z.Date <= until)
            //                            )
            //                            .Select(z => new
            //                            {
            //                                x.Id,
            //                                ConcessionName = x.Concession.Name,
            //                                z.Amount,
            //                                z.Payin,
            //                                z.Date
            //                            })
            //                    )
            //            )
            //            .GroupBy(x => new { x.Id, x.ConcessionName })
            //            .Select(x => new
            //            {
            //                Amount = x.Sum(y => (decimal?)y.Amount) ?? 0,
            //                Payin = x.Sum(y => (decimal?)y.Payin) ?? 0,
            //                Since = x.Min(y => (DateTime?)y.Date),
            //                Until = SqlFunctions.GetDate(),
            //                PaymentsCount = x.Count(),
            //                ConcessionId = x.Key.Id,
            //                x.Key.ConcessionName
            //})
            //            .OrderByDescending(x => x.Until)
            //            .ToList()
            //            .Select(x => new LiquidationGetAllResult
            //            {
            //                //Id = x.Id,
            //                State = LiquidationState.Opened, // x.State,
            //                Amount = x.Amount,
            //                Commission = x.Payin,
            //                Total = x.Amount - x.Payin,
            //                Since = x.Since,
            //                Until = x.Until,
            //                PaymentDate = null, //x.PaymentDate,
            //                RequestDate = null, // x.RequestDate,
            //                PaymentsCount = x.PaymentsCount,
            //                ConcessionId = x.ConcessionId,
            //                ConcessionName = x.ConcessionName
            //})
            //            .ToList();

            //        var liquidations = liquidationsAbiertas
            //            .Union(liquidationsCerradas)
            //            .OrderBy(x => x.State);

            return new LiquidationGetAllResultBase
			{
				Data = liquidations
            };
		}
		#endregion ExecuteAsync
	}
}
