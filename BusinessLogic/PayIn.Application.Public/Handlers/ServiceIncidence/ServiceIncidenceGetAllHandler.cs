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
    public class ServiceIncidenceGetAllHandler :
        IQueryBaseHandler<ServiceIncidenceGetAllArguments, ServiceIncidenceGetAllResult>
    {
		[Dependency] public IEntityRepository<ServiceIncidence> Repository { get; set; }

        #region ExecuteAsync
        public async Task<ResultBase<ServiceIncidenceGetAllResult>> ExecuteAsync(ServiceIncidenceGetAllArguments arguments)
		{
			var items = (await Repository.GetAsync())
				.Where(x =>
					(
						x.State != IncidenceState.Deleted
					)
				);


				//.Where(x =>
					//		//(
					//		//	(x.Concession.Supplier.Login == SessionData.Login && x.Concession.State == ConcessionState.Active) ||
					//		//	(x.PaymentWorker.Login == SessionData.Login && x.PaymentWorker.State == WorkerState.Active)
					//		//) ||
					//(
					//arguments.ServiceUserId == null ||
					//x.ServiceUserId == arguments.ServiceUserId
					//)
				//);


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
				.Select(x => new ServiceIncidenceGetAllResult
				{
					Id = x.Id,
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
				});

			return new ResultBase<ServiceIncidenceGetAllResult> { Data = result };
		}
		#endregion ExecuteAsync
	}
}
