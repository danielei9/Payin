using System.Linq;
using System.Threading.Tasks;
using Microsoft.Practices.Unity;
using PayIn.Application.Dto.Payments.Arguments;
using PayIn.Application.Dto.Payments.Results;
using PayIn.Domain.Payments;
using Xp.Application.Dto;
using Xp.Domain;

namespace PayIn.Application.Payments.Handlers
{
	public class ProductGetHandler :
		IQueryBaseHandler<ProductGetArguments, ProductGetResult>
	{
		[Dependency] public IEntityRepository<Product> Repository { get; set; }

		#region ExecuteAsync
		public async Task<ResultBase<ProductGetResult>> ExecuteAsync(ProductGetArguments arguments)
		{
            var items = (await Repository.GetAsync())
                .Where(x => x.Id == arguments.Id)
                .Select(x => new ProductGetResult
                {
                    Id = x.Id,
                    Name = x.Name,
                    Code = x.Code,
                    Description = x.Description,
                    PhotoUrl = x.PhotoUrl,
                    Price   = x.Price,
                    FamilyId = x.FamilyId,
                    FamilyName = x.Family.Name ?? "",
					IsVisible = x.IsVisible,
					Visibility = x.Visibility,
					SellableInTpv = x.SellableInTpv,
					SellableInWeb = x.SellableInWeb
                })
				.ToList();

            return new ResultBase<ProductGetResult> { Data = items};
		}
		#endregion ExecuteAsync
	}
}

