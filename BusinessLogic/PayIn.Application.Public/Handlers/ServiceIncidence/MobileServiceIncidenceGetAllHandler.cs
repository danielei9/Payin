using Microsoft.Practices.Unity;
using PayIn.Application.Dto.Arguments;
using PayIn.Application.Dto.Results;
using Xp.Application.Dto;
using System.Threading.Tasks;
using Xp.Domain;
using PayIn.Domain.Public;
using PayIn.BusinessLogic.Common;
using System.Linq;

using System;
using PayIn.Common;

namespace PayIn.Application.Public.Handlers
{
    public class MobileServiceIncidenceGetAllHandler :
        IQueryBaseHandler<MobileServiceIncidenceGetAllArguments, MobileServiceIncidenceGetAllResult>
    {
		[Dependency] public IEntityRepository<ServiceIncidence> Repository { get; set; }
		[Dependency] public ISessionData SessionData { get; set; }

        #region ExecuteAsync
        public async Task<ResultBase<MobileServiceIncidenceGetAllResult>> ExecuteAsync(MobileServiceIncidenceGetAllArguments arguments)
		{

			var items = (await Repository.GetAsync())
				.Where(x =>
					x.State != IncidenceState.Deleted &&
					x.Notifications.Any(y =>
						y.SenderLogin == SessionData.Login ||
						y.ReceiverLogin == SessionData.Login
					)
				);

			if (!arguments.Filter.IsNullOrEmpty())
				items = items
					.Where(x => x.Name.Contains(arguments.Filter));

			var result = items
				.Select(x => new 
				{
					x.Id,
					x.Name,
					x.Type,
					x.Category,
					x.SubCategory,
					x.DateTime,
					x.State,
					x.Notifications
				})
				.OrderBy(x =>
					x.State
				)
				.ThenByDescending(x =>
					x.DateTime
				)
				.ToList()
				.Select(x => new MobileServiceIncidenceGetAllResult
				{
					Id = x.Id,
					Name = x.Name,
					Type = x.Type,
					TypeName = x.Type.ToEnumAlias(),
					Category = x.Category,
					CategoryName = x.Category.ToEnumAlias(),
					SubCategory = x.SubCategory,
					SubCategoryName = x.SubCategory.ToEnumAlias(),
					DateTime = x.DateTime,
					State = x.State,
					StateName = x.State.ToEnumAlias(),
					Notifications = x.Notifications
				})
				.ToList();

			return new ResultBase<MobileServiceIncidenceGetAllResult> { Data = result };
		}
		#endregion ExecuteAsync
	}
}
