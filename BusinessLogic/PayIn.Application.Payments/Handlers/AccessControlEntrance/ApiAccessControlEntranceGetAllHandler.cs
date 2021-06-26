using Microsoft.Practices.Unity;
using PayIn.Application.Dto.Payments.Arguments;
using PayIn.Application.Dto.Payments.Results;
using PayIn.BusinessLogic.Common;
using PayIn.Domain.Payments;
using System;
using System.Linq;
using System.Threading.Tasks;
using Xp.Application.Dto;
using Xp.Domain;

namespace PayIn.Application.Payments.Handlers
{
    public class ApiAccessControlEntranceGetAllHandler :
        IQueryBaseHandler<ApiAccessControlEntranceGetAllArguments, ApiAccessControlEntranceGetAllResult>
    {
        [Dependency] public IEntityRepository<AccessControlEntrance> Repository { get; set; }
        [Dependency] public ISessionData SessionData { get; set; }

        #region ExecuteAsync

        public async Task<ResultBase<ApiAccessControlEntranceGetAllResult>> ExecuteAsync(ApiAccessControlEntranceGetAllArguments arguments)
        {
            var accesses = (await Repository.GetAsync())
                .Where(x => x.AccessControlId == arguments.AccessControlId);

            if (!arguments.Filter.IsNullOrEmpty())
                accesses = accesses
                    .Where(x =>
                        x.Name.Contains(arguments.Filter)
                    );

            var result = accesses
                .Select(x => new
                {
                    x.Id,
                    Name = x.Name.Replace("\"", "'"),
                })
                .OrderBy(x => x.Name)
                .ToList()
                .Select(x => new ApiAccessControlEntranceGetAllResult
                {
                    Id = x.Id,
                    Name = x.Name
                });

            return new ResultBase<ApiAccessControlEntranceGetAllResult> { Data = result }; ;
        }

        #endregion
    }
}
