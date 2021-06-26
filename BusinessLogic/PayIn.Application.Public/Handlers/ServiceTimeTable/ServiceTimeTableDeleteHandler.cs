using PayIn.Application.Dto.Arguments.ServiceTimeTable;
using PayIn.Domain.Public;
using System;
using System.Threading.Tasks;
using Xp.Application.Dto;
using Xp.Domain;

namespace PayIn.Application.Public.Handlers
{
	public class ServiceTimeTableDeleteHandler :
		IServiceBaseHandler<ServiceTimeTableDeleteArguments>
	{
		private readonly IEntityRepository<ServiceTimeTable> Repository;

		#region Constructors
		public ServiceTimeTableDeleteHandler(IEntityRepository<ServiceTimeTable> repository)
		{
			if (repository == null) throw new ArgumentNullException("repository");
			Repository = repository;
		}
		#endregion Constructors

		#region ExecuteAsync
		async Task<dynamic> IServiceBaseHandler<ServiceTimeTableDeleteArguments>.ExecuteAsync(ServiceTimeTableDeleteArguments arguments)
		{
			var item = await Repository.GetAsync(arguments.Id);
			await Repository.DeleteAsync(item);

			return null;
		}
		#endregion ExecuteAsync
	}
}
