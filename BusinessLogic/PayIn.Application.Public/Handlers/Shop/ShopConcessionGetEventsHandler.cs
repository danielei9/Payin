using Microsoft.Practices.Unity;
using PayIn.Application.Dto.Arguments.Shop;
using PayIn.Application.Dto.Results.Shop;
using PayIn.BusinessLogic.Common;
using PayIn.Common;
using PayIn.Domain.Payments;
using PayIn.Domain.Public;
using PayIn.Infrastructure.Security;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xp.Application.Dto;
using Xp.Common;
using Xp.Domain;

namespace PayIn.Application.Public.Handler.Shop
{
    public class ShopGetEventsConcessions :
        IQueryBaseHandler<ShopConcessionGetEventsArguments, ShopConcessionGetEventsResult>
    {
        [Dependency] public IEntityRepository<Event> Repository { get; set; }
		[Dependency] public IEntityRepository<ServiceUser> ServiceUserRepository { get; set; }
		[Dependency] public IEntityRepository<PaymentConcession> paymentConcessionRepository { get; set; }
		[Dependency] public SecurityRepository securityRepository { get; set; }

		#region ExecuteAsync
		public async Task<ResultBase<ShopConcessionGetEventsResult>> ExecuteAsync(ShopConcessionGetEventsArguments arguments)
        {
            var now = DateTime.UtcNow;

			var users = (await ServiceUserRepository.GetAsync());

			var supplier = (await paymentConcessionRepository.GetAsync())
				.Where(x => x.Id == arguments.Id)
				.Select(x => new
				{
					Login = x.Concession.Supplier.Login
				})
				.FirstOrDefault();
			if (supplier == null)
				throw new ApplicationException("Compañia suministradora no especificada o inválida");

			var userSecurity = await securityRepository.GetUserAsync(supplier.Login);
			var logoUrl = userSecurity.PhotoUrl + (userSecurity.PhotoUrl == "" ? "" : "?now=" + DateTime.Now.ToString());

			var result = (await Repository.GetAsync())
                .Where(x =>
                    x.State == EventState.Active &&
                    x.PaymentConcession.Id == arguments.Id &&
                    x.PaymentConcession.Concession.State == ConcessionState.Active &&
                    x.EntranceTypes.Any() &&
                    x.IsVisible
                )
                .Select(x => new
                {
                    Id = x.Id,
                    Name = x.Name,
                    PhotoUrl = x.PhotoUrl,
                    Place = x.Place,
                    Date = x.EventStart,
                    PriceStart = x.EntranceTypes
                        .Select(y => y.Price)
                        .Min(),
                    ConcessionId = x.PaymentConcessionId,
                    ConcessionName = x.PaymentConcession.Concession.Name,
                    ConcessionPhotoUrl = users
						.Where(y => y.Login == x.PaymentConcession.Concession.Supplier.Login)
						.Select(y => y.Photo)
						.FirstOrDefault()
                })
                .ToList()
                .Select(x => new ShopConcessionGetEventsResult
                {
                    Id = x.Id,
                    Name = x.Name,
                    Place = x.Place,
                    Date = x.Date == XpDateTime.MinValue ? 
						(DateTime?)null :
						x.Date.ToUTC(),
                    PhotoUrl = x.PhotoUrl,
                    PriceStart = x.PriceStart,
                    ConcessionId = x.ConcessionId,
                    ConcessionName = x.ConcessionName,
                    ConcessionPhotoUrl = x.ConcessionPhotoUrl
                });

			var rst = new ShopConcessionGetEventsResultBase
			{
				Data = result,
				ConcessionPhotoUrl = "",
				ConcessionLogoUrl = logoUrl
			};

            return rst;
        }
        #endregion ExecuteAsync
    }
}
