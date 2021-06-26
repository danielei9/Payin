using Microsoft.Practices.Unity;
using PayIn.Application.Dto.Arguments;
using PayIn.Application.Dto.Results;
using PayIn.Domain.Public;
using System.Linq;
using System.Threading.Tasks;
using Xp.Application.Dto;
using Xp.Domain;

namespace PayIn.Application.Public.Handlers
{
    public class ServiceUserGetServiceGroupsHandler :
		IQueryBaseHandler<ServiceUserGetServiceGroupsArguments, ServiceUserGetServiceGroupsResult>
	{
		[Dependency] public IEntityRepository<ServiceGroup> Repository		{ get; set; }
		[Dependency] public IEntityRepository<ServiceUser> RepositoryUser	{ get; set; }

		#region ExecuteAsync
		public async Task<ResultBase<ServiceUserGetServiceGroupsResult>> ExecuteAsync(ServiceUserGetServiceGroupsArguments arguments)
		{
			var userName = (await RepositoryUser.GetAsync())
				.Where(x => x.Id == arguments.UserId)
				.Select(x => new {
					Name = x.Name,
					LastName = x.LastName
				})
				.FirstOrDefault();

			var items = (await Repository.GetAsync())
				.Where(x => x.Users
					.Any(y => y.Id == arguments.UserId)
				)
				.Select(x => new ServiceUserGetServiceGroupsResult
				{
					Id = x.Id,
					Name = x.Name
				});

			return new ServiceUserGetServiceGroupsResultBase
			{
				Data = items,
				UserName = userName.Name + " " + userName.LastName
			};

		}
        #endregion ExecuteAsync
    }
}

