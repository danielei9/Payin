using Microsoft.Practices.Unity;
using PayIn.Application.Dto.Arguments;
using PayIn.Application.Dto.Results;
using PayIn.Domain.Public;
using System;
using System.Linq;
using System.Threading.Tasks;
using Xp.Application.Dto;
using Xp.Domain;
using PayIn.BusinessLogic.Common;
using PayIn.Domain.Security;



namespace PayIn.Application.Public.Handlers
{
	public class ServiceGroupGetAllHandler :
		IQueryBaseHandler<ServiceGroupGetAllArguments, ServiceGroupGetAllResult>
	{
		[Dependency] public IEntityRepository<ServiceGroup> Repository { get; set; }
		[Dependency] public ISessionData SessionData { get; set; }

		#region ExecuteAsync
		public async Task<ResultBase<ServiceGroupGetAllResult>> ExecuteAsync(ServiceGroupGetAllArguments arguments)
		{
			var items = (await Repository.GetAsync());
			if (!SessionData.Roles.Contains(AccountRoles.Superadministrator))
			{
				 items = items 
					.Where(x =>
						(
							(x.Category.ServiceConcession.Supplier.Login == SessionData.Login)
						)
					);
			}

			if (!arguments.Filter.IsNullOrEmpty())
				items = items
					.Where(x => x.Name.Contains(arguments.Filter));

			var result = items
				.Select(x => new ServiceGroupGetAllResult
				{
					Id = x.Id,
					Name = x.Name,
					CategoryId = x.CategoryId ?? 0,
					State = 1,
					CounterGroupAfiliates = x.Users.Count(),
				})
				.ToList();

			return new ResultBase<ServiceGroupGetAllResult> { Data = result };
		}
	}
	#endregion ExecuteAsync
}

