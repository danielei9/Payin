using Microsoft.Practices.Unity;
using PayIn.Application.Dto.Arguments;
using PayIn.Application.Dto.Results;
using PayIn.BusinessLogic.Common;
using PayIn.Common;
using PayIn.Domain.Public;
using System;
using System.Linq;
using System.Threading.Tasks;
using Xp.Application.Dto;
using Xp.Domain;

namespace PayIn.Application.Public.Handlers
{
    public class MobileServiceIncidenceGetHandler :
        IQueryBaseHandler<MobileServiceIncidenceGetArguments, MobileServiceIncidenceGetResult>
    {
		[Dependency] public IEntityRepository<ServiceIncidence> Repository { get; set; }
		[Dependency] public IEntityRepository<ServiceNotification> ServiceNotificationRepository { get; set; }
		[Dependency] public ISessionData SessionData { get; set; }
		[Dependency] public IUnitOfWork UnitOfWork { get; set; }

		#region ExecuteAsync
		public async Task<ResultBase<MobileServiceIncidenceGetResult>> ExecuteAsync(MobileServiceIncidenceGetArguments arguments)
		{
			var items = (await Repository.GetAsync())
				.Where(x =>
					x.Id == arguments.Id &&
					x.State != IncidenceState.Deleted &&
					x.Notifications.Any(y =>
						y.SenderLogin == SessionData.Login ||
						y.ReceiverLogin == SessionData.Login
					)
				);

			// Mark readed
			var toMarkRead = (await ServiceNotificationRepository.GetAsync())
				.Where(x =>
					x.IncidenceId == arguments.Id &&
					x.SenderLogin != SessionData.Login &&
					x.State == NotificationState.Actived
				);
			foreach (var item in toMarkRead)
				item.State = NotificationState.Read;
			await UnitOfWork.SaveAsync();

			var result = items
				.Select(x => new 
				{
					x.Name,
					x.Type,
					x.Category,
					x.SubCategory,
					x.DateTime,
					x.State,
					x.Notifications
				})
				.ToList()
				.Select(x => new MobileServiceIncidenceGetResult
				{
					Name = x.Name,
					Type = x.Type,
					TypeName = x.Type.ToEnumAlias(),
					Category = x.Category,
					CategoryName = x.Category.ToEnumAlias(),
					SubCategory = x.SubCategory,
					SubCategoryName = x.SubCategory.ToEnumAlias(),
					DateTime = x.DateTime.ToUTC(),
					State = x.State,
					StateName = x.State.ToEnumAlias(),
					Notifications = x.Notifications
				})
				.ToList();

			return new ResultBase<MobileServiceIncidenceGetResult> { Data = result };
		}
		#endregion ExecuteAsync
	}
}
