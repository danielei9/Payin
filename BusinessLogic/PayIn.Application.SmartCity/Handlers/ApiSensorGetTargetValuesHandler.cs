using Microsoft.Practices.Unity;
using PayIn.Application.Dto.SmartCity.Arguments;
using PayIn.Application.Dto.SmartCity.Results;
using PayIn.BusinessLogic.Common;
using PayIn.Domain.SmartCity;
using PayIn.Domain.SmartCity.Enums;
using System.Linq;
using System.Threading.Tasks;
using Xp.Application.Dto;
using Xp.Domain;

namespace PayIn.Application.SmartCity.Handlers
{
	public class ApiSensorGetTargetValuesHandler :
        IQueryBaseHandler<ApiSensorGetTargetValuesArguments, ApiSensorGetTargetValuesResult>
    {
        [Dependency] public IEntityRepository<Sensor> SensorRepository { get; set; }
		[Dependency] public ISessionData SessionData { get; set; }

		#region ExecuteAsync
		public async Task<ResultBase<ApiSensorGetTargetValuesResult>> ExecuteAsync(ApiSensorGetTargetValuesArguments arguments)
		{
			var result = (await SensorRepository.GetAsync())
				.Where(x =>
					x.State != SensorState.Delete &&
					x.Component.State != ComponentState.Delete &&
					x.Component.Device.State != DeviceState.Delete &&
					x.Component.Device.Concession.Login == SessionData.Login &&
					x.TargetValue != null
				)
				.OrderBy(x => x.Name)
				.Select(x => new ApiSensorGetTargetValuesResult {
					Id = x.Id,
					Code = x.Code,
					Type = x.Type,
					TargetValue = x.TargetValue.Value
				})
				.ToList();

			return new ResultBase<ApiSensorGetTargetValuesResult>
			{
				Data = result
			};
		}
		#endregion ExecuteAsync
	}
}
