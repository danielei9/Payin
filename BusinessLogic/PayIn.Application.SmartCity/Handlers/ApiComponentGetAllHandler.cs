using Microsoft.Practices.Unity;
using PayIn.Application.Dto.SmartCity.Arguments;
using PayIn.Application.Dto.SmartCity.Results;
using PayIn.Domain.SmartCity;
using PayIn.Domain.SmartCity.Enums;
using System;
using System.Linq;
using System.Threading.Tasks;
using Xp.Application.Dto;
using Xp.Domain;

namespace PayIn.Application.SmartCity.Handlers
{
	public class ApiComponentGetAllHandler :
        IQueryBaseHandler<ApiComponentGetAllArguments, ApiComponentGetAllResult>
    {
        [Dependency] public IEntityRepository<Component> Repository { get; set; }

        #region ExecuteAsync
        public async Task<ResultBase<ApiComponentGetAllResult>> ExecuteAsync(ApiComponentGetAllArguments arguments)
		{
			var result = (await Repository.GetAsync())
				.Where(x =>
					x.State != ComponentState.Delete &&
					x.DeviceId == arguments.DeviceId
				)
				.Select(x => new {
					x.Id,
					x.Name,
					x.Type,
					x.Model,
					SensorsNumber = x.Sensors
						.Where(y => y.State != SensorState.Delete)
						.Count(),
					LastTimestamp = x.Sensors
						.Max(y => y.LastTimestamp)
				})
				.ToList()
				.Select(x => new ApiComponentGetAllResult
				{
					Id = x.Id,
					Name = x.Name,
					Type = x.Type,
					TypeName = x.Type.ToEnumAlias(),
					Model = x.Model,
					SensorsNumber = x.SensorsNumber,
					LastTimestamp = x.LastTimestamp
				})
				;

			return new ResultBase<ApiComponentGetAllResult>
			{
				Data = result
			};
		}
		#endregion ExecuteAsync
	}
}
