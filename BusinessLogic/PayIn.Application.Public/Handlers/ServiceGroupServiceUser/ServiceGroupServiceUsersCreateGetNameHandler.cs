using PayIn.Application.Dto.Arguments;
using PayIn.Application.Dto.Results;
using PayIn.BusinessLogic.Common;
using Microsoft.Practices.Unity;
using Xp.Application.Dto;
using System;
using System.Linq;
using System.Threading.Tasks;
using Xp.Domain;
using PayIn.Domain.Public;

namespace PayIn.Application.Dto.Handlers
{
    public class TransportCardSupportTitleCompatibilityCreateGetNameHandler : IQueryBaseHandler<ServiceGroupServiceUsersCreateGetNameArguments, ServiceGroupServiceUsersCreateGetNameResult>
    {
        [Dependency] public ISessionData SessionData { get; set; }
        [Dependency] public IEntityRepository<ServiceGroup> ServiceGroupRepository { get; set; }

        #region ExecuteAsync
        public async Task<ResultBase<ServiceGroupServiceUsersCreateGetNameResult>> ExecuteAsync(ServiceGroupServiceUsersCreateGetNameArguments arguments)
        {
            var groupName = (await ServiceGroupRepository.GetAsync())
                .Where(x => x.Id == arguments.Id)
                .Select(x => x.Name)
                .FirstOrDefault();
            if (groupName == null)
                throw new ArgumentNullException("Id");

            return new ServiceGroupServiceUsersCreateGetNameResultBase
			{
				GroupName = groupName
			};
        }
        #endregion ExecuteAsync
    }
}








