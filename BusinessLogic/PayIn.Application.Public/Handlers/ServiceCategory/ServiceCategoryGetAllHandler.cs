using Microsoft.Practices.Unity;
using PayIn.Application.Dto.Arguments;
using PayIn.Application.Dto.Results;
using PayIn.BusinessLogic.Common;
using PayIn.Domain.Public;
using System;
using System.Linq;
using System.Threading.Tasks;
using Xp.Application.Dto;
using Xp.Domain;

namespace PayIn.Application.Public.Handlers
{
	public class ServiceCategoryGetAllHandler :
		IQueryBaseHandler<ServiceCategoryGetAllArguments, ServiceCategoryGetAllResult>
	{
		[Dependency] public IEntityRepository<ServiceCategory> Repository { get; set; }
		[Dependency] public ISessionData SessionData { get; set; }

		#region ExecuteAsync
		public async Task<ResultBase<ServiceCategoryGetAllResult>> ExecuteAsync(ServiceCategoryGetAllArguments arguments)
		{
			var items = await Repository.GetAsync("ServiceGroup");

			if (!arguments.Filter.IsNullOrEmpty())
				items = items
					.Where(x => x.Name.Contains(arguments.Filter));


			var result = items
				.Where(x =>
					x.ServiceConcession.Supplier.Login == SessionData.Login ||
					x.ServiceConcession.Supplier.Workers
						.Any(y => y.Login == SessionData.Login)
				)
				.Select(x => new ServiceCategoryGetAllResult
				{
					Id = x.Id,
					Name = x.Name,
					ConcessionId = x.ServiceConcessionId,
					ConcessionName = x.ServiceConcession.Name,
					Groups = x.Groups.Select(grp => new ServiceCategoryGetAll_GroupResult
					{
						Id = grp.Id,
						Name = grp.Name,
						MembersCount = grp.Users
							.Where(y => y.State != Common.ServiceUserState.Deleted)
							.Count()
					})
				});

			return new ResultBase<ServiceCategoryGetAllResult> { Data = result };
		}
	}
	#endregion ExecuteAsync
}

