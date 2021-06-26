using Microsoft.Practices.Unity;
using PayIn.Application.Dto.Payments.Arguments;
using PayIn.Application.Dto.Payments.Results;
using PayIn.BusinessLogic.Common;
using PayIn.Common;
using PayIn.Domain.Payments;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xp.Application.Dto;
using Xp.Common;
using Xp.Domain;

namespace PayIn.Application.Payments.Handlers
{
	public class ApiNoticeGetPagesHandler :
		IQueryBaseHandler<ApiNoticeGetPagesArguments, ApiNoticeGetPagesResult>
	{
		[Dependency] public IEntityRepository<Notice> Repository { get; set; }
        [Dependency] public ISessionData SessionData { get; set; }

		#region ExecuteAsync
		public async Task<ResultBase<ApiNoticeGetPagesResult>> ExecuteAsync(ApiNoticeGetPagesArguments arguments)
		{
			var items = (await Repository.GetAsync())
				.Where(x =>
					(
						(x.PaymentConcession.Concession.Supplier.Login == SessionData.Login) ||
						(x.PaymentConcession.Concession.Supplier.Workers.Select(y => y.Login).Contains(SessionData.Login))
					) &&
					(x.State != NoticeState.Deleted) &&
                    (x.Type == NoticeType.Page)
                )
                .Select(x => new ApiNoticeGetPagesResult
                {
                    Id = x.Id,
                    Name = x.Name,
                    IsVisible = x.IsVisible,
                    Visibility = x.Visibility,
                    State = x.State,
                    SuperNoticeId = x.SuperNoticeId,
                    SuperNoticeName = "_" + (x.SuperNotice.Name ?? ""),
                    ConcessionName = x.PaymentConcession.Concession.Name ?? ""
                })
                .OrderBy(x => x.SuperNoticeName)
                .ToList()
                ;

            foreach (var item in items)
            {
                item.Index = GetIndex(item, items);
                item.Level = GetLevel(item, items) - 1;
            }

            var result = items
                .OrderBy(x => x.Index)
                .ThenBy(x => x.Name)
                .Select(x => new ApiNoticeGetPagesResult
                {
                    Id = x.Id,
                    Name = x.Name,
                    HasChild = items
                        .Any(y => y.SuperNoticeId == x.Id),
                    IsVisible = x.IsVisible,
                    Visibility = x.Visibility,
                    State = x.State,
                    SuperNoticeId = x.SuperNoticeId,
                    SuperNoticeName = x.Index ?? "",
                    Level = x.Level,
                    ConcessionName = x.ConcessionName
                })
                ;
            //if (!arguments.Filter.IsNullOrEmpty())
            //	items = items.Where(x =>
            //		x.Name.Contains(arguments.Filter) ||
            //		x.Event.Name.Contains(arguments.Filter)
            //	);

            //var temp = result.ToList();
            //while (temp.Any())
            //{
            //    var item = temp.FirstOrDefault();
            //    temp = temp.Skip(1).ToList();

            //    var subItems = items
            //        .Where(x => x.SuperNoticeId == item.Id)
            //        .ToList();
            //    item.SubNotices = subItems;
            //    if (subItems.Any())
            //        temp.AddRange(subItems);
            //}

            return new ResultBase<ApiNoticeGetPagesResult> { Data = result };
		}
        #endregion ExecuteAsync

        #region GetIndex
        public string GetIndex(ApiNoticeGetPagesResult item, IEnumerable<ApiNoticeGetPagesResult> list)
        {
            return (list
                .Where(x => x.Id == item.SuperNoticeId)
                .Select(x => GetIndex(x, list))
                .FirstOrDefault() ?? ""
            ) + "_" + item.Name;
        }
        #endregion GetIndex

        #region GetLevel
        public int GetLevel(ApiNoticeGetPagesResult item, IEnumerable<ApiNoticeGetPagesResult> list)
        {
            return (list
                .Where(x => x.Id == item.SuperNoticeId)
                .Select(x => (int?)GetLevel(x, list))
                .FirstOrDefault() ?? 0
            ) + 1;
        }
        #endregion GetLevel
    }
}
