using Microsoft.Practices.Unity;
using PayIn.Application.Dto.Arguments.ServiceAddress;
using PayIn.Domain.Public;
using System;
using System.Threading.Tasks;
using Xp.Application.Dto;
using Xp.Domain;

namespace PayIn.Application.Public.Handlers
{
	public class ServiceAddressCreateHandler :
		IServiceBaseHandler<ServiceAddressCreateArguments>
	{
		[Dependency] public IEntityRepository<ServiceAddress> _Repository { get; set; }
		[Dependency] public IEntityRepository<ServiceAddressName> _RepositoryServiceAddressName { get; set; }

		#region ExecuteAsync
		async Task<dynamic> IServiceBaseHandler<ServiceAddressCreateArguments>.ExecuteAsync(ServiceAddressCreateArguments arguments)
		{
			var itemServiceAddress = new ServiceAddress
			{
				CityId = arguments.CityId,
				ZoneId = arguments.ZoneId,
				From = arguments.From,
				Until = arguments.Until,
				Side = arguments.Side
			};
			await _Repository.AddAsync(itemServiceAddress);

			var itemServiceAddressName = new ServiceAddressName
			{
				Address = itemServiceAddress,
				Name = arguments.Name
			};
			await _RepositoryServiceAddressName.AddAsync(itemServiceAddressName);
			return itemServiceAddress;
		}
		#endregion ExecuteAsync
	}
}
