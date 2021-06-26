using Microsoft.Practices.Unity;
using PayIn.Application.Dto.Payments.Arguments;
using PayIn.Application.Dto.Payments.Results;
using PayIn.BusinessLogic.Common;
using PayIn.Domain.Payments;
using System.Linq;
using System.Threading.Tasks;
using Xp.Application.Dto;
using Xp.Domain;

namespace PayIn.Application.Payments.Handlers
{
    public class ApiAccessControlGetHandler : 
		IQueryBaseHandler<ApiAccessControlGetArguments, ApiAccessControlGetResult>
	{
		[Dependency] public IEntityRepository<AccessControl> Repository { get; set; }
		[Dependency] public ISessionData SessionData { get; set; }

		#region ExecuteAsync

		public async Task<ResultBase<ApiAccessControlGetResult>> ExecuteAsync(ApiAccessControlGetArguments arguments)
		{
			var accesses = (await Repository.GetAsync())
				.Where(x => x.Id == arguments.Id);

			var result = accesses
				.Select(x => new
				{
					x.Id,
					Name = x.Name.Replace("\"", "'"),
					x.Schedule,
					x.MapUrl,
					x.CurrentCapacity,
					x.MaxCapacity,
				})
                .OrderBy(x => x.Name)
				.ToList()
				.Select(x => new ApiAccessControlGetResult
				{
					Id = x.Id,
					Name = x.Name,
					Schedule = x.Schedule,
					Map = x.MapUrl,
					CurrentCapacity = x.CurrentCapacity,
					MaxCapacity = x.MaxCapacity,
				});

			return new ResultBase<ApiAccessControlGetResult> { Data = result }; ;
		}

		#endregion
	}
}
