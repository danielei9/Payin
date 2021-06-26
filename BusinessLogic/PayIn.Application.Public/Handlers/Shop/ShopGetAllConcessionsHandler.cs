using PayIn.Application.Dto.Arguments.Shop;
using PayIn.Application.Dto.Results.Shop;
using PayIn.BusinessLogic.Common;
using PayIn.Common;
using PayIn.Domain.Payments;
using PayIn.Domain.Public;
using System;
using System.Linq;
using System.Threading.Tasks;
using Xp.Application.Dto;
using Xp.Domain;

namespace PayIn.Application.Public.Handler.Shop
{
    public class ShopGetAllConcessionsHandler :
        IQueryBaseHandler<ShopGetAllConcessionsArguments, ShopGetAllConcessionsResult>
    {
        private readonly IEntityRepository<PaymentConcession> Repository;
		private readonly IEntityRepository<ServiceUser> ServiceUserRepository;

        #region Constructors
        public ShopGetAllConcessionsHandler(
            IEntityRepository<PaymentConcession> repository,
			IEntityRepository<ServiceUser> serviceUserRepository
		)
        {
			if (repository == null) throw new ArgumentNullException("repository");
			if (serviceUserRepository == null) throw new ArgumentNullException("serviceUserRepository");
			
			Repository = repository;
			ServiceUserRepository = serviceUserRepository;
        }
        #endregion Constructors

        #region ExecuteAsync
        public async Task<ResultBase<ShopGetAllConcessionsResult>> ExecuteAsync(ShopGetAllConcessionsArguments arguments)
        {
            var now = DateTime.UtcNow;

			var users = (await ServiceUserRepository.GetAsync());

            var result = (await Repository.GetAsync())
                .Where(x =>
                    x.Concession.State == ConcessionState.Active &&
                    x.Events.Count() >= 1
                )
				.Select(x => new
				{
					x.Id,
					x.Concession.Name,
					PhotoUrl = users
						.Where(y => y.Login == x.Concession.Supplier.Login)
						.Select(y => y.Photo)
						.FirstOrDefault(),
					EventImages = x.Events
						.SelectMany(y => y.EventImages
							.Select(z => new ShopGetAllConcessionsResult_EventImages
							{
								PhotoUrl = z.PhotoUrl
							})
						)
				})
				.ToList()
				.Select(x => new ShopGetAllConcessionsResult
				{
					Id = x.Id,
					Name = x.Name,
					PhotoUrl = x.PhotoUrl,
					EventImages = x.EventImages
				});

            return new ResultBase<ShopGetAllConcessionsResult> { Data = result };
        }
        #endregion ExecuteAsync
    }
}
