using PayIn.Application.Dto.Payments.Arguments;
using PayIn.Common;
using PayIn.Domain.Payments;
using System;
using System.Linq;
using System.Threading.Tasks;
using Xp.Application.Dto;
using Xp.Domain;

namespace PayIn.Application.Payments.Handlers
{
	public class ProductFamilyDeleteHandler :
        IServiceBaseHandler<ProductFamilyDeleteArguments>
    {
        private readonly IEntityRepository<ProductFamily> Repository;

        #region Constructors
        public ProductFamilyDeleteHandler(IEntityRepository<ProductFamily> repository)
        {
            if (repository == null) throw new ArgumentNullException("repository");
            Repository = repository;
        }
        #endregion Constructors

        #region ExecuteAsync
        async Task<dynamic> IServiceBaseHandler<ProductFamilyDeleteArguments>.ExecuteAsync(ProductFamilyDeleteArguments arguments)
        {
            var item = (await Repository.GetAsync("Products", "SubFamilies"))
                .Where(x => x.Id == arguments.Id)
                .FirstOrDefault();

			foreach (var product in item.Products)
				product.FamilyId = item.SuperFamilyId;
            foreach (var prodFamilies in item.SubFamilies)
                prodFamilies.SuperFamilyId = item.SuperFamilyId;

            item.State = ProductFamilyState.Deleted;

            return null;
        }
        #endregion ExecuteAsync
    }
}