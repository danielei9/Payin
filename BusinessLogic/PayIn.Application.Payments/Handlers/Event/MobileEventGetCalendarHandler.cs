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
	public class MobileEventGetCalendarHandler : IQueryBaseHandler<MobileEventGetCalendarArguments, MobileEventGetCalendarResult>
	{
		[Dependency] public IEntityRepository<Event> Repository { get; set; }
		[Dependency] public IEntityRepository<EntranceType> EntranceTypeRepository { get; set; }
        [Dependency] public IEntityRepository<PaymentConcession> PaymentConcessionRepository { get; set; }
        [Dependency] public ISessionData SessionData { get; set; }

		#region ExecuteAsync
		public async Task<ResultBase<MobileEventGetCalendarResult>> ExecuteAsync(MobileEventGetCalendarArguments arguments)
        {
            var now = DateTime.UtcNow;
            var start = new DateTime(now.Year, now.Month, 1)
                .AddMonths(-6);
            var end = new DateTime(now.Year, now.Month, 1)
                .AddMonths(6+1)
                .AddDays(-1);

            var events = (await Repository.GetAsync());
			var result = events
                .Where(x =>
                    ((arguments.PaymentConcessionId == null) || (x.PaymentConcessionId == arguments.PaymentConcessionId)) &&
                    (x.State == EventState.Active) &&
                    (x.IsVisible) &&
                    (x.EventEnd >= start) &&
                    (x.EventStart <= end)
                )
				.Select(x => new
				{
					x.Id,
					x.Place,
					x.Name,
					x.EventStart,
					x.EventEnd,
					x.State
				})
				.ToList()
				.Select(x => new MobileEventGetCalendarResult
                {
					Id = x.Id,
					Place = x.Place,
					Name = x.Name,
					EventStart = (x.EventStart == XpDateTime.MinValue) ? (DateTime?)null : x.EventStart.ToUTC(),
					EventEnd = (x.EventEnd == XpDateTime.MaxValue) ? (DateTime?)null : x.EventEnd.ToUTC(),
					State = x.State,
                    Foo = x.Name,
                    Date = (x.EventStart == XpDateTime.MinValue) ?
                        "" :
                        x.EventStart.ToUTC().ToString("yyyy-MM-dd")
                });

			return new ResultBase<MobileEventGetCalendarResult>
			{
				Data = result
			};
		}
		#endregion ExecuteAsync
	}
}
