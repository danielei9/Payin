using PayIn.Application.Dto.Arguments.ServiceAddressName;
using PayIn.Domain.Public;
using System;
using System.Threading.Tasks;
using Xp.Application.Dto;
using Xp.Domain;

namespace PayIn.Application.Public.Handlers
{
	public class ServiceAddressNameCreateHandler :
		IServiceBaseHandler<ServiceAddressNameCreateArguments>
	{
		private readonly IEntityRepository<ServiceAddressName> _Repository;

		#region Constructors
		public ServiceAddressNameCreateHandler(IEntityRepository<ServiceAddressName> repository)
		{
			if (repository == null)
				throw new ArgumentNullException("repository");
			_Repository = repository;
		}
		#endregion Constructors

		#region ExecuteAsync
		async Task<dynamic> IServiceBaseHandler<ServiceAddressNameCreateArguments>.ExecuteAsync(ServiceAddressNameCreateArguments arguments)
		{
			var itemServiceAddressName = new ServiceAddressName
			{
				AddressId = arguments.AddressId,
				Name = arguments.Name,
				ProviderMap = arguments.ProviderMap
			};
			await _Repository.AddAsync(itemServiceAddressName);

			return itemServiceAddressName;
		}
		#endregion ExecuteAsync
	}
}
