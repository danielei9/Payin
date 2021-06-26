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
using Xp.Domain;

namespace PayIn.Application.Payments.Handlers
{
	public class MobileEventGetAllCheckViewHandler :
		IQueryBaseHandler<MobileEventGetAllCheckViewArguments, MobileEventGetAllCheckViewResult>
	{
		[Dependency] public IEntityRepository<Event> Repository { get; set; }
		[Dependency] public ISessionData SessionData { get; set; }

		#region ExecuteAsync
		public async Task<ResultBase<MobileEventGetAllCheckViewResult>> ExecuteAsync(MobileEventGetAllCheckViewArguments arguments)
		{
			var now = DateTime.UtcNow;
			var items = (await Repository.GetAsync())
				.Where(x =>
					(x.IsVisible) &&
                    (x.State == EventState.Active) &&
                    (x.PaymentConcession.Concession.State == ConcessionState.Active) &&
                    (x.CheckInStart <= now) &&
                    (x.CheckInEnd >= now) &&
                    (
						(x.PaymentConcession.Concession.Supplier.Login == SessionData.Login) ||
						(x.PaymentConcession.PaymentWorkers.Any(y => y.Login == SessionData.Login))
					)
				);
            if (!arguments.Filter.IsNullOrEmpty())
                items = items
                    .Where(x =>
                        x.Name.Contains(arguments.Filter) ||
                        x.Place.Contains(arguments.Filter)
                    );

			var result = items
				.OrderBy(y => y.EventStart)
				.Select(y => new
				{
					Id = y.Id,
					Name = y.Name,
					Place = y.Place,
					PhotoUrl = y.PhotoUrl,
					EventStart = y.EventStart,
					EventEnd = y.EventEnd,
					MinPrice = y.EntranceTypes
						.Select(z => (decimal?) z.Price)
						.Min() ?? 0,
					MaxPrice = y.EntranceTypes
						.Select(z => (decimal?)z.Price)
						.Max() ?? 0
				})
				.ToList()
				.Select(y => new MobileEventGetAllCheckViewResult
				{
					Id = y.Id,
					Name = y.Name,
					Place = y.Place,
					PhotoUrl = y.PhotoUrl,
					EventStart = y.EventStart.ToUTC(),
					EventEnd = y.EventEnd.ToUTC(),
					MinPrice = y.MinPrice,
					MaxPrice = y.MaxPrice
				})
#if DEBUG
				.ToList()
#endif // DEBUG
				;
			return new ResultBase<MobileEventGetAllCheckViewResult> { Data = result };
		}
		#endregion ExecuteAsync
	}
}
