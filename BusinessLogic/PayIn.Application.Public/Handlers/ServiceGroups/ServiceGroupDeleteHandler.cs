using Microsoft.Practices.Unity;
using PayIn.Application.Dto.Arguments;
using PayIn.Domain.Public;
using System.Threading.Tasks;
using Xp.Application.Dto;
using Xp.Domain;
using System.Linq;


namespace PayIn.Application.Public.Handlers
{
    public class ServiceGroupDeleteHandler :
		IServiceBaseHandler<ServiceGroupDeleteArguments>
	{
		[Dependency] public IEntityRepository<ServiceGroup> Repository { get; set; }
		[Dependency] public IEntityRepository<ServiceGroupProduct> ServiceGroupProductsRepository { get; set; }

		#region ExecuteAsync
		async Task<dynamic> IServiceBaseHandler<ServiceGroupDeleteArguments>.ExecuteAsync(ServiceGroupDeleteArguments arguments)
		{
			var item = await Repository.GetAsync(arguments.Id, "Users", "Products");
						
			if (item.Users.Count > 0)
				throw new System.ApplicationException("El grupo no se puede eliminar porque contiene usuarios");

			if (item.Products.Count > 0)
			{
				var products = (await ServiceGroupProductsRepository.GetAsync())
					.Where(x => x.GroupId==arguments.Id);

				foreach (var product in products)
				{
					await ServiceGroupProductsRepository.DeleteAsync(product);
				}
			}

			await Repository.DeleteAsync(item);

			return null;
		}
		#endregion ExecuteAsync
	}
}
