using PayIn.Application.Dto;
using PayIn.Application.Dto.Arguments.ServiceCity;
using PayIn.Application.Dto.Results.ServiceCity;
using PayIn.Domain.Public;
using System;
using System.Linq;
using System.Threading.Tasks;
using Xp.Application.Dto;
using Xp.Domain;

namespace PayIn.Application.Public.Handlers
{
	public class CityGetByConcessionTypeHandler :
		IQueryBaseHandler<ServiceCityGetByZoneTypeArguments, ServiceCityGetByZoneTypeResult>
	{
		private readonly IEntityRepository<ServiceCity> _Repository;

		#region Constructors
		public CityGetByConcessionTypeHandler(IEntityRepository<ServiceCity> repository)
		{
			if (repository == null)
				throw new ArgumentNullException("repository");
			_Repository = repository;
		}
		#endregion Constructors

		#region ExecuteAsync
		async Task<ResultBase<ServiceCityGetByZoneTypeResult>> IQueryBaseHandler<ServiceCityGetByZoneTypeArguments, ServiceCityGetByZoneTypeResult>.ExecuteAsync(ServiceCityGetByZoneTypeArguments arguments)
		{
			var items = await _Repository.GetAsync();

			var result = items
				.Where(x => x.Addresses
					.Any(y => y.Zone.Concession.Type == arguments.Type)
				)
				.Select(x => new ServiceCityGetByZoneTypeResult
				{
					Id = x.Id,
					Name = x.Name,
					Addresses = x.Addresses.Select(y => new ServiceCityGetByZoneTypeResult.ServiceCityGetByZoneType_AddressResult
					{
						Id = y.Id,
						Name = y.Name,
						Side = y.Side,
						From = y.From,
						Until = y.Until,
						Zone = new CalculateZone
						{
							Id = y.Zone.Id,
							Name = y.Zone.Name,
							ConcessionId = y.Zone.ConcessionId,
							ConcessionName = y.Zone.Concession.Name,
							SupplierId = y.Zone.Concession.SupplierId,
							SupplierName = y.Zone.Concession.Supplier.Name,
							Prices = y.Zone.Prices.Select(z => new CalculatePrice
							{
								Id = z.Id,
								Price = z.Price,
								Time = z.Time
							}),
							TimeTable = y.Zone.TimeTables.Select(z => new CalculateTimeTable
							{
								Id = z.Id,
								FromWeekday = z.FromWeekday,
								UntilWeekday = z.UntilWeekday,
								FromHour = z.FromHour,
								DurationHour = z.DurationHour
							})
						}
					})
				})
				.OrderByDescending(x => x.Id)
			;

			return new ResultBase<ServiceCityGetByZoneTypeResult> { Data = result };
		}
		#endregion ExecuteAsync
	}
}
