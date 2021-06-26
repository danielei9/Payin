using Microsoft.Practices.Unity;
using PayIn.Application.Dto.Payments.Arguments;
using PayIn.Application.Dto.Payments.Results;
using PayIn.BusinessLogic.Common;
using PayIn.Domain.Payments;
using System;
using System.Linq;
using System.Threading.Tasks;
using Xp.Application.Attributes;
using Xp.Application.Dto;
using Xp.Domain;

namespace PayIn.Application.Payments.Handlers
{
	[XpLog("Product", "MobileGetTpv")]
	public class ProductMobileGetTpvHandler :
		IQueryBaseHandler<ProductMobileGetTpvArguments, ProductMobileGetTpvResult>
	{
		[Dependency] public IEntityRepository<Product> Repository { get; set; }
		[Dependency] public ISessionData SessionData { get; set; }

		#region ExecuteAsync
		public async Task<ResultBase<ProductMobileGetTpvResult>> ExecuteAsync(ProductMobileGetTpvArguments arguments)
		{
            var result = (await Repository.GetAsync())
                .Where(x =>
					(x.SellableInWeb) &&
					(x.PaymentConcession.Concession.State == Common.ConcessionState.Active) &&
					(x.PaymentConcessionId == arguments.PaymentConcessionId) &&
					//(
					//	(x.PaymentConcession.Concession.Supplier.Login == SessionData.Login) ||
					//	(x.PaymentConcession.PaymentWorkers.Any(y => y.Login == SessionData.Login))
					//) &&
					(x.State == Common.ProductState.Active) &&
					(x.IsVisible) &&
					(x.Visibility == Common.ProductVisibility.Public)
				)
                .Select(x => new ProductMobileGetTpvResult
                {
                    Id = x.Id,
                    Name = x.Name,
                    PhotoUrl = x.PhotoUrl,
                    Price   = x.Price,
                    ConcessionId = x.PaymentConcessionId,
					ConcessionName = x.PaymentConcession.Concession.Name
				});

            return new ResultBase<ProductMobileGetTpvResult> { Data = result};
		}
		#endregion ExecuteAsync
	}
}

