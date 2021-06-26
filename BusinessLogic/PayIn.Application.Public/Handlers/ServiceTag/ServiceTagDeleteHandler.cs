using PayIn.Application.Dto.Arguments.ServiceTag;
using PayIn.Domain.Public;
using System;
using System.Threading.Tasks;
using Xp.Application.Dto;
using Xp.Domain;
namespace PayIn.Application.Public
{
	public class ServiceTagDeleteHandler :
		IServiceBaseHandler<ServiceTagDeleteArguments>
	{
		private readonly IEntityRepository<ServiceTag> Repository;
		#region Constructors
		public ServiceTagDeleteHandler(IEntityRepository<ServiceTag> repository)
		{
			if (repository == null) throw new ArgumentNullException("repository");
			Repository = repository;
		}
		#endregion Constructors
		#region ServiceTagDelete
		public async Task<dynamic> ExecuteAsync(ServiceTagDeleteArguments arguments)
		{
			var item = await Repository.GetAsync(arguments.Id);
			await Repository.DeleteAsync(item);
			return null;
		}
		#endregion ServiceCheckPointDelete
	}
}
