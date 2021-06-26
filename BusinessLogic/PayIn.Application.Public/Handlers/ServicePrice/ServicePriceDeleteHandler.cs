using PayIn.Application.Dto.Arguments.ServicePrice;
using PayIn.Domain.Public;
using System;
using System.Threading.Tasks;
using Xp.Application.Dto;
using Xp.Domain;

namespace PayIn.Application.Public.Handlers
{
	public class ServicePriceDeleteHandler :
		IServiceBaseHandler<ServicePriceDeleteArguments>
	{
		private readonly IEntityRepository<ServicePrice> Repository;

		#region Constructors
		public ServicePriceDeleteHandler(IEntityRepository<ServicePrice> repository)
		{
			if (repository == null) throw new ArgumentNullException("repository");
			Repository = repository;
		}
		#endregion Constructors

		#region ExecuteAsync
		async Task<dynamic> IServiceBaseHandler<ServicePriceDeleteArguments>.ExecuteAsync(ServicePriceDeleteArguments arguments)
		{
			var item = await Repository.GetAsync(arguments.Id);
			await Repository.DeleteAsync(item);

			return null;
		}
		#endregion ExecuteAsync
	}
}
