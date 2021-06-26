using PayIn.Application.Dto.Payments.Arguments;
using PayIn.Application.Dto.Payments.Results;
using PayIn.BusinessLogic.Common;
using PayIn.Common;
using PayIn.Domain.Payments;
using PayIn.Domain.Public;
using System;
using System.Linq;
using System.Threading.Tasks;
using Xp.Application.Attributes;
using Xp.Application.Dto;
using Xp.Domain;

namespace PayIn.Application.Payments.Handlers
{
	[XpLog("PaymentConcession", "MobileGet")]
	public class PaymentConcessionMobileGetHandler :
		IQueryBaseHandler<PaymentConcessionMobileGetArguments, PaymentConcessionMobileGetResult>
	{
		private readonly IEntityRepository<PaymentConcession> Repository;
		private readonly IEntityRepository<ServiceUser> ServiceUserRepository;
		private readonly ISessionData SessionData;

		#region Constructors
		public PaymentConcessionMobileGetHandler(
			IEntityRepository<PaymentConcession> repository,
			IEntityRepository<ServiceUser> serviceUserRepository,
			ISessionData sessionData
		)
		{
			if (repository == null) throw new ArgumentNullException(nameof(repository));
			if (serviceUserRepository == null) throw new ArgumentNullException(nameof(serviceUserRepository));
			if (sessionData == null) throw new ArgumentNullException(nameof(sessionData));

			Repository = repository;
			ServiceUserRepository = serviceUserRepository;
			SessionData = sessionData;
		}
		#endregion Constructors

		#region ExecuteAsync
		public async Task<ResultBase<PaymentConcessionMobileGetResult>> ExecuteAsync(PaymentConcessionMobileGetArguments arguments)
		{
			var now = DateTime.UtcNow;

			var users = (await ServiceUserRepository.GetAsync());

			var items = (await Repository.GetAsync())
				.Where(x =>
					x.Id == arguments.Id &&
					x.Concession.State == ConcessionState.Active
				)
				.Select(x => new {
					Id = x.Id,
					Name = x.Concession.Name,
					Address = x.Address,
					Phone = x.Phone,
					Email = x.Concession.Supplier.Login,
					PhotoUrl = users
						.Where(y => y.Login == x.Concession.Supplier.Login)
						.Select(y => y.Photo)
						.FirstOrDefault(),
					Products = (
						x.Products
							.Where(y =>
								y.State == ProductState.Active &&
								y.IsVisible
							)
							.Select(y => new PaymentConcessionMobileGetResult_Product
							{
								Id = y.Id,
								Description = y.Description,
								FamilyId = y.FamilyId,
								PhotoUrl = y.PhotoUrl,
								Name = y.Name,
								Price = y.Price,
								Type = PaymentConcessionMobileGetResult_ProductTypeEnum.Product
							})
					).Union(
						x.Families
							.Where(y =>
								y.State == ProductFamilyState.Active &&
								y.IsVisible
							)
							.Select(y => new PaymentConcessionMobileGetResult_Product
							{
								Id = y.Id,
								Description = y.Description,
								FamilyId = y.SuperFamilyId,
								PhotoUrl = y.PhotoUrl,
								Name = y.Name,
								Price = null, //y.Price,
								Type = PaymentConcessionMobileGetResult_ProductTypeEnum.Family
							})
					),
					Events = x.Events
						.Where(y =>
							(y.IsVisible) &&
							(y.EventEnd > now) &&
							(y.State == EventState.Active) &&
							(
								(y.Visibility != EventVisibility.Members) ||
								(x.Concession.ServiceUsers
									.Where(z => z.Login == SessionData.Login)
									.Any()
								)
							)
						)
						.OrderBy(y => y.EventStart)
						.Select(y => new
						{
							Id = y.Id,
							Name = y.Name,
							Place = y.Place,
							PhotoUrl = y.PhotoUrl,
							Description = y.Description,
							EventStart = y.EventStart,
							EventEnd = y.EventEnd,
							MinPrice = y.EntranceTypes
								.Select(z => (decimal?)z.Price)
								.Min() ?? 0,
							MaxPrice = y.EntranceTypes
								.Select(z => (decimal?)z.Price)
								.Max() ?? 0
						}),
					Promotions = x.Campaigns
						.Where(y => y.State == CampaignState.Active)
						.Select(y => new
						{
							Id = y.Id,
							Title = y.Title,
							Since = y.Since,
							Until = y.Until,
							PhotoUrl = y.PhotoUrl,
							Price = (decimal?) null //y.Price
						})
				})
				.ToList()
				.Select(x => new PaymentConcessionMobileGetResult
				{
					Id = x.Id,
					Name = x.Name,
					Address = x.Address,
					Phone = x.Phone,
					Email = x.Email,
					PhotoUrl = x.PhotoUrl,
					Products = x.Products,
					Events = x.Events
						.Select(y => new PaymentConcessionMobileGetResult_Event
						{
							Id = y.Id,
							Name = y.Name,
							Place = y.Place,
							PhotoUrl = y.PhotoUrl,
							Description = y.Description,
							EventStart = y.EventStart.ToUTC(),
							EventEnd = y.EventEnd.ToUTC(),
							MinPrice = y.MinPrice,
							MaxPrice = y.MaxPrice
						}),
					Promotions = x.Promotions
						.Select(y => new PaymentConcessionMobileGetResult_Promotion
						{
							Id = y.Id,
							Title = y.Title,
							Since = y.Since.ToUTC(),
							Until = y.Until.ToUTC(),
							PhotoUrl = y.PhotoUrl,
							Price = y.Price
						})
				});

			return new ResultBase<PaymentConcessionMobileGetResult> {
				Data = items
			};
		}
        #endregion ExecuteAsync

        #region PayinConcession
        public async Task<IQueryable<PaymentConcession>> PayinConcession()
        {
            var payinPaymentConcessions = (await Repository.GetAsync())
                .Where(x =>
                    (x.Concession.State == ConcessionState.Active) &&
                    (x.Concession.Supplier.Login == "info@pay-in.es")
                );

            return payinPaymentConcessions;
        }
        #endregion PayinConcession
    }
}
