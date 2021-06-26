using Microsoft.Practices.Unity;
using PayIn.Application.Dto.SmartCity.Arguments;
using PayIn.Application.Dto.SmartCity.Results;
using PayIn.BusinessLogic.Common;
using PayIn.Domain.Security;
using PayIn.Domain.SmartCity;
using PayIn.Domain.SmartCity.Enums;
using System.Linq;
using System.Threading.Tasks;
using Xp.Application.Dto;
using Xp.Domain;

namespace PayIn.Application.SmartCity.Handlers
{
	public class ApiDeviceGetAllHandler :
        IQueryBaseHandler<ApiDeviceGetAllArguments, ApiDeviceGetAllResult>
    {
        [Dependency] public IEntityRepository<Device> Repository { get; set; }
		[Dependency] public ISessionData SessionData { get; set; }

		#region ExecuteAsync
		public async Task<ResultBase<ApiDeviceGetAllResult>> ExecuteAsync(ApiDeviceGetAllArguments arguments)
		{
			var result = (await Repository.GetAsync())
				.Where(x =>
					x.State != DeviceState.Delete &&
					(
						(SessionData.Roles.Contains(AccountRoles.Superadministrator)) ||
						(x.Concession.Login == SessionData.Login)
					)
				)
				.Select(x => new {
					x.Id,
					x.Name,
					x.Model,
					x.CO2Factor,
					ComponentNumber = x.Components
						.Where(y => y.State != ComponentState.Delete)
						.Count(),
					LastTimestamp = x.Components
						.Max(y => y.Sensors
							.Max(z => z.LastTimestamp)
						),
					x.ProviderName,
					x.ProviderCode,
					x.Concession.Login
				})
				.OrderBy(x => x.Name)
				.ToList()
				.Select(x => new ApiDeviceGetAllResult
				{
					Id = x.Id,
					Name = x.Name,
					Model = x.Model,
					CO2Factor = x.CO2Factor,
					ComponentsNumber = x.ComponentNumber,
					LastTimestamp = x.LastTimestamp,
					ProviderName = x.ProviderName,
					ProviderCode = x.ProviderCode,
					ConcessionLogin = x.Login
				})
				;

			return new ResultBase<ApiDeviceGetAllResult>
			{
				Data = result
			};
		}
		#endregion ExecuteAsync
	}
}
