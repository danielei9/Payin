using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Practices.Unity;
using PayIn.Application.Dto.Payments.Arguments;
using PayIn.Application.Dto.Payments.Results;
using PayIn.BusinessLogic.Common;
using PayIn.Common;
using PayIn.Domain.Payments;
using PayIn.Domain.Public;
using Xp.Application.Dto;
using Xp.Domain;

namespace PayIn.Application.Payments.Handlers
{
	public class ProductGetAllHandler : 
		IQueryBaseHandler<ProductGetAllArguments, ProductGetAllResult>
	{
		[Dependency] public IEntityRepository<ProductFamily> ProductFamilyRepository { get; set; }
		[Dependency] public IEntityRepository<ServiceGroup> ServiceGroupRepository { get; set; }
		[Dependency] public IEntityRepository<Product> Repository { get; set; }
		[Dependency] public ISessionData SessionData { get; set; }

		#region ExecuteAsync
		public async Task<ResultBase<ProductGetAllResult>> ExecuteAsync(ProductGetAllArguments arguments)
		{
			var serviceGroups = (await ServiceGroupRepository.GetAsync());

            var products_ = (await Repository.GetAsync())
                .Where(x =>
                    (x.State == ProductState.Active) &&
                    (
                        (x.PaymentConcession.Concession.Supplier.Login == SessionData.Login) ||
                        (x.PaymentConcession.PaymentWorkers
                            .Any(y => y.Login == SessionData.Login)
                        )
                    )
                );
            if (!arguments.Filter.IsNullOrEmpty())
                products_ = products_
                    .Where(x =>
                        x.Name.Contains(arguments.Filter) ||
                        x.Description.Contains(arguments.Filter)
                    );
            var products = products_
				.Select(x => new ProductGetAllResult
				{
					Id = x.Id,
					Name = x.Name,
                    Code = x.Code,
					Price = x.Price,
					SuperFamilyId = x.FamilyId,
					Type = ProductGetAllResult.TypeEnum.Product,
					IsVisible = x.IsVisible,
					Visibility = x.Visibility,
					GroupsCount = serviceGroups
						.Where(y =>
							y.Category.ServiceConcessionId == x.PaymentConcession.ConcessionId &&
							y.Products
								.Any(z => z.ProductId == x.Id)
						)
						.Count()
				})
                .ToList();

            var families_ = (await ProductFamilyRepository.GetAsync())
                .Where(x =>
                    (x.State == ProductFamilyState.Active) &&
                    (
                        (x.PaymentConcession.Concession.Supplier.Login == SessionData.Login) ||
                        (x.PaymentConcession.PaymentWorkers
                            .Any(y => y.Login == SessionData.Login)
                        )
                    )
                );
            if (!arguments.Filter.IsNullOrEmpty())
                families_ = families_
                    .Where(x =>
                        (x.Name).Contains(arguments.Filter) ||
                        (x.Description).Contains(arguments.Filter)
                    );
            var families = families_
                .Select(x => new ProductGetAllResult
				{
					Id = x.Id,
					Name = x.Name,
                    Code = x.Code,
					Description = x.Description,
					PhotoUrl = x.PhotoUrl,
					Price = null,
					SuperFamilyId = x.SuperFamilyId,
					Type = ProductGetAllResult.TypeEnum.Family,
					IsVisible = x.IsVisible,
					Visibility = ProductVisibility.Public
				})
                .ToList();

			var result = products
                .Union(families)
				.ToList() as IEnumerable<ProductGetAllResult>;

			// Calculate TreeId and TreeLevel
			var familiesDictionary = families.ToDictionary(x => x.Id, x => x);
			foreach (var item in result)
			{
				item.TreeId = GetTreeId(familiesDictionary, item);
				item.TreeLevel = item.TreeId.Split('_').Count() - 1;
			}
			result = result
				.OrderBy(x => x.TreeId);

			return new ResultBase<ProductGetAllResult> {
				Data = result
			};
		}
		#endregion ExecuteAsync

		#region GetTreeId
		public string GetTreeId(Dictionary<int, ProductGetAllResult> items, ProductGetAllResult item, string prefix = "")
		{
			if (!item.TreeId.IsNullOrEmpty())
				return item.TreeId;
			if (item.SuperFamilyId == null)
				return item.Id.ToString();

			if (!items.ContainsKey(item.SuperFamilyId.Value))
				return item.Id.ToString();
			
			return GetTreeId(items, items[item.SuperFamilyId.Value]) + "_" + item.Id.ToString();
		}
		#endregion GetTreeId
	}
}
