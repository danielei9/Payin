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
	public class ApiNoticeGetEdictsHandler :
		IQueryBaseHandler<ApiNoticeGetEdictsArguments, ApiNoticeGetEdictsResult>
	{
		[Dependency] public IEntityRepository<Notice> Repository { get; set; }
        [Dependency] public ISessionData SessionData { get; set; }

		#region ExecuteAsync
		public async Task<ResultBase<ApiNoticeGetEdictsResult>> ExecuteAsync(ApiNoticeGetEdictsArguments arguments)
		{
			var items = (await Repository.GetAsync())
				.Where(x =>
					(
						(x.PaymentConcession.Concession.Supplier.Login == SessionData.Login) ||
						(x.PaymentConcession.Concession.Supplier.Workers.Select(y => y.Login).Contains(SessionData.Login))
					) &&
					(x.State != NoticeState.Deleted) &&
                    (x.Type == NoticeType.Edict)
				);

			if (!arguments.Filter.IsNullOrEmpty())
				items = items.Where(x =>
					x.Name.Contains(arguments.Filter) ||
					x.Event.Name.Contains(arguments.Filter)
				);

			var result = items
                .Select(x => new
				{
					x.Id,
                    x.Name,
                    x.IsVisible,
					x.Visibility,
					x.State,
                    x.Start,
                    ConcessionName = x.PaymentConcession.Concession.Name ?? ""
                })
                .OrderByDescending(x => x.Start)
                .ToList()
				.Select(x => new ApiNoticeGetEdictsResult
				{
					Id = x.Id,
                    Name = x.Name,
					IsVisible = x.IsVisible,
					Visibility = x.Visibility,
					State = x.State,
                    Start = (x.Start == XpDateTime.MinValue) ? (DateTime?)null : x.Start.ToUTC(),
                    ConcessionName = x.ConcessionName
				});
			
			return new ResultBase<ApiNoticeGetEdictsResult> { Data = result };
		}
		#endregion ExecuteAsync
	}
}
