using Microsoft.Practices.Unity;
using PayIn.Application.Dto.Arguments;
using PayIn.Application.Dto.Results;
using PayIn.BusinessLogic.Common;
using PayIn.Domain.Public;
using PayIn.Infrastructure.Security;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xp.Application.Dto;
using Xp.Domain;

namespace PayIn.Application.Public.Handlers
{
    public class ServiceIncidenceGetHandler :
        IQueryBaseHandler<ServiceIncidenceGetArguments, ServiceIncidenceGetResult>
    {
		[Dependency] public IEntityRepository<ServiceIncidence> Repository { get; set; }
		[Dependency] public SecurityRepository SecurityRepository { get; set; }
		[Dependency] public ISessionData SessionData { get; set; }

		#region ExecuteAsync
		public async Task<ResultBase<ServiceIncidenceGetResult>> ExecuteAsync(ServiceIncidenceGetArguments arguments)
		{
			var serviceUsers = (await SecurityRepository.GetUsers()).Data;

			var item = (await Repository.GetAsync())
				.Where(x => x.Id == arguments.Id)
				.Select(x => new
				{
					x.Name,
					x.Type,
					x.Category,
					x.SubCategory,
					x.DateTime,
					x.State,
					x.InternalObservations,
					UserLogin = SessionData.Login,
					SenderLogin = x.Notifications
						.Select(y => y.SenderLogin)
						.FirstOrDefault()
				})
				.FirstOrDefault();
			if (item == null)
				throw new ApplicationException("No se ha encontrado la incidencia");

			var serviceUser = serviceUsers
				.Where(x => x.UserName == item.SenderLogin)
				.FirstOrDefault();
			if (serviceUser == null)
				throw new ApplicationException("No se ha encontrado el emisor de la incidencia");

			var resultItem = new ServiceIncidenceGetResult
				{
					Name = item.Name,
					Type = item.Type,
					TypeName = item.Type.ToEnumAlias(),
					Category = item.Category,
					CategoryName = item.Category.ToEnumAlias(),
					SubCategory = item.SubCategory,
					SubCategoryName = item.SubCategory.ToEnumAlias(),
					DateTime = item.DateTime.ToUTC(),
					State = item.State,
					StateName = item.State.ToEnumAlias(),
					InternalObservations = item.InternalObservations,
					UserLogin = item.UserLogin,
					User_NameLastname = serviceUser.Name,
					User_Email = serviceUser.Email == "" ? serviceUser.UserName : serviceUser.Email,
					User_Phone = serviceUser.Phone ?? ""
				};

			var result = new List<ServiceIncidenceGetResult>();
			result.Add(resultItem);

			return new ResultBase<ServiceIncidenceGetResult> { Data = result };
		}
		#endregion ExecuteAsync
	}
}
