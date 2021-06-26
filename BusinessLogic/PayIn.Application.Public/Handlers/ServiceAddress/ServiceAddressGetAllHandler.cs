using Microsoft.Practices.Unity;
using PayIn.Application.Dto.Arguments.ServiceAddress;
using PayIn.Application.Dto.Results.ServiceAddress;
using PayIn.BusinessLogic.Common;
using PayIn.Common;
using PayIn.Domain.Public;
using System;
using System.Linq;
using System.Threading.Tasks;
using Xp.Application.Dto;
using Xp.Domain;

namespace PayIn.Application.Public.Handlers
{
	public class ServiceAddressGetAllHandler :
		IQueryBaseHandler<ServiceAddressGetAllArguments, ServiceAddressGetAllResult>
	{
		[Dependency] private ISessionData                      SessionData { get; set; }
		[Dependency] private IEntityRepository<ServiceAddress> Repository  { get; set; }

		#region ExecuteAsync
		async Task<ResultBase<ServiceAddressGetAllResult>> IQueryBaseHandler<ServiceAddressGetAllArguments, ServiceAddressGetAllResult>.ExecuteAsync(ServiceAddressGetAllArguments arguments)
		{
			var items = await Repository.GetAsync();

			//if (arguments.CityId != null)
			//items = items.Where(x => x.CityId == arguments.CityId);
			items = items.Where(x => x.Zone.Concession.Supplier.Login == SessionData.Login);

			if (!arguments.Filter.IsNullOrEmpty())
				items = items.Where(x => (
					x.Name.Contains(arguments.Filter) ||
					x.Zone.Name.Contains(arguments.Filter) ||
					x.Zone.Concession.Name.Contains(arguments.Filter) ||
					x.Names.Any(y => y.Name.Contains(arguments.Filter))
				));

			var result = items
				.Select(x => new ServiceAddressGetAllResult
				{
					Id = x.Id,
					Names = x.Names.Select(y => new ServiceAddressGetAllResult.ServiceAddressGetAll_NamesResult
					{
						Id = y.Id,
						Name = y.Name,
						ProviderMap = y.ProviderMap
					}),
					Name = x.Name,
					Side = x.Side,
					From = x.From,
					Until = x.Until,
					ZoneId = x.ZoneId,
					ZoneName = x.Zone.Name,
					ConcessionId = x.Zone.ConcessionId,
					ConcessionName = x.Zone.Concession.Name
				})
				.ToList()
				.Select(x => new ServiceAddressGetAllResult
				{
					Id = x.Id,
					Names = x.Names.Select(y => new ServiceAddressGetAllResult.ServiceAddressGetAll_NamesResult
					{
						Id = y.Id,
						Name = y.Name,
						ProviderMap = y.ProviderMap
					}),
					Name = x.Name,
					Side = x.Side,
					From = x.From,
					Until = x.Until,
					ZoneId = x.ZoneId,
					ZoneName = x.ZoneName,
					ConcessionId = x.ConcessionId,
					ConcessionName = x.ConcessionName
				})
				.OrderBy(x => new
				{
					x.ConcessionName,
					x.ZoneName,
					x.Name,
					x.From
				});

			return new ResultBase<ServiceAddressGetAllResult> { Data = result };
		}
		#endregion ExecuteAsync
	}
}
