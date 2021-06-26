using Microsoft.Practices.Unity;
using PayIn.Application.Dto.Arguments;
using PayIn.BusinessLogic.Common;
using PayIn.Common;
using PayIn.Domain.Payments;
using PayIn.Domain.Public;
using System;
using System.Linq;
using System.Threading.Tasks;
using Xp.Application.Dto;
using Xp.Domain;

namespace PayIn.Application.Public.Handlers
{
	public class ServiceCardGetSelectorHandler :
		IQueryBaseHandler<ServiceCardGetSelectorArguments, SelectorResult>
    {
        [Dependency] public ISessionData SessionData { get; set; }
        [Dependency] public IEntityRepository<ServiceCard> Repository { get; set; }
        [Dependency] public IEntityRepository<PaymentConcession> PaymentConcessionRepository { get; set; }

        private string hexArray = "0123456789ABCDEF";

		#region ExecuteAsync
		public async Task<ResultBase<SelectorResult>> ExecuteAsync(ServiceCardGetSelectorArguments arguments)
		{
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

            var items = (await Repository.GetAsync())
                .Where(x =>
                    (
                        (x.State == ServiceCardState.Active) ||
                        (x.State == ServiceCardState.Emited)
                    ) &&
                    (
                        (myConcessions
                            .Any(c =>
                                (x.SystemCard.ConcessionOwner.Supplier.Login == c.Login)
                            )
                        ) ||
                        (x.SystemCard.SystemCardMembers
                            .Any(y => myConcessions
                                .Any(c =>
                                    (y.Login == c.Login)
                                )
                            )
                        )
                    )
                )
                .Select(x => new
				{
					x.Id,
                    Uid = x.Uid.ToString(),
                    UidText =
						x.ServiceCardBatch.UidFormat == UidFormat.BigEndian ?
							(
								hexArray.Substring((int)((x.Uid / 268435456) % 16), 1) +
								hexArray.Substring((int)((x.Uid / 16777216) % 16), 1) +
								hexArray.Substring((int)((x.Uid / 1048576) % 16), 1) +
								hexArray.Substring((int)((x.Uid / 65536) % 16), 1) +
								hexArray.Substring((int)((x.Uid / 4096) % 16), 1) +
								hexArray.Substring((int)((x.Uid / 256) % 16), 1) +
								hexArray.Substring((int)((x.Uid / 16) % 16), 1) +
								hexArray.Substring((int)((x.Uid / 1) % 16), 1)
							).ToString() :
						x.ServiceCardBatch.UidFormat == UidFormat.LittleEndian ?
							(
								hexArray.Substring((int)((x.Uid / 16) % 16), 1) +
								hexArray.Substring((int)((x.Uid / 1) % 16), 1) +
								hexArray.Substring((int)((x.Uid / 4096) % 16), 1) +
								hexArray.Substring((int)((x.Uid / 256) % 16), 1) +
								hexArray.Substring((int)((x.Uid / 1048576) % 16), 1) +
								hexArray.Substring((int)((x.Uid / 65536) % 16), 1) +
								hexArray.Substring((int)((x.Uid / 268435456) % 16), 1) +
								hexArray.Substring((int)((x.Uid / 16777216) % 16), 1)
							).ToString() :
						x.Uid.ToString()
				})
                .Select(x => new SelectorResult
                {
                    Id = x.Id,
                    Value = (x.Uid == x.UidText) ?
                        x.UidText :
                        (x.Uid + " (" + x.UidText + ")")
                });

			if (!arguments.Filter.IsNullOrEmpty())
				items = items.Where(x =>
					x.Value.Contains(arguments.Filter)
				);

			var result = items
				.Select(x => new SelectorResult
                {
					Id = x.Id,
					Value = x.Value
				});

			return new ResultBase<SelectorResult> { Data = result };
		}
		#endregion ExecuteAsync
	}
}
