using Microsoft.Practices.Unity;
using PayIn.Application.Dto.Payments.Arguments;
using PayIn.Application.Dto.Payments.Results;
using PayIn.BusinessLogic.Common;
using PayIn.Domain.Public;
using System.Linq;
using System;
using System.Threading.Tasks;
using Xp.Application.Dto;
using Xp.Domain;
using PayIn.Domain.Security;
using PayIn.Domain.Payments;
using PayIn.Common;

namespace PayIn.Application.Public.Handlers
{
	public class LiquidationGetSelectorOpenedHandler :
		IQueryBaseHandler<LiquidationGetSelectorOpenedArguments, SelectorResult>
    {
        [Dependency] public IEntityRepository<PaymentConcession> PaymentConcessionRepository { get; set; }
        [Dependency] public IEntityRepository<Liquidation> Repository { get; set; }
        [Dependency] public ISessionData SessionData { get; set; }

		#region ExecuteAsync
		public async Task<ResultBase<SelectorResult>> ExecuteAsync(LiquidationGetSelectorOpenedArguments arguments)
        {
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
                );

#if DEBUG
            var myPaymentConcessionsResult = myPaymentConcessions.ToList();
#endif // DEBUG

            var result = (await Repository.GetAsync())
                .Where(x =>
                    ((arguments.PaymentConcessionId == null) || (arguments.PaymentConcessionId == x.ConcessionId)) &&
                    (x.Concession.Concession.Name.Contains(arguments.Filter)) &&
                    (x.State == LiquidationState.Opened) &&
                    (myPaymentConcessions.Any(y => y.Id == x.LiquidationConcessionId))
				)
                .Select(x => new
                {
                    x.Id,
                    x.RequestDate,
                    ConcessionName = x.Concession.Concession.Name
                })
				.ToList()
				.Select(x => new SelectorResult
				{
					Id = x.Id,
					Value = x.ConcessionName + " (" + x.RequestDate.ToIsoString() + ")"
				})
#if DEBUG
                .ToList()
#endif // DEBUG
                ;

            return new ResultBase<SelectorResult> { Data = result };
		}
		#endregion ExecuteAsync
	}
}
