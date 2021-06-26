using PayIn.Application.Dto.Arguments.ServiceCheckPoint;
using PayIn.Domain.Public;
using System;
using System.Threading.Tasks;
using Xp.Application.Dto;
using Xp.Domain;

namespace PayIn.Application.Public.Handlers
{
	public class ServiceCheckPointDeleteHandler :
		IServiceBaseHandler<ServiceCheckPointDeleteArguments>
	{
		private readonly IEntityRepository<ServiceCheckPoint> Repository;

		#region Constructors
		public ServiceCheckPointDeleteHandler(IEntityRepository<ServiceCheckPoint> repository)
		{
			if (repository == null) throw new ArgumentNullException("repository");
			Repository = repository;
		}
		#endregion Constructors

		#region ServiceCheckPointDelete
		public async Task<dynamic> ExecuteAsync(ServiceCheckPointDeleteArguments arguments)
		{
			var item = await Repository.GetAsync(arguments.Id);
			await Repository.DeleteAsync(item);
			return null;
		}
		#endregion ServiceCheckPointDelete
	}
}
