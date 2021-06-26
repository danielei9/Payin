using PayIn.Application.Dto.Arguments.ServiceCheckPoint;
using PayIn.Domain.Public;
using System;
using System.Threading.Tasks;
using Xp.Application.Dto;
using Xp.Domain;

namespace PayIn.Application.Public.Handlers
{
	public class ServiceCheckPointCreateHandler :
		IServiceBaseHandler<ServiceCheckPointCreateArguments>
	{
		private readonly IEntityRepository<ServiceCheckPoint> _Repository;

		#region Constructors
		public ServiceCheckPointCreateHandler(IEntityRepository<ServiceCheckPoint> repository)
		{
			if (repository == null)
				throw new ArgumentNullException("repository");
			_Repository = repository;
		}
		#endregion Constructors

		#region ExecuteAsync
		public async Task<dynamic> ExecuteAsync(ServiceCheckPointCreateArguments arguments)
		{
			var itemServiceCheckPoint = new PayIn.Domain.Public.ServiceCheckPoint
			{
				Name = arguments.Name,
				Observations = "",
				SupplierId = arguments.SupplierId,
				Longitude = arguments.Longitude,
				Latitude = arguments.Latitude,				
				TagId = arguments.TagId,
				ItemId = arguments.ItemId,
				Type = arguments.Type
			};
			await _Repository.AddAsync(itemServiceCheckPoint);

			return itemServiceCheckPoint;
		}
		#endregion ExecuteAsync
	}
}
