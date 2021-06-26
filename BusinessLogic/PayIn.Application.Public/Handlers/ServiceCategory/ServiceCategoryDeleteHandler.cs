using Microsoft.Practices.Unity;
using PayIn.Application.Dto.Arguments;
using PayIn.Domain.Public;
using System;
using System.Linq;
using System.Threading.Tasks;
using Xp.Application.Dto;
using Xp.Domain;


namespace PayIn.Application.Public.Handlers
{
	public class ServiceCategoryDeleteHandler :
		IServiceBaseHandler<ServiceCategoryDeleteArguments>
	{
		[Dependency] public IEntityRepository<ServiceCategory> Repository { get; set; }
		[Dependency] public IEntityRepository<ServiceGroup> ServiceGroupRepository { get; set; }
		[Dependency] public IEntityRepository<ServiceConcession> RepositoryServiceConcession { get; set; }

		#region ExecuteAsync
		async Task<dynamic> IServiceBaseHandler<ServiceCategoryDeleteArguments>.ExecuteAsync(ServiceCategoryDeleteArguments arguments)
		{
			var groupsCount = (await ServiceGroupRepository.GetAsync())
				.Where(x => x.CategoryId == arguments.Id)
				.Select(x => x.Id)
				.Count();

			if (groupsCount > 0)
				throw new ArgumentException("La Categoría no está vacía");

			var item = await Repository.GetAsync(arguments.Id);
			await Repository.DeleteAsync(item);

			return null;
		}
		#endregion ExecuteAsync
	}
}
