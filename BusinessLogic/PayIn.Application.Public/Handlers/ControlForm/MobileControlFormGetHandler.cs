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
    public class MobileControlFormGetHandler :
		IQueryBaseHandler<MobileControlFormGetArguments, MobileControlFormGetResult>
	{
		[Dependency] public IEntityRepository<ControlForm> Repository { get; set; }

		#region ExecuteAsync
		public async Task<ResultBase<MobileControlFormGetResult>> ExecuteAsync(MobileControlFormGetArguments arguments)
		{
            var items = (await Repository.GetAsync());
            if (arguments.Ids != null)
                items = items
                    .Where(x => arguments.Ids.Contains(x.Id));

            var result = items
                .Where(x => x.State == ControlFormState.Active)
                .Select(x => new MobileControlFormGetResult
                {
                    Id = x.Id,
                    Name = x.Name,
                    Observations = x.Observations,
                    Arguments = x.Arguments
                        .Where(y => y.State == ControlFormArgumentState.Active)
                        .Select(y => new MobileControlFormGetResult_Arguments
                        {
                            Id = y.Id,
                            Name = y.Name,
                            Observations = y.Observations,
                            Type = y.Type,
                            MinOptions = y.MinOptions,
                            MaxOptions = y.MaxOptions,
                            Options = y.Options
                                .Where(z => z.State == ControlFormOptionState.Active)
                                .Select(z => new MobileControlFormGetResult_Options
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

			return new ResultBase<MobileControlFormGetResult> { Data = result };
		}
		#endregion ExecuteAsync
	}
}
