using Microsoft.Practices.Unity;
using PayIn.Application.Dto.Payments.Arguments;
using PayIn.Application.Dto.Payments.Results;
using PayIn.BusinessLogic.Common;
using PayIn.Domain.Payments;
using PayIn.Domain.Security;
using System;
using System.Linq;
using System.Threading.Tasks;
using Xp.Application.Dto;
using Xp.Domain;

namespace PayIn.Application.Payments.Handlers
{
    public class ApiAccessControlGetPlaceHandler :
        IQueryBaseHandler<ApiAccessControlGetPlaceArguments, ApiAccessControlGetPlaceResult>
    {
        [Dependency] public IEntityRepository<AccessControl> Repository { get; set; }
        [Dependency] public ISessionData SessionData { get; set; }

        #region ExecuteAsync

        public async Task<ResultBase<ApiAccessControlGetPlaceResult>> ExecuteAsync(ApiAccessControlGetPlaceArguments arguments)
        {
            var accesses = (await Repository.GetAsync())
                .Where(x => x.Id == arguments.Id);

            var result = accesses
                .Select(x => new
                {
                    x.Id,
                    Name = x.Name.Replace("\"", "'"),
                    x.CurrentCapacity,
                    x.MaxCapacity,
                    Entries = x.Entrances
                    .Select(y => new
                    {
                        y.Id,
                        y.Name
                    }),
                })
                .OrderBy(x => x.Name)
                .ToList()
                .Select(x => new ApiAccessControlGetPlaceResult
                {
                    Id = x.Id,
                    Name = x.Name,
                    CurrentCapacity = x.CurrentCapacity,
                    MaxCapacity = x.MaxCapacity,
                    DateTime = DateTime.UtcNow,
                    Entries = x.Entries
                });

            return new ResultBase<ApiAccessControlGetPlaceResult> { Data = result };
        }

        #endregion
    }
}
