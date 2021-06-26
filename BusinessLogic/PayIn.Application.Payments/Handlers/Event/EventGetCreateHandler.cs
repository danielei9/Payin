using Microsoft.Practices.Unity;
using PayIn.Application.Dto.Payments.Arguments;
using PayIn.Application.Dto.Payments.Results;
using PayIn.BusinessLogic.Common;
using PayIn.Domain.Payments;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xp.Application.Dto;
using Xp.Domain;

namespace PayIn.Application.Payments.Handlers
{
    public class EventGetCreateHandler :
        IQueryBaseHandler<EventGetCreateArguments, EventGetCreateResult>
    {
        [Dependency] public IEntityRepository<PaymentConcession> PaymentConcessionRepository { get; set; }
        [Dependency] public IEntityRepository<EntranceSystem> EntranceSystemRepository { get; set; }
        [Dependency] public ISessionData SessionData { get; set; }

        #region ExecuteAsync
        public async Task<ResultBase<EventGetCreateResult>> ExecuteAsync(EventGetCreateArguments arguments)
        {
            // PaymentConcessions
            var items = (await PaymentConcessionRepository.GetAsync())
                .Where(x => x.Concession.State == Common.ConcessionState.Active);
            var paymentConcessions = (
                items
                    .Where(x => x.Concession.Supplier.Login == SessionData.Login)
                    .Select(x => new
					{
                        PaymentConcession = x,
                        Order = 1
					})
            ).Union(
                items
                    .Where(x => x.PaymentWorkers.Any(y => y.Login == SessionData.Login))
                    .Select(x => new
					{
						PaymentConcession = x,
                        Order = 0
					})
            )
			.Select(x => new EventGetCreateResultBase_PaymentConcessions
			{
				Id = x.PaymentConcession.Id,
				Value = x.PaymentConcession.Concession.Name,
				Order = x.Order,
				EntranceSystems = x.PaymentConcession.EntranceSystems
					.Select(y => new SelectorResult
					{
						Id = y.Id,
						Value = y.Name
					})
					.OrderBy(y => y.Value)
			})
			.OrderByDescending(x => x.Order)
            .ThenBy(x => x.Value)
            .ToList();

            // EntranceSystems
            var entranceSystems = (await EntranceSystemRepository.GetAsync())
                .Select(x => new SelectorResult
                {
                    Id = x.Id,
                    Value = x.Name
                })
                .OrderBy(x => x.Value)
                .ToList();

            return new EventGetCreateResultBase
            {
                PaymentConcessionId = paymentConcessions,
                EntranceSystemId = entranceSystems,
                Data = new List<EventGetCreateResult> {
                    new EventGetCreateResult {
                        EntranceSystemId = entranceSystems
                            .Select(x => (int?)x.Id)
                            .Min(),
                        PaymentConcessionId = paymentConcessions
                            .FirstOrDefault()
                            ?.Id
                    }
                }
            };
		}
		#endregion ExecuteAsync
	}
}
