using PayIn.Application.Dto.Arguments.ServicePrice;
using PayIn.Domain.Public;
using System;
using System.Threading.Tasks;
using Xp.Application.Dto;
using Xp.Domain;

namespace PayIn.Application.Public.Handlers
{
	public class ServicePriceUpdateHandler :
		IServiceBaseHandler<ServicePriceUpdateArguments>
	{
		private readonly IEntityRepository<ServicePrice> _Repository;

		#region Constructors
		public ServicePriceUpdateHandler(IEntityRepository<ServicePrice> repository)
		{
			if (repository == null)
				throw new ArgumentNullException("repository");
			_Repository = repository;
		}
		#endregion Constructors

		#region ExecuteAsync
		async Task<dynamic> IServiceBaseHandler<ServicePriceUpdateArguments>.ExecuteAsync(ServicePriceUpdateArguments arguments)
		{
			var item = await _Repository.GetAsync(arguments.Id);
			item.Price = arguments.Price;
			item.Time = arguments.Time;

			return item;
		}
		#endregion ExecuteAsync
	}
}
