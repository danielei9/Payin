using PayIn.Application.Dto.Arguments.ServicePrice;
using PayIn.Domain.Public;
using System;
using System.Threading.Tasks;
using Xp.Application.Dto;
using Xp.Domain;

namespace PayIn.Application.Public.Handlers
{
	public class ServicePriceCreateHandler :
		IServiceBaseHandler<ServicePriceCreateArguments>
	{
		private readonly IEntityRepository<ServicePrice> _Repository;

		#region Constructors
		public ServicePriceCreateHandler(IEntityRepository<ServicePrice> repository)
		{
			if (repository == null)
				throw new ArgumentNullException("repository");
			_Repository = repository;
		}
		#endregion Constructors

		#region ExecuteAsync
		async Task<dynamic> IServiceBaseHandler<ServicePriceCreateArguments>.ExecuteAsync(ServicePriceCreateArguments arguments)
		{
			var item = new PayIn.Domain.Public.ServicePrice
			{
				Price = arguments.Price,
				Time = arguments.Time,
				ZoneId = arguments.ZoneId
			};
			await _Repository.AddAsync(item);

			return item;
		}
		#endregion ExecuteAsync
	}
}
