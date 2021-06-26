using PayIn.Application.Dto.Arguments.ServiceCheckPoint;
using PayIn.Domain.Public;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xp.Application.Dto;
using Xp.Domain;

namespace PayIn.Application.Public.Handlers
{
	public class ServiceCheckPointUpdateHandler :
		IServiceBaseHandler<ServiceCheckPointUpdateArguments>
	{
		private readonly IEntityRepository<ServiceCheckPoint> _Repository;

		#region Constructors
		public ServiceCheckPointUpdateHandler(IEntityRepository<ServiceCheckPoint> repository)
		{
			if (repository == null)
				throw new ArgumentNullException("repository");
			_Repository = repository;
		}
		#endregion Constructors

		#region ExecuteAsync
		async Task<dynamic> IServiceBaseHandler<ServiceCheckPointUpdateArguments>.ExecuteAsync(ServiceCheckPointUpdateArguments arguments)
		{
			var itemServiceCheckPoint = await _Repository.GetAsync(arguments.Id);
			itemServiceCheckPoint.Name = arguments.Name ?? "";
			itemServiceCheckPoint.Observations = arguments.Observations ?? "";
			itemServiceCheckPoint.Latitude = arguments.Latitude;
			itemServiceCheckPoint.Longitude = arguments.Longitude;
			itemServiceCheckPoint.TagId = arguments.TagId;
			itemServiceCheckPoint.Type = arguments.Type;
			return itemServiceCheckPoint;
		}
		#endregion ExecuteAsync
	}
}
