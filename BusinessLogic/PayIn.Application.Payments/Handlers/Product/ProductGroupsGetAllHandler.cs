using System.Linq;
using System.Threading.Tasks;
using Microsoft.Practices.Unity;
using PayIn.Application.Dto.Payments.Arguments;
using PayIn.Application.Dto.Payments.Results;
using PayIn.Domain.Payments;
using PayIn.Domain.Public;
using Xp.Application.Dto;
using Xp.Domain;

namespace PayIn.Application.Payments.Handlers
{
	public class ProductGroupsGetAllHandler :
		IQueryBaseHandler<ProductGroupsGetAllArguments, ProductGroupsGetAllResult>
	{
		[Dependency] public IEntityRepository<Product> Repository { get; set; }
		[Dependency] public IEntityRepository<ServiceGroup> ServiceGroupRepository { get; set; }

		#region ExecuteAsync
		public async Task<ResultBase<ProductGroupsGetAllResult>> ExecuteAsync(ProductGroupsGetAllArguments arguments)
		{
			var product = (await Repository.GetAsync())
				.Where(x => x.Id == arguments.Id)
				.Select(x => new {
					x.Name
				})
				.FirstOrDefault();

			var items = (await ServiceGroupRepository.GetAsync())
				.Where(x => x.Products.Any(y => y.ProductId == arguments.Id))
				.Select(x=> new ProductGroupsGetAllResult
				{
					Id = x.Id,
					Name = x.Name
				});

			return new ProductGroupsGetAllResultBase {
				ProductName = product?.Name ?? "",
				Data = items
			};
		}
		#endregion ExecuteAsync
	}
}

