using Microsoft.Practices.Unity;
using PayIn.Application.Dto.Payments.Arguments;
using PayIn.Application.Dto.Payments.Results;
using PayIn.Domain.Payments;
using System.Linq;
using System.Threading.Tasks;
using Xp.Application.Dto;
using Xp.Domain;

namespace PayIn.Application.Payments.Handlers
{
    public class ApiAccessControlEntranceGetHandler :
        IQueryBaseHandler<ApiAccessControlEntranceGetArguments, ApiAccessControlEntranceGetResult>
    {
        [Dependency] public IEntityRepository<AccessControlEntrance> Repository { get; set; }

        #region ExecuteAsync

        public async Task<ResultBase<ApiAccessControlEntranceGetResult>> ExecuteAsync(ApiAccessControlEntranceGetArguments arguments)
        {
            var accesses = (await Repository.GetAsync())
                .Where(x => x.Id == arguments.Id);

            var result = accesses
                .Select(x => new
                {
                    x.Id,
                    Name = x.Name.Replace("\"", "'"),
                })
                .OrderBy(x => x.Name)
                .ToList()
                .Select(x => new ApiAccessControlEntranceGetResult
                {
                    Id = x.Id,
                    Name = x.Name,
                });

            return new ResultBase<ApiAccessControlEntranceGetResult> { Data = result }; ;
        }

        #endregion
    }
}
