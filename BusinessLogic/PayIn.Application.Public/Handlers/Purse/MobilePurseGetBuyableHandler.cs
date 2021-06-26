using Microsoft.Practices.Unity;
using PayIn.Application.Dto.Payments.Arguments;
using PayIn.Application.Dto.Payments.Results;
using PayIn.BusinessLogic.Common;
using PayIn.Common;
using PayIn.Domain.Payments;
using System;
using System.Linq;
using System.Threading.Tasks;
using Xp.Application.Dto;
using Xp.Common;
using Xp.Domain;

namespace PayIn.Application.Payments.Handlers
{
    public class MobilePurseGetBuyableHandler :
        IQueryBaseHandler<MobilePurseGetBuyableArguments, MobilePurseGetBuyableResult>
    {
        [Dependency] public IEntityRepository<Purse> Repository { get; set; }
        [Dependency] public IEntityRepository<PaymentConcession> PaymentConcessionRepository { get; set; }
        [Dependency] public ISessionData SessionData { get; set; }

        #region ExecuteAsync
        public async Task<ResultBase<MobilePurseGetBuyableResult>> ExecuteAsync(MobilePurseGetBuyableArguments arguments)
        {
			var now = new XpDate(DateTime.Now);

            var payinPaymentConcessions = (await PaymentConcessionRepository.GetAsync())
                .Where(x =>
                    (x.Concession.State == ConcessionState.Active) &&
                    (x.Concession.Supplier.Login == "info@pay-in.es")
                );

            var result = (await Repository.GetAsync())
                .Where(x =>
                    (x.SystemCard.Cards
                        .Any(y =>
                            (y.Id ==  arguments.ServiceCardId) &&
                            (
                                (y.OwnerLogin == SessionData.Login) ||
                                (y.OwnerUser.Login == SessionData.Login)
                            ) &&
                            (
                                (y.State == ServiceCardState.Active) ||
                                (y.State == ServiceCardState.Emited)
                            )
                        )
                    ) &&
                    (x.State == PurseState.Active) &&
                    (x.IsRechargable) &&
                    (x.IsPayin) &&
                    (x.Validity >= now)
                )
                .OrderByDescending(y => y.Slot)
                .Select(x => new MobilePurseGetBuyableResult
                {
                    Id = x.Id,
                    Value = x.Name,
                    PaymentConcessionId = payinPaymentConcessions.FirstOrDefault().Id
                });

			return new ResultBase<MobilePurseGetBuyableResult>
			{
                Data = result
            };
		}
		#endregion ExecuteAsync
	}
}
