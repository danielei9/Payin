using PayIn.Application.Dto.Arguments.ServiceAddressName;
using PayIn.Domain.Public;
using System;
using System.Threading.Tasks;
using Xp.Application.Dto;
using Xp.Domain;

namespace PayIn.Application.Public.Handlers
{
	public class ServiceAddressNameUpdateHandler :
		IServiceBaseHandler<ServiceAddressNameUpdateArguments>
	{
		private readonly IEntityRepository<ServiceAddressName> _Repository;

		#region Constructors
		public ServiceAddressNameUpdateHandler(IEntityRepository<ServiceAddressName> repository)
		{
			if (repository == null)
				throw new ArgumentNullException("repository");
			_Repository = repository;
		}
		#endregion Constructors

		#region ExecuteAsync
		async Task<dynamic> IServiceBaseHandler<ServiceAddressNameUpdateArguments>.ExecuteAsync(ServiceAddressNameUpdateArguments arguments)
		{
			var itemServiceNameAddress = await _Repository.GetAsync(arguments.Id);
			itemServiceNameAddress.Name = arguments.Name;
			itemServiceNameAddress.ProviderMap = arguments.ProviderMap;

			return itemServiceNameAddress;
		}
		#endregion ExecuteAsync
	}
}
