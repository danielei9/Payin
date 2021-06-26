using PayIn.Application.Dto.SmartCity.Arguments;
using PayIn.Application.Dto.SmartCity.Results;
using PayIn.Domain.Security;
using PayIn.Web.Security;
using System.Threading.Tasks;
using System.Web.Http;
using Xp.Application.Dto;
using Xp.DistributedServices.Filters;
using Xp.DistributedServices.ModelBinder;

namespace PayIn.DistributedServices.SmartCity.Controllers.Api
{
	[HideSwagger]
	[RoutePrefix("SmartCity/Api/Sensor")]
    [XpAuthorize(
           ClientIds = AccountClientId.Web,
           Roles = AccountRoles.Superadministrator + "," + AccountRoles.Operator + "," + AccountRoles.Commerce + "," + AccountRoles.CommercePayment
    )]
    public class ApiSensorController : ApiController
	{
		#region GET /
		[HttpGet]
		[Route()]
		public async Task<ResultBase<ApiSensorGetAllResult>> GetAll(
			[FromUri] ApiSensorGetAllArguments arguments,
			[Injection] IQueryBaseHandler<ApiSensorGetAllArguments, ApiSensorGetAllResult> handler
		)
		{
			var items = await handler.ExecuteAsync(arguments);
			return items;
		}
		#endregion GET /

		#region GET /Energy/{id:int}
		[HttpGet]
		[Route("Energy/{id:int}")]
		public async Task<ResultBase<ApiSensorGetEnergyResult>> GetEnergy(
			int id,
			[FromUri] ApiSensorGetEnergyArguments arguments,
			[Injection] IQueryBaseHandler<ApiSensorGetEnergyArguments, ApiSensorGetEnergyResult> handler
		)
		{
			arguments.Id = id;

			var items = await handler.ExecuteAsync(arguments);
			return items;
		}
		#endregion GET /Energy/{id:int}

		#region GET /MaxEnergy/{id:int}
		[HttpGet]
		[Route("MaxEnergy/{id:int}")]
		public async Task<ResultBase<ApiSensorGetMaxEnergyResult>> MaxEnergy(
			int id,
			[FromUri] ApiSensorGetMaxEnergyArguments arguments,
			[Injection] IQueryBaseHandler<ApiSensorGetMaxEnergyArguments, ApiSensorGetMaxEnergyResult> handler
		)
		{
			arguments.Id = id;

			var items = await handler.ExecuteAsync(arguments);
			return items;
		}
		#endregion GET /MaxEnergy/{id:int}

		#region GET /Power/{id:int}
		[HttpGet]
		[Route("Power/{id:int}")]
		public async Task<ResultBase<ApiSensorGetPowerResult>> GetPower(
			int id,
			[FromUri] ApiSensorGetPowerArguments arguments,
			[Injection] IQueryBaseHandler<ApiSensorGetPowerArguments, ApiSensorGetPowerResult> handler
		)
		{
			arguments.Id = id;

			var items = await handler.ExecuteAsync(arguments);
			return items;
		}
		#endregion GET /Power/{id:int}

		#region GET /Instantaneous/{id:int}
		[HttpGet]
		[Route("Instantaneous/{id:int}")]
		public async Task<ResultBase<ApiSensorGetInstantaneousResult>> GetInstantaneous(
			int id,
			[FromUri] ApiSensorGetInstantaneousArguments arguments,
			[Injection] IQueryBaseHandler<ApiSensorGetInstantaneousArguments, ApiSensorGetInstantaneousResult> handler
		)
		{
			arguments.Id = id;

			var items = await handler.ExecuteAsync(arguments);
			return items;
		}
		#endregion GET /Instantaneous/{id:int}

		#region GET /PerHour/{id:int}
		[HttpGet]
		[Route("PerHour/{id:int}")]
		public async Task<ResultBase<ApiSensorGetPerHourResult>> GetPerHour(
			int id,
			[FromUri] ApiSensorGetPerHourArguments arguments,
			[Injection] IQueryBaseHandler<ApiSensorGetPerHourArguments, ApiSensorGetPerHourResult> handler
		)
		{
			arguments.Id = id;

			var items = await handler.ExecuteAsync(arguments);
			return items;
		}
		#endregion GET /PerHour/{id:int}

		#region GET /TargetValues
		[HttpGet]
		[Route("TargetValues")]
		public async Task<ResultBase<ApiSensorGetTargetValuesResult>> GetTargetValues(
			[FromUri] ApiSensorGetTargetValuesArguments arguments,
			[Injection] IQueryBaseHandler<ApiSensorGetTargetValuesArguments, ApiSensorGetTargetValuesResult> handler
		)
		{
			var items = await handler.ExecuteAsync(arguments);
			return items;
		}
        #endregion GET /TargetValues

        #region PUT /SetTargetValue
        [HttpPut]
        [Route("SetTargetValue/{id:int}")]
        public async Task<dynamic> SetTargetValue(
            int id,
            [FromBody] ApiSmartCitySetTargetValueArguments arguments,
            [Injection] IServiceBaseHandler<ApiSmartCitySetTargetValueArguments> handler
        )
        {
            arguments.Id = id;

            var item = await handler.ExecuteAsync(arguments);
            return new { item.Id };
        }
        #endregion POST /SetTargetValue

        #region PUT /RemoveTargetValue
        [HttpPut]
        [Route("RemoveTargetValue/{id:int}")]
        public async Task<dynamic> RemoveTargetValue(
            int id,
            [FromBody] ApiSmartCityRemoveTargetValueArguments arguments,
            [Injection] IServiceBaseHandler<ApiSmartCitySetTargetValueArguments> handler
        )
        {
            var newArguments = new ApiSmartCitySetTargetValueArguments
            (
                id,
                null
            );

            var item = await handler.ExecuteAsync(newArguments);
            return new { item.Id };
        }
        #endregion POST /RemoveTargetValue

    }
}
