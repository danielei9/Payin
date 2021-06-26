using Microsoft.Practices.Unity;
using PayIn.Application.Dto.Payments.Arguments;
using PayIn.Common;
using PayIn.Domain.Payments;
using PayIn.Domain.Public;
using System;
using System.Linq;
using System.Threading.Tasks;
using Xp.Application.Dto;
using Xp.Domain;

namespace PayIn.Application.Payments.Handlers
{
	public class ProductDeleteHandler :
		IServiceBaseHandler<ProductDeleteArguments>
	{
		[Dependency] public IEntityRepository<Product> Repository { get; set; }
		[Dependency] public IEntityRepository<ServiceGroupProduct> ServiceGroupProductRepository { get; set; }

		#region ExecuteAsync
		async Task<dynamic> IServiceBaseHandler<ProductDeleteArguments>.ExecuteAsync(ProductDeleteArguments arguments)
		{
			var item = (await Repository.GetAsync())
				.Where(x => x.Id == arguments.Id)
				.FirstOrDefault();
			item.State = ProductState.Deleted;

			var linkedServiceGroups = (await ServiceGroupProductRepository.GetAsync())
				.Where(x =>
					x.ProductId == arguments.Id
				);
			await ServiceGroupProductRepository.DeleteAsync(linkedServiceGroups);

			return null;
		}
		#endregion ExecuteAsync
	}
}
