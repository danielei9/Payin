using PayIn.Application.Dto.Payments.Arguments;
using PayIn.Application.Dto.Payments.Results;
using PayIn.Domain.Payments;
using System;
using System.Linq;
using System.Threading.Tasks;
using Xp.Application.Dto;
using Xp.Domain;

namespace PayIn.Application.Payments.Handlers
{
	public class ProductFamilyGetHandler :
		IQueryBaseHandler<ProductFamilyGetArguments, ProductFamilyGetResult>
	{
		private readonly IEntityRepository<ProductFamily> Repository;
		
		#region Constructors
		public ProductFamilyGetHandler(
			IEntityRepository<ProductFamily> repository
		)
		{
			if (repository == null) throw new ArgumentNullException("repository");

			Repository = repository;
		}
		#endregion Constructors

		#region ExecuteAsync
		public async Task<ResultBase<ProductFamilyGetResult>> ExecuteAsync(ProductFamilyGetArguments arguments)
		{
            var items = (await Repository.GetAsync())
                .Where(x => x.Id == arguments.Id);

            var familyName = "";
            if (items.FirstOrDefault().SuperFamilyId != null)
            {
                var parentName = (await Repository.GetAsync())
                    .Where(x => x.Id == items.FirstOrDefault().SuperFamilyId)
                    .Select(y => y.Name)
                    .FirstOrDefault();

                familyName = parentName;
            }

			var result = items
                .Select( x => new ProductFamilyGetResult
                {
                    Id = x.Id,
                    Name = x.Name,
                    Code = x.Code,
					Description = x.Description,
					PhotoUrl = x.PhotoUrl,
                    ParentId = x.SuperFamilyId,
                    FamilyName = familyName,
					IsVisible = x.IsVisible
                });

			return new ResultBase<ProductFamilyGetResult> { Data = result };
		}
		#endregion ExecuteAsync
	}
}

