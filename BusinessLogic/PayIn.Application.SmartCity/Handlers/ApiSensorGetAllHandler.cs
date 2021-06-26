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
	public class ApiSensorGetAllHandler :
        IQueryBaseHandler<ApiSensorGetAllArguments, ApiSensorGetAllResult>
    {
        [Dependency] public IEntityRepository<Component> Repository { get; set; }

        #region ExecuteAsync
        public async Task<ResultBase<ApiSensorGetAllResult>> ExecuteAsync(ApiSensorGetAllArguments arguments)
		{
			var resultSql = (await Repository.GetAsync())
				.Where(x =>
					x.State != ComponentState.Delete &&
					x.Id == arguments.ComponentId
				)
				.OrderBy(x => x.Name)
				.Select(x => new {
					x.Id,
					x.Name,
					x.Type,
					Sensors = x.Sensors
						.Where(y =>
							y.State != SensorState.Delete
						)
						.OrderBy(y => y.Name)
						.Select(y => new {
							y.Id,
							y.Name,
							y.Code,
							y.Type,
							y.Updatable,
							y.HasMaximeter,
							TariffName = y.EnergyContract.Tariff.Name,
							ContractName = y.EnergyContract.Name,
							ContractCompany = y.EnergyContract.Company,
							y.LastValue,
							y.TargetValue,
							y.LastTimestamp,
							y.Unit,
							Prices = y.EnergyContract.Prices
								.Where(a => a.State == EnergyTariffPriceState.Active)
								.OrderBy(a => a.Period.Name)
								.Select(a => new
								{
									a.Id,
									a.Period.Color,
									Price = y.EnergyContract.Prices
										.Where(c => c.Id == a.Id)
										.Select(c => c.EnergyPrice)
										.FirstOrDefault(),
									a.PowerContract,
									a.PowerContractFactor,
									a.PowerContractUnit
								})
						})
				});
			var result = resultSql
				.ToList()
				.Select(x => new {
					x.Id,
					x.Name,
					x.Type,
					Sensors = x.Sensors
						.Select(y => new ApiSensorGetAllResult {
							Id = y.Id,
							Name = y.Name,
							Code = y.Code,
							Type = y.Type,
							Updatable = y.Updatable,
							LastValue = y.LastValue,
							TargetValue = y.TargetValue,
							Unit = y.Unit,
							HasMaximeter = y.HasMaximeter,
							LastTimestamp = y.LastTimestamp.ToUTC(),
							TariffName = y.TariffName,
							ContractName = y.ContractName,
							ContractCompany = y.ContractCompany,
							Prices = y.Prices
								.Select(z => new ApiSensorGetAllResult_Price
								{
									Id = z.Id,
									Price = z.Price,
									PowerContract = z.PowerContract,
									PowerContractFactor = z.PowerContractFactor,
									PowerContractUnit = z.PowerContractUnit,
									Color = z.Color
								})
						})
				})
				.FirstOrDefault();

			return new ApiSensorGetAllResultBase
			{
				Id = result?.Id,
				Name = result?.Name ?? "",
				Data = result?.Sensors
			};
		}
		#endregion ExecuteAsync
	}
}
