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
	class ApiNoticeGetAllHandler :
		IQueryBaseHandler<ApiNoticeGetAllArguments, ApiNoticeGetAllResult>
	{
		private readonly IEntityRepository<Notice> Repository;
		private readonly ISessionData SessionData;

		#region Constructors
		public ApiNoticeGetAllHandler(
			IEntityRepository<Notice> repository,
			ISessionData sessionData
			)
		{
			if (repository == null) throw new ArgumentNullException("repository");
			if (sessionData == null) throw new ArgumentNullException("sessionData");

			Repository = repository;
			SessionData = sessionData;
		}
		#endregion Constructors

		#region ExecuteAsync
		public async Task<ResultBase<ApiNoticeGetAllResult>> ExecuteAsync(ApiNoticeGetAllArguments arguments)
		{
			var items = (await Repository.GetAsync())
				.Where(x =>
					(
						(x.PaymentConcession.Concession.Supplier.Login == SessionData.Login) ||
						(x.PaymentConcession.Concession.Supplier.Workers.Select(y => y.Login).Contains(SessionData.Login))
					) &&
					(x.State != NoticeState.Deleted) &&
                    (x.Type == NoticeType.Notice) &&
                    ((!(arguments.EventId > 0)) || (x.EventId == arguments.EventId))
                )
                .Select(x => new
                {
                    x.SuperNoticeId,
                    x.Id,
                    x.Name,
                    x.IsVisible,
                    x.Visibility,
                    x.State,
                    x.Start,
                    EventName = x.Event.Name ?? "",
                    ConcessionName = x.PaymentConcession.Concession.Name
                })
                .OrderByDescending(x => x.Start)
                .ToList()
                .Select(x => new ApiNoticeGetAllResult
                {
                    SuperNoticeId = x.SuperNoticeId,
                    Id = x.Id,
                    Name = x.Name,
                    IsVisible = x.IsVisible,
                    Visibility = x.Visibility,
                    State = x.State,
                    Start = (x.Start == XpDateTime.MinValue) ? (DateTime?)null : x.Start.ToUTC(),
                    EventName = x.EventName ?? "",
                    ConcessionName = x.ConcessionName ?? ""
                });

            //if (!arguments.Filter.IsNullOrEmpty())
            //	items = items.Where(x =>
            //		x.Name.Contains(arguments.Filter) ||
            //		x.Event.Name.Contains(arguments.Filter)
            //	);

            var result = items
                .Where(x =>
                    (x.SuperNoticeId == null)
                );

            var temp = result.ToList();
            while (temp.Any())
            {
                var item = temp.FirstOrDefault();
                temp = temp.Skip(1).ToList();

                var subItems = items
                    .Where(x => x.SuperNoticeId == item.Id)
                    .ToList();
                item.SubNotices = subItems;
                if (subItems.Any())
                    temp.AddRange(subItems);
            }

			return new ResultBase<ApiNoticeGetAllResult> { Data = result };
		}
		#endregion ExecuteAsync
	}
}
