using Microsoft.Practices.Unity;
using PayIn.Application.Dto.Payments.Arguments;
using PayIn.Application.Dto.Payments.Results;
using PayIn.BusinessLogic.Common;
using PayIn.Domain.Payments;
using PayIn.Domain.Public;
using System;
using System.Linq;
using System.Threading.Tasks;
using Xp.Application.Dto;
using Xp.Domain;

namespace PayIn.Application.Payments.Handlers
{
	public class PaymentConcessionMobileGetServiceCardsByUidHandler : 
		IQueryBaseHandler<PaymentConcessionMobileGetServiceCardsByUidArguments, PaymentConcessionMobileGetServiceCardsByUidResult>
    {
        [Dependency] public IEntityRepository<PaymentConcession> Repository { get; set; }
        [Dependency] public IEntityRepository<SystemCardMember> SystemCardMemberRepository { get; set; }
        [Dependency] public IEntityRepository<Campaign> CampaignRepository { get; set; }
        [Dependency] public ISessionData SessionData { get; set; }

		#region ExecuteAsync
		public async Task<ResultBase<PaymentConcessionMobileGetServiceCardsByUidResult>> ExecuteAsync(PaymentConcessionMobileGetServiceCardsByUidArguments arguments)
		{
            var systemCardMembers = (await SystemCardMemberRepository.GetAsync());

            var items = (await Repository.GetAsync())
				.Where(x => x.Concession.Supplier.Login == SessionData.Login);

			var promotions = (await CampaignRepository.GetAsync())
				.Where(x =>
					x.State == Common.CampaignState.Active
				);

			var result = items
				.Where(x =>
					x.Concession.State == Common.ConcessionState.Active)
				.SelectMany(x =>
                    //x.Concession.Members
                    systemCardMembers
                    .SelectMany(y => y.SystemCard.Cards
						.Where(z =>
                            y.Login == x.Concession.Supplier.Login &&
                            z.State == Common.ServiceCardState.Active &&
                            z.Uid == arguments.Uid
                        )
						.Select(z => new PaymentConcessionMobileGetServiceCardsByUidResult
						{
							OwnerUserName = z.OwnerUser.Name,
							OwnerUserVatNumber = z.OwnerUser.VatNumber,
							OwnerUserPhoto = z.OwnerUser.Photo,
							Concessions = z.Users
								.Where(a => a.State == Common.ServiceUserState.Active)
								.Select(a => new PaymentConcessionMobileGetServiceCardsByUidResult_Concession
								{
									Id = a.ConcessionId,
									VatNumber = a.Concession.Supplier.TaxNumber,
									Name = a.Concession.Supplier.Name,
									Address = a.Concession.Supplier.TaxAddress,
									//Phone = "961234567",
									Email = a.Concession.Supplier.Login,
									//Image = "http://www.hotelsailing.com/wp-content/uploads/2017/01/pasqua_000027-1024x768.jpg"
								}),
							Promotions = promotions
								.Where(a => a.State == Common.CampaignState.Active)
								.Select(a => new PaymentConcessionMobileGetServiceCardsByUidResult_Promotion {
									Id = a.Id,
									Description = a.Description,
									Title = a.Title,
									Image = a.PhotoUrl,
									Since = a.Since,
									Until = a.Until
								})
						})
					)
				);

			return new ResultBase<PaymentConcessionMobileGetServiceCardsByUidResult> { Data = result };
		}
		#endregion ExecuteAsync
	}
}
