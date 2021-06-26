using PayIn.Application.Dto.Arguments.ServiceAddressName;
using PayIn.Domain.Public;
using System;
using System.Threading.Tasks;
using Xp.Application.Dto;
using Xp.Domain;

namespace PayIn.Application.Public.Handlers
{
	public class ServiceAddressNameDeleteHandler :
		IServiceBaseHandler<ServiceAddressNameDeleteArguments>
	{
		private readonly IEntityRepository<ServiceAddressName> Repository;

		#region Constructors
		public ServiceAddressNameDeleteHandler(IEntityRepository<ServiceAddressName> repository)
		{
			if (repository == null) throw new ArgumentNullException("repository");
			Repository = repository;
		}
		#endregion Constructors

		#region ServiceAddressDelete
		async Task<dynamic> IServiceBaseHandler<ServiceAddressNameDeleteArguments>.ExecuteAsync(ServiceAddressNameDeleteArguments arguments)
		{
			var item = await Repository.GetAsync(arguments.Id);
			await Repository.DeleteAsync(item);

			return null;
		}
		#endregion ServiceAddressDelete
	}
}
