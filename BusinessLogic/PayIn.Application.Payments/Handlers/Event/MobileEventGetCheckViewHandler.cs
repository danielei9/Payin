using Microsoft.Practices.Unity;
using PayIn.Application.Dto.Payments.Arguments;
using PayIn.Application.Dto.Payments.Results;
using PayIn.BusinessLogic.Common;
using PayIn.Common;
using PayIn.Domain.Payments;
using System;
using System.Linq;
using System.Threading.Tasks;
using Xp.Application.Attributes;
using Xp.Application.Dto;
using Xp.Domain;

namespace PayIn.Application.Payments.Handlers
{
	public class MobileEventGetCheckViewHandler :
		IQueryBaseHandler<MobileEventGetCheckViewArguments, MobileEventGetCheckViewResult>
	{		
		[Dependency] public IEntityRepository<Event> Repository { get; set; }
        [Dependency] public ISessionData SessionData { get; set; }

        #region ExecuteAsync
        public async Task<ResultBase<MobileEventGetCheckViewResult>> ExecuteAsync(MobileEventGetCheckViewArguments arguments)
		{
			var now = DateTime.UtcNow;

			var result = (await Repository.GetAsync())
				.Where(x =>
					(x.Id == arguments.Id) &&
					(x.State == EventState.Active) &&
                    (x.PaymentConcession.Concession.State == ConcessionState.Active) &&
                    (x.CheckInStart <= now) &&
                    (x.CheckInEnd >= now) &&
                    (
                        (x.PaymentConcession.Concession.Supplier.Login == SessionData.Login) ||
                        (x.PaymentConcession.PaymentWorkers.Any(y => y.Login == SessionData.Login))
                    )
                )
				.Select(x => new MobileEventGetCheckViewResult
                {
					Id = x.Id,
					Name = x.Name,
					PhotoUrl = x.PhotoUrl,
					Capacity = x.Capacity,
                    Count = x.EntranceTypes
                        .Sum(y => y.Entrances
                            .Count(z => z.Checks
                                .Any(a =>
                                    (a.Type == CheckInType.In) &&
                                    (!z.Checks
                                        .Any(b =>
                                            (b.Type == CheckInType.Out) &&
                                            (b.TimeStamp > a.TimeStamp)
                                        )
                                    )
                                )
                            )
                        )
                })
#if DEBUG
				.ToList()
#endif // DEBUG
				;
			return new ResultBase<MobileEventGetCheckViewResult> { Data = result };
		}
		#endregion ExecuteAsync
	}
}
