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
    public class MobileNoticeGetByEventHandler :
		IQueryBaseHandler<MobileNoticeGetByEventArguments, MobileNoticeGetByEventResult>
    {
        [Dependency] public IEntityRepository<Notice> Repository { get; set; }
        [Dependency] public ISessionData SessionData { get; set; }

		#region ExecuteAsync
		public async Task<ResultBase<MobileNoticeGetByEventResult>> ExecuteAsync(MobileNoticeGetByEventArguments arguments)
		{
			var item = (await Repository.GetAsync())
				.Where(x =>
                    (x.Id == arguments.Id) &&
                    (x.State == NoticeState.Active) &&
                    (x.IsVisible) &&
                    (
                        (x.Visibility != NoticeVisibility.Members) ||
                        (
                            x.Event.Entrances
                                .Where(y => y.Login == SessionData.Login)
                                .Any()
                        )
                    )
                );

			var result = item
				.Select(x => new 
				{
					x.Id,
					x.Name,
					x.ShortDescription,
					x.Description,
					x.PhotoUrl,
					x.Start
				})
				.ToList()
				.Select(x => new MobileNoticeGetByEventResult
				{
					Id = x.Id,
					Name = x.Name,
					ShortDescription = x.ShortDescription,
					Description = x.Description,
					PhotoUrl = x.PhotoUrl,
					Start = x.Start.ToUTC()
				});

			return new ResultBase<MobileNoticeGetByEventResult> { Data = result };
		}
		#endregion ExecuteAsync
	}
}