using PayIn.Application.Dto.Arguments.ServiceAddress;
using System;
using System.Threading.Tasks;
using Xp.Application.Dto;
using Xp.Domain;

namespace PayIn.Application.Public.Handlers
{
	public class ServiceAddressDeleteHandler :
			IServiceBaseHandler<ServiceAddressDeleteArguments>
	{
		private readonly IEntityRepository<PayIn.Domain.Public.ServiceAddress> Repository;

		#region Constructors
		public ServiceAddressDeleteHandler(IEntityRepository<PayIn.Domain.Public.ServiceAddress> repository)
		{
			if (repository == null) throw new ArgumentNullException("repository");
			Repository = repository;
		}
		#endregion Constructors

		#region ServiceAddressDelete
		async Task<dynamic> IServiceBaseHandler<ServiceAddressDeleteArguments>.ExecuteAsync(ServiceAddressDeleteArguments arguments)
		{
			var item = await Repository.GetAsync(arguments.Id);
			await Repository.DeleteAsync(item);

			return null;
		}
		#endregion ServiceAddressDelete
	}
}
