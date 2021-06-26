using PayIn.Application.Dto.Arguments;
using PayIn.Application.Dto.Results;
using PayIn.BusinessLogic.Common;
using Microsoft.Practices.Unity;
using System;
using System.Linq;
using System.Threading.Tasks;
using Xp.Application.Dto;
using Xp.Domain;
using PayIn.Domain.Public;
using PayIn.Domain.Security;

namespace PayIn.Application.Dto.Handlers
{
	public class ServiceGroupServiceUserGetAllHandler : 
		IQueryBaseHandler<ServiceGroupServiceUsersGetAllArguments, ServiceGroupServiceUsersGetAllResult>
	{
		[Dependency] public IEntityRepository<ServiceUser> Repository { get; set; }
		[Dependency] public ISessionData SessionData { get; set; }
		[Dependency] public IEntityRepository<ServiceGroup> ServiceGroupRepository { get; set; }
		
		#region ExecuteAsync
		async Task<ResultBase<ServiceGroupServiceUsersGetAllResult>> IQueryBaseHandler<ServiceGroupServiceUsersGetAllArguments, ServiceGroupServiceUsersGetAllResult>.ExecuteAsync(ServiceGroupServiceUsersGetAllArguments arguments)
		{

			var groupName = (await ServiceGroupRepository.GetAsync())
			   .Where(x => x.Id == arguments.GroupId)
			   .Select(x => x.Name)
			   .FirstOrDefault();
			if (groupName == null)
				throw new ArgumentNullException("GroupId");

			var items = (await Repository.GetAsync());
			if (!SessionData.Roles.Contains(AccountRoles.Superadministrator))
			{
				items = items
				   .Where(x =>
					   (
						   (x.Concession.Supplier.Login == SessionData.Login)
					   )
				   );
			}
			if (!arguments.Filter.IsNullOrEmpty())
				items = items
					.Where(x =>
						(x.Name + " " + x.LastName).Contains(arguments.Filter) ||
						(x.Email.Contains(arguments.Filter)) ||
						(x.VatNumber.Contains(arguments.Filter))
					);

			var result = items
				.Where(x =>
					x.Groups
					.Any(y => y.Id == arguments.GroupId))
				.ToList()
			.Select(x => new ServiceGroupServiceUsersGetAllResult
			 {
				 Id = x.Id,
				 Name = x.Name,
				 LastName = x.LastName,
				 VatNumber = x.VatNumber,
				 State = x.State,
				 Email = x.Email
			 });

			return new ServiceGroupServiceUsersGetAllResultBase
			{
				Data = result,
				GroupName = groupName
			};
		}
		#endregion ExecuteAsync
	}
}
