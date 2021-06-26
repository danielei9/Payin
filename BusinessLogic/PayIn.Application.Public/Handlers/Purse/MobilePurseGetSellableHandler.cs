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
using Xp.Common;
using Xp.Domain;

namespace PayIn.Application.Payments.Handlers
{
    public class MobilePurseGetSellableHandler :
        IQueryBaseHandler<MobilePurseGetSellableArguments, MobilePurseGetSellableResult>
    {
        [Dependency] public IEntityRepository<Purse> Repository { get; set; }
        [Dependency] public IEntityRepository<PaymentConcession> PaymentConcessionRepository { get; set; }
        [Dependency] public IEntityRepository<ServiceConcession> ServiceConcessionRepository { get; set; }
        [Dependency] public ISessionData SessionData { get; set; }

        #region ExecuteAsync
        public async Task<ResultBase<MobilePurseGetSellableResult>> ExecuteAsync(MobilePurseGetSellableArguments arguments)
        {
			var now = new XpDate(DateTime.Now);

            var myConcessions = (await PaymentConcessionRepository.GetAsync())
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
                .Select(x => new
                {
                    x.Concession.Supplier.Login
                });

            var result = (await Repository.GetAsync())
                .Where(x =>
                    (x.SystemCard.Cards
                        .Any(y =>
                            (y.Id ==  arguments.ServiceCardId) &&
                            (
                                (y.State == ServiceCardState.Active) ||
                                (y.State == ServiceCardState.Emited)
                            )
                        )
                    ) &&
                    (myConcessions
                        .Any(y =>
                            // Soy de la empresa propietaria del SystemCard
                            (y.Login == SessionData.Login) ||
                            // Soy de una empresa miembro del SystemCard
                            (x.SystemCard.SystemCardMembers
                                .Any(z =>
                                    (z.Login == y.Login)
                                )
                            )
                        )
                    ) &&
                    (x.State == PurseState.Active) &&
                    (x.IsRechargable) &&
                    (x.IsPayin) &&
                    (x.Validity >= now)
                )
                .OrderByDescending(y => y.Slot)
                .Select(x => new MobilePurseGetSellableResult
                {
                    Id = x.Id,
                    Value = x.Name,
                    PaymentConcessionId = x.ConcessionId
                });

			return new ResultBase<MobilePurseGetSellableResult>
			{
                Data = result
            };
		}
		#endregion ExecuteAsync
	}
}
