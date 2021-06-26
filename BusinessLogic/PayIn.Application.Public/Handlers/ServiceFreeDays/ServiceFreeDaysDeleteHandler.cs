using PayIn.Application.Dto.Arguments.ServiceFreeDays;
using PayIn.Domain.Public;
using System;
using System.Threading.Tasks;
using Xp.Application.Dto;
using Xp.Domain;

namespace PayIn.Application.Public.Handlers
{
	public class ServiceFreeDaysDeleteHandler :
			IServiceBaseHandler<ServiceFreeDaysDeleteArguments>
	{
		private readonly IEntityRepository<ServiceFreeDays> Repository;

		#region Constructors
		public ServiceFreeDaysDeleteHandler(IEntityRepository<ServiceFreeDays> repository)
		{
			if (repository == null) throw new ArgumentNullException("repository");
			Repository = repository;
		}
		#endregion Constructors

		#region ServiceFreeDaysDelete
		async Task<dynamic> IServiceBaseHandler<ServiceFreeDaysDeleteArguments>.ExecuteAsync(ServiceFreeDaysDeleteArguments arguments)
		{
			var item = await Repository.GetAsync(arguments.Id);
			await Repository.DeleteAsync(item);

			return null;
		}
		#endregion ServiceFreeDaysDelete
	}
}
