using Microsoft.Practices.Unity;
using PayIn.Application.Dto.Arguments;
using PayIn.Application.Dto.Results;
using PayIn.Common;
using PayIn.Domain.Public;
using System.Linq;
using System.Threading.Tasks;
using Xp.Application.Dto;
using Xp.Domain;

namespace PayIn.Application.Public.Handlers
{
    public class ApiControlFormGetHandler :
		IQueryBaseHandler<ApiControlFormGetArguments, ApiControlFormGetResult>
	{
		[Dependency] public IEntityRepository<ControlForm> Repository { get; set; }

		#region ExecuteAsync
		public async Task<ResultBase<ApiControlFormGetResult>> ExecuteAsync(ApiControlFormGetArguments arguments)
		{
            var items = (await Repository.GetAsync());
            if (arguments.Ids != null)
                items = items
                    .Where(x => arguments.Ids.Contains(x.Id));

            var result = items
                .Where(x => x.State == ControlFormState.Active)
                .Select(x => new ApiControlFormGetResult
                {
                    Id = x.Id,
                    Name = x.Name,
                    Observations = x.Observations,
                    Arguments = x.Arguments
                        .Where(y => y.State == ControlFormArgumentState.Active)
                        .Select(y => new ApiControlFormGetResult_Arguments
                        {
                            Id = y.Id,
                            Name = y.Name,
                            Observations = y.Observations,
                            Type = y.Type,
                            Target = y.Target,
                            MinOptions = y.MinOptions,
                            MaxOptions = y.MaxOptions,
                            Options = y.Options
                                .Where(z => z.State == ControlFormOptionState.Active)
                                .Select(z => new ApiControlFormGetResult_Options
                                {
                                    Id = z.Id,
                                    Text = z.Text,
                                    Value = z.Value
                                })
                        })
                })
#if DEBUG
				.ToList()
#endif // DEBUG
				;

			return new ResultBase<ApiControlFormGetResult> { Data = result };
		}
		#endregion ExecuteAsync
	
	}
}
