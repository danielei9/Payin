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
	class MobileNoticeGetAllByEventHandler :
		IQueryBaseHandler<MobileNoticeGetAllByEventArguments, MobileNoticeGetAllByEventResult>
	{
		private readonly IEntityRepository<Notice> Repository;
		private readonly ISessionData SessionData;

		#region Constructors
		public MobileNoticeGetAllByEventHandler(
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
		public async Task<ResultBase<MobileNoticeGetAllByEventResult>> ExecuteAsync(MobileNoticeGetAllByEventArguments arguments)
		{
			var result = (await Repository.GetAsync())
				.Where(x =>
					x.EventId == arguments.Id &&
					x.State != NoticeState.Deleted &&
					x.IsVisible == true &&
					x.Visibility != NoticeVisibility.Internal
				)
				.Select(x => new
				{
					Id = x.Id,
					Name = x.Name,
					ShortDescription = x.ShortDescription,
					PhotoUrl = x.PhotoUrl,
					Start = x.Start
				})
				.ToList()
				.Select(x => new MobileNoticeGetAllByEventResult
				{
					Id = x.Id,
					Name = x.Name,
					ShortDescription = x.ShortDescription,
					PhotoUrl = x.PhotoUrl,
					Start = x.Start.ToUTC()
				});
			return new ResultBase<MobileNoticeGetAllByEventResult> { Data = result };
		}
		#endregion ExecuteAsync
	}
}