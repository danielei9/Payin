using PayIn.Application.Dto.Arguments.ServiceAddress;
using System;
using System.Threading.Tasks;
using Xp.Application.Dto;
using Xp.Domain;

namespace PayIn.Application.Public.Handlers
{
	public class ServiceAddressUpdateHandler :
		IServiceBaseHandler<ServiceAddressUpdateArguments>
	{
		private readonly IEntityRepository<PayIn.Domain.Public.ServiceAddress> _Repository;

		#region Constructors
		public ServiceAddressUpdateHandler(IEntityRepository<PayIn.Domain.Public.ServiceAddress> repository)
		{
			if (repository == null)
				throw new ArgumentNullException("repository");
			_Repository = repository;
		}
		#endregion Constructors

		#region ExecuteAsync
		async Task<dynamic> IServiceBaseHandler<ServiceAddressUpdateArguments>.ExecuteAsync(ServiceAddressUpdateArguments arguments)
		{
			var itemServiceAddress = await _Repository.GetAsync(arguments.Id);
			itemServiceAddress.CityId = arguments.CityId;
			itemServiceAddress.From = arguments.From;
			itemServiceAddress.Until = arguments.Until;
			//itemServiceAddress.Name = arguments.Name;

			return itemServiceAddress;
		}
		#endregion ExecuteAsync
	}
}
