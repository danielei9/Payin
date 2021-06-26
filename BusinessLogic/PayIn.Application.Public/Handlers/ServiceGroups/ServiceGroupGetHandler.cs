using Microsoft.Practices.Unity;
using PayIn.Application.Dto.Arguments;
using PayIn.Application.Dto.Results;
using PayIn.Domain.Public;
using System;
using System.Linq;
using System.Threading.Tasks;
using Xp.Application.Dto;
using Xp.Domain;

namespace PayIn.Application.Public.Handlers
{
	public class ServiceGroupGetHandler :
		IQueryBaseHandler<ServiceGroupGetArguments, ServiceGroupGetResult>
	{
		[Dependency] public IEntityRepository<ServiceGroup> Repository { get; set; }
		[Dependency] public IEntityRepository<ServiceCategory> Category { get; set; }


		#region ExecuteAsync
		public async Task<ResultBase<ServiceGroupGetResult>> ExecuteAsync(ServiceGroupGetArguments arguments)
		{
			var items = (await Repository.GetAsync())
				.Where(x => x.Id == arguments.Id);

			var result = items
				.Select(x => new ServiceGroupGetResult
				{
					Id = x.Id,
					Name = x.Name,
					CategoryName = x.Category.Name,
					CategoryId = x.CategoryId ?? 0,
					Users = x.Users
						.Where(y => y.State != Common.ServiceUserState.Deleted)
						.Select(y => new ServiceGroupGetUserResult
						{
							Id = y.Id,
							Name = y.Name,
							LastName = y.LastName,
							Email = y.Email,
							State = y.State
						})					
				});

			return new ResultBase<ServiceGroupGetResult> { Data = result };
		}
	}
	#endregion ExecuteAsync
}

