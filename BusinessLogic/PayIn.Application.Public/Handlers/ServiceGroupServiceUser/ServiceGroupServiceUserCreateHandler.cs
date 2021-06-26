using Microsoft.Practices.Unity;
using PayIn.Application.Dto.Arguments;
using PayIn.BusinessLogic.Common;
using PayIn.Common;
using PayIn.Common.Resources;
using PayIn.Domain.Public;
using System;
using System.Linq;
using System.Threading.Tasks;
using Xp.Application.Dto;
using Xp.Domain;
using Xp.Infrastructure.Repositories;

namespace PayIn.Application.Public.Handlers
{
	public class ServiceGroupServiceUserCreateHandler : IServiceBaseHandler<ServiceGroupServiceUsersCreateArguments>
	{
		[Dependency] public IEntityRepository<ServiceGroup> RepositoryGroup { get; set; }
		[Dependency] public IEntityRepository<ServiceUser> RepositoryUser { get; set; }

		#region ExecuteAsync
		public async Task<dynamic> ExecuteAsync(ServiceGroupServiceUsersCreateArguments arguments)
		{

			var Group = (await RepositoryGroup.GetAsync(arguments.GroupId));

			var User = (await RepositoryUser.GetAsync(arguments.UserId, "Groups"));

			User.Groups.Add(Group);

			return User;
		}
		#endregion ExecuteAsync
	}
}
