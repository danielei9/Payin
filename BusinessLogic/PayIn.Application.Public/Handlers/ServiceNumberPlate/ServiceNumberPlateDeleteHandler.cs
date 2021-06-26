using PayIn.Application.Dto.Arguments.ServiceNumberPlate;
using System;
using System.Threading.Tasks;
using Xp.Application.Dto;
using Xp.Domain;

namespace PayIn.Application.Public.Handlers.ServiceNumberPlate
{
	public class ServiceNumberPlateDeleteHandler :
		IServiceBaseHandler<ServiceNumberPlateDeleteArguments>
	{
		private readonly IEntityRepository<PayIn.Domain.Public.ServiceNumberPlate> Repository;

		#region Constructors
		public ServiceNumberPlateDeleteHandler(IEntityRepository<PayIn.Domain.Public.ServiceNumberPlate> repository)
		{
			if (repository == null) throw new ArgumentNullException("repository");
			Repository = repository;
		}
		#endregion Constructors

		#region ExecuteAsync
		async Task<dynamic> IServiceBaseHandler<ServiceNumberPlateDeleteArguments>.ExecuteAsync(ServiceNumberPlateDeleteArguments arguments)
		{
			var item = await Repository.GetAsync(arguments.Id);
			await Repository.DeleteAsync(item);

			return null;
		}
		#endregion ExecuteAsync
	}
}
