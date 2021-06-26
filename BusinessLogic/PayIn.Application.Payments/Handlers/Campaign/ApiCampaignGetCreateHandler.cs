using Microsoft.Practices.Unity;
using PayIn.Application.Dto.Payments.Arguments;
using PayIn.Application.Dto.Payments.Results;
using PayIn.BusinessLogic.Common;
using PayIn.Domain.Payments;
using PayIn.Domain.Public;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xp.Application.Dto;
using Xp.Domain;

namespace PayIn.Application.Payments.Handlers
{
    public class ApiCampaignGetCreateHandler : IQueryBaseHandler<ApiCampaignGetCreateArguments, ApiCampaignGetCreateResult>
    {
        [Dependency] public IEntityRepository<PaymentConcession> PaymentConcessionRepository { get; set; }
        [Dependency] public IEntityRepository<SystemCard> SystemCardRepository { get; set; }
        [Dependency] public ISessionData SessionData { get; set; }

        #region ExecuteAsync
        public async Task<ResultBase<ApiCampaignGetCreateResult>> ExecuteAsync(ApiCampaignGetCreateArguments arguments)
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
			.Select(x => new ApiCampaignGetCreateResultBase_PaymentConcessions
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
			var targetSystemCardId = (await SystemCardRepository.GetAsync())
				.Where(x =>
					x.ConcessionOwner.Supplier.Login == SessionData.Login ||
					x.SystemCardMembers
						.Any(y => y.Login == SessionData.Login)
				)
				.Select(x => new SelectorResult
				{
					Id = x.Id,
					Value = x.Name
				})
				.ToList();

			return new ApiCampaignGetCreateResultBase
			{
                PaymentConcessionId = paymentConcessions,
				TargetSystemCardId = targetSystemCardId,
				Data = new List<ApiCampaignGetCreateResult> {
                    new ApiCampaignGetCreateResult {
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
