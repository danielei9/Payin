using Microsoft.Practices.Unity;
using PayIn.Application.Dto.SmartCity.Arguments;
using PayIn.Application.Dto.SmartCity.Results;
using PayIn.Domain.SmartCity;
using System;
using System.Linq;
using System.Threading.Tasks;
using Xp.Application.Dto;
using Xp.Domain;

namespace PayIn.Application.SmartCity.Handlers
{
	public class ApiEnergyTariffGetAllHandler :
        IQueryBaseHandler<ApiEnergyTariffGetAllArguments, ApiEnergyTariffGetAllResult>
    {
        [Dependency] public IEntityRepository<EnergyTariff> Repository { get; set; }

        #region ExecuteAsync
        public async Task<ResultBase<ApiEnergyTariffGetAllResult>> ExecuteAsync(ApiEnergyTariffGetAllArguments arguments)
		{
			var result = (await Repository.GetAsync())
				.Select(x => new {
					x.Id,
					x.Name,
					x.PowerMax,
					x.PowerMaxFactor,
					x.PowerMaxUnit,
					x.VoltageMax,
					x.VoltageMaxFactor,
					x.VoltageMaxUnit,
					Schedules = x.Schedules
						.Select(y => new
						{
							y.Id,
							y.Name,
							y.Since,
							y.Until,
							y.WeekDay,
							TimeTables = y.TimeTables
								.Select(z => new
								{
									z.Id,
									z.Since,
									z.Until,
									PeriodId = z.Period.Id,
									PeriodName = z.Period.Name,
									PeriodColor = z.Period.Color
								})
						})
				})
				.ToList()
				.Select(x => new ApiEnergyTariffGetAllResult
				{
					Id = x.Id,
					Name = x.Name,
					PowerMax = x.PowerMax,
					PowerMaxFactor = x.PowerMaxFactor,
					PowerMaxUnit = x.PowerMaxUnit,
					VoltageMax = x.VoltageMax,
					VoltageMaxFactor = x.VoltageMaxFactor,
					VoltageMaxUnit = x.VoltageMaxUnit,
					Schedules = x.Schedules
						.Select(y => new ApiEnergyTariffGetAllResult_Schedule
						{
							Id = y.Id,
							Name = y.Name,
							Since = y.Since,
							Until = y.Until,
							WeekDay = y.WeekDay,
							TimeTables = y.TimeTables
								.Select(z => new ApiEnergyTariffGetAllResult_TimeTable
								{
									Id = z.Id,
									Since = z.Since,
									Until = z.Until,
									PeriodId = z.PeriodId,
									PeriodName = z.PeriodName,
									PeriodColor = z.PeriodColor
								})
						})
				})
				;

			return new ResultBase<ApiEnergyTariffGetAllResult>
			{
				Data = result
			};
		}
		#endregion ExecuteAsync
	}
}
