using Microsoft.Practices.Unity;
using PayIn.Application.Dto.Arguments.Main;
using PayIn.Application.Dto.Results.Main;
using PayIn.Domain.Payments.Infrastructure;
using PayIn.Domain.Public;
using System.Linq;
using System.Threading.Tasks;
using Xp.Application.Dto;
using Xp.Domain;

namespace PayIn.Application.Public.Handlers.Main
{
    public class MobileMainGetVersionHandler : 
		IQueryBaseHandler<MobileMainGetVersionArguments, MainMobileGetVersionResult>
	{
		[Dependency] public IEntityRepository<ServiceOption> ServiceOptionRepository { get; set; }
		[Dependency] public IInternalService InternalService { get; set; }

		#region ExecuteAsync
		public async Task<ResultBase<MainMobileGetVersionResult>> ExecuteAsync(MobileMainGetVersionArguments arguments)
        {
            var internalVersions = await InternalService.VersionAsync();

			var result = (await ServiceOptionRepository.GetAsync())
				.Where(x => x.Name == "ServerVersionName")
				.Select(x => new MainMobileGetVersionResult
                {
                    PublicServerVersion = "",
                    PublicDbVersion = x.Value,
                    InternalDbVersion = internalVersions.InternalDbVersion
				});

            return new ResultBase<MainMobileGetVersionResult> {
                Data = result
            };
		}
		#endregion ExecuteAsync
	}
}
