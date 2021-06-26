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
	public class ServiceGroupServiceUserGetSelectorHandler :
		IQueryBaseHandler<ServiceGroupServiceUsersGetSelectorArguments, ServiceGroupServiceUsersGetSelectorResult>
	{
		private readonly IEntityRepository<ServiceUser> Repository;

		#region Constructors
		public ServiceGroupServiceUserGetSelectorHandler(IEntityRepository<ServiceUser> repository)
		{
			if (repository == null)
				throw new ArgumentNullException("repository");
			Repository = repository;
		}
		#endregion Constructors

		#region ExecuteAsync
		public async Task<ResultBase<ServiceGroupServiceUsersGetSelectorResult>> ExecuteAsync(ServiceGroupServiceUsersGetSelectorArguments arguments)
		{
			var result = (await Repository.GetAsync())
				.Where(x => x.Name.Contains(arguments.Filter))
				.GroupBy(y => y.Name)
				.Select(z => z.FirstOrDefault())			
				.ToList()
				.Select (x => new ServiceGroupServiceUsersGetSelectorResult
				{
					Id = x.Id,
					Value = x.Name
				 });

			return new ResultBase<ServiceGroupServiceUsersGetSelectorResult> { Data = result };
		}
		#endregion ExecuteAsync
	}
}
