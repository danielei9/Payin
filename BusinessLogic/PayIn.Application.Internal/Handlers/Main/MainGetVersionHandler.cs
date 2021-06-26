using Microsoft.Practices.Unity;
using PayIn.Application.Dto.Internal.Arguments;
using PayIn.Application.Dto.Internal.Results;
using PayIn.Domain.Internal;
using System.Linq;
using System.Threading.Tasks;
using Xp.Application.Dto;
using Xp.Domain;

namespace PayIn.Application.Internal.Handlers
{
	public class MainGetVersionHandler : 
		IQueryBaseHandler<MainGetVersionArguments, MainGetVersionResult>
	{
		[Dependency] public IEntityRepository<Option> OptionRepository { get; set; }

		#region ExecuteAsync
		public async Task<ResultBase<MainGetVersionResult>> ExecuteAsync(MainGetVersionArguments arguments)
        {
			var result = (await OptionRepository.GetAsync())
				.Where(x => x.Name == "ServerVersionName")
				.Select(x => new MainGetVersionResult
				{
					InternalDbVersion = x.Value
				});

			return new ResultBase<MainGetVersionResult> { Data = result };
		}
		#endregion ExecuteAsync
	}
}
