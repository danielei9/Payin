using PayIn.Application.Dto.Payments.Arguments;
using PayIn.Domain.Payments;
using System;
using System.Linq;
using System.Threading.Tasks;
using Xp.Application.Dto;
using Xp.Domain;

namespace PayIn.Application.Payments.Handlers
{
	public class ProductFamilyUpdateHandler :
        IServiceBaseHandler<ProductFamilyUpdateArguments>
    {
        private readonly IEntityRepository<ProductFamily> Repository;

        #region Constructors
        public ProductFamilyUpdateHandler(
            IEntityRepository<ProductFamily> repository
        )
        {
            if (repository == null) throw new ArgumentNullException("repository");
            Repository = repository;
        }
        #endregion Constructors

        #region ExecuteAsync
        public async Task<dynamic> ExecuteAsync(ProductFamilyUpdateArguments arguments)
        {
            var family = (await Repository.GetAsync())
                .Where(x =>
                    x.Id == arguments.Id
                )
                .FirstOrDefault();

            family.Name             = arguments.Name;
            family.Description      = arguments.Description;
            family.SuperFamilyId    = arguments.ParentId;
			family.IsVisible		= arguments.IsVisible;
            family.Code             = arguments.Code;

			return family;
        }
        #endregion ExecuteAsync
    }
}