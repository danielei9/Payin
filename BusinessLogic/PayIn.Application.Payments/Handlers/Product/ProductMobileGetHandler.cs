using PayIn.Application.Dto.Payments.Arguments;
using PayIn.Application.Dto.Payments.Results;
using PayIn.Domain.Payments;
using System;
using System.Linq;
using System.Threading.Tasks;
using Xp.Application.Attributes;
using Xp.Application.Dto;
using Xp.Domain;

namespace PayIn.Application.Payments.Handlers
{
	[XpLog("Product", "MobileGet")]
	public class ProductMobileGetHandler :
		IQueryBaseHandler<ProductMobileGetArguments, ProductMobileGetResult>
	{
		private readonly IEntityRepository<Product>	Repository;
		
		#region Constructors
		public ProductMobileGetHandler(
			IEntityRepository<Product> repository
		)
		{
			if (repository == null) throw new ArgumentNullException("repository");

			Repository = repository;
		}
		#endregion Constructors

		#region ExecuteAsync
		public async Task<ResultBase<ProductMobileGetResult>> ExecuteAsync(ProductMobileGetArguments arguments)
		{
            var result = (await Repository.GetAsync())
                .Where(x => x.Id == arguments.Id)
                .Select(x => new ProductMobileGetResult
                {
                    Id = x.Id,
                    Name = x.Name,
                    Description = x.Description,
                    PhotoUrl = x.PhotoUrl,
                    Price   = x.Price,
                    ConcessionId = x.PaymentConcessionId,
					ConcessionName = x.PaymentConcession.Concession.Name
				});

            return new ResultBase<ProductMobileGetResult> { Data = result};
		}
		#endregion ExecuteAsync
	}
}

