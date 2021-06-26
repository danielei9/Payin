using PayIn.Application.Dto.Arguments.ServiceZone;
using PayIn.Application.Dto.Results.ServiceZone;
using PayIn.BusinessLogic.Common;
using PayIn.Domain.Public;
using System;
using System.Linq;
using System.Threading.Tasks;
using Xp.Application.Dto;
using Xp.Domain;

namespace PayIn.Application.Public.Handlers
{
	public class ServiceZoneGetByConcessionHandler :
		IQueryBaseHandler<ServiceZoneGetByConcessionArguments, ServiceZoneGetByConcessionResult>
	{
		private readonly IEntityRepository<ServiceZone> _Repository;
		private readonly ISessionData _SessionData;

		#region Constructors
		public ServiceZoneGetByConcessionHandler(IEntityRepository<ServiceZone> repository, ISessionData sessionData)
		{
			if (repository == null)
				throw new ArgumentNullException("repository");
			_Repository = repository;
			if (sessionData == null)
				throw new ArgumentNullException("sessionData");
			_SessionData = sessionData;
		}
		#endregion Constructors

		#region ExecuteAsync
		async Task<ResultBase<ServiceZoneGetByConcessionResult>> IQueryBaseHandler<ServiceZoneGetByConcessionArguments, ServiceZoneGetByConcessionResult>.ExecuteAsync(ServiceZoneGetByConcessionArguments arguments)
		{
			var items = await _Repository.GetAsync();
			var result = items
				.Where(x =>
					x.ConcessionId == arguments.ConcessionId
				)
				.Select(x => new ServiceZoneGetByConcessionResult
				{
					Id = x.Id,
					Name = x.Name,
					ConcessionId = x.ConcessionId,
					CancelationAmount = x.CancelationAmount,
					Prices = x.Prices.Select(y => new ServiceZoneGetByConcessionResult.ServiceZoneGetByConcession_ServicePriceResult
					{
						Id = y.Id,
						Price = y.Price,
						Time = y.Time,
						ZoneId = y.ZoneId
					}),
					Addresses = x.Addresses.Select(y => new ServiceZoneGetByConcessionResult.ServiceZoneGetByConcession_ServiceAddressResult
					{
						Id = y.Id,
						//Name = y.Name,
						From = y.From,
						Until = y.Until,
						Side = y.Side,
						ZoneId = y.ZoneId,
						CityId = y.CityId,
					}),
					TimeTables = x.TimeTables.Select(y => new ServiceZoneGetByConcessionResult.ServiceZoneGetByConcession_ServiceTimeTableResult
					{
						Id = y.Id,
						DurationHour = y.DurationHour,
						FromHour = y.FromHour,
						FromWeekday = y.FromWeekday,
						UntilWeekday = y.UntilWeekday,
						ZoneId = y.ZoneId
					})
				});

			return new ResultBase<ServiceZoneGetByConcessionResult> { Data = result };
		}
		#endregion ExecuteAsync
	}
}
