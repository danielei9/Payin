using Microsoft.Practices.Unity;
using PayIn.Application.Dto.Payments.Arguments;
using PayIn.Application.Dto.Payments.Results;
using PayIn.BusinessLogic.Common;
using PayIn.Common;
using PayIn.Domain.Payments;
using PayIn.Domain.Payments.Infrastructure;
using PayIn.Domain.Promotions;
using PayIn.Domain.Public;
using PayIn.Domain.Transport;
using System;
using System.Collections.Generic;
using System.Data.Entity.SqlServer;
using System.Linq;
using System.Threading.Tasks;
using Xp.Application.Dto;
using Xp.Domain;

namespace PayIn.Application.Payments.Handlers
{
	public class MobilePaymentMediaGetAllHandler :
		IQueryBaseHandler<MobilePaymentMediaGetAllArguments, MobilePaymentMediaGetAllResult>
	{
		[Dependency] public IEntityRepository<PaymentMedia> Repository { get; set; }
		[Dependency] public IInternalService InternalService { get; set; }
        [Dependency] public IEntityRepository<PromoExecution> ExecutionRepository { get; set; }
        [Dependency] public ISessionData SessionData { get; set; }
        [Dependency] public IEntityRepository<TransportPrice> PriceRepository { get; set; }
        [Dependency] public IEntityRepository<Entrance> EntranceRepository { get; set; }
        [Dependency] public IEntityRepository<ServiceCard> ServiceCardRepository { get; set; }
        [Dependency] public IEntityRepository<SystemCardMember> SystemCardMemberRepository { get; set; }
        [Dependency] public MobileEntranceGetAllHandler MobileEntranceGetAllHandler { get; set; }

        #region ExecuteAsync
        public async Task<ResultBase<MobilePaymentMediaGetAllResult>> ExecuteAsync(MobilePaymentMediaGetAllArguments arguments)
		{
			var now = DateTime.Now;
			var titles = (await PriceRepository.GetAsync("Title"));

			var promotions = (await ExecutionRepository.GetAsync("Promotion", "Promotion.Concession.Concession", "Promotion.PromoActions", "PromoUser", "Promotion.PromoLaunchers"))
				.Where(x =>
					x.AppliedDate == null &&
					x.PromoUser.Login == SessionData.Login &&
					//x.Promotion.StartDate <= now &&
					x.Promotion.EndDate >= now &&
					x.Promotion.State == Common.PromotionState.Active &&
					x.Promotion.PromoLaunchers
						.Where(y => y.Type == Common.PromoLauncherType.Instant)
						.Any()
				)
				.Select(x => new
				{
					x.Id,
					x.Promotion.Name,
					x.Promotion.EndDate,
					Concession = x.Promotion.Concession.Concession.Name,
					Actions = x.Promotion.PromoActions
						.Select(y => new MobilePaymentMediaGetAllResult_PromotionAction
						{
							Type = y.Type,
							Quantity = y.Quantity
						}),
					Prices = x.Promotion.PromoPrices
						.Select(y => new MobilePaymentMediaGetAllResult_PromotionPrice
						{
							Id = y.TransportPriceId,
							Code = y.TransportPrice.Title.Code,
							Zone = y.TransportPrice.Zone,
							HasZone = y.TransportPrice.Title.HasZone,
							Name = y.TransportPrice.Title.Name
						}),
				})
				.ToList()
				.Select(x => new MobilePaymentMediaGetAllResult_Promotion
				{
					Name = x.Name,
					EndDate = x.EndDate.ToUTC(),
					Id = x.Id,
					Concession = x.Concession,
					Actions = x.Actions,
					Prices = x.Prices
						.Select(y => new MobilePaymentMediaGetAllResult_PromotionPrice
						{
							Id = y.Id,
							Code = y.Code,
							Zone = y.Zone,
							HasZone = y.HasZone,
							Name = y.Name + (y.HasZone ? " " + y.Zone.ToEnumAlias("") : "")
						})
				});

			var items = (await GetPaymentMediasAsync(now , null))
				.ToList(); // Necesario porque sino al modificar el valor de Balance Linq no lo hace correctamente al usar otra copia.
			foreach (var pMedia in items)
			{
				var res = await InternalService.PaymentMediaGetBalanceAsync(pMedia.Id);
				if (res != null)
					pMedia.Balance = res.Balance;
			}
			var cards = items
				.Where(x => (x.Type == PaymentMediaType.WebCard)) //No sea monedero
				.Select(x => new MobilePaymentMediaGetAllResult
				{
					Id = x.Id,
					Title = x.Title,
					Subtitle = x.Type.ToString(),
					VisualOrder = x.VisualOrder,
					NumberHash = x.NumberHash,
					ExpirationMonth = x.ExpirationMonth,
					ExpirationYear = x.ExpirationYear,
					Type = x.Type,
					State = x.State,
					BankEntity = x.BankEntity,
					Balance = x.Balance,
					Image = x.Image
				});

			var purses = items
			.Where(x => (x.Type == PaymentMediaType.Purse) && x.Balance != 0) //Sea monedero y  con valor distinto de 0
			.Select(x => new MobilePaymentMediaGetAllResult_Purse
			{
				Id = x.Id,
				Title = x.Title,
				Subtitle = x.Type.ToString(),
				VisualOrder = x.VisualOrder,
				NumberHash = x.NumberHash,
				ExpirationMonth = x.ExpirationMonth,
				ExpirationYear = x.ExpirationYear,
				Type = x.Type,
				State = x.State,
				BankEntity = x.BankEntity,
				Balance = x.Balance,
				Image = x.Image
			});

			var userHasPayment = await InternalService.UserHasPaymentAsync();
			var entrances = await MobileEntranceGetAllHandler.ExecuteInternalAsync(now, SessionData.Login, null, null, null);
			var serviceCards = await GetServiceCardsAsync(now, SessionData.Login);

			return new MobilePaymentMediaGetAllResultBase
			{
				UserHasPayment = userHasPayment,
				Data = cards,
				Promotions = promotions,
				Entrances = entrances,
				Purses = purses,
				ServiceCards = serviceCards
			};
		}
		#endregion ExecuteAsync

		#region GetPaymentMediasAsync
		public async Task<IEnumerable<MobilePaymentMediaGetAllResult>> GetPaymentMediasAsync(DateTime now, int? paymentConcessionId)
		{
			return (await Repository.GetAsync())
				.Where(x =>
					x.Login == SessionData.Login &&
					x.State != PaymentMediaState.Error &&
					x.State != PaymentMediaState.Delete &&
					x.State != PaymentMediaState.Expired &&
					((x.Purse != null && x.Purse.Expiration != null) ? x.Purse.Expiration >= now : true) &&
					((x.Purse != null && x.Purse.Validity != null) ? x.Purse.Validity >= now : true) &&
					(
						(paymentConcessionId == null) ||
						(paymentConcessionId == x.PaymentConcessionId) ||
						(x.PaymentConcessionId == null)
					)
				)
				.Select(x => new
				{
					Id = x.Id,
					Title = x.Name,
					Subtitle = x.Type.ToString(),
					VisualOrder = x.VisualOrder,
					NumberHash = x.NumberHash,
					ExpirationMonth = x.ExpirationMonth,
					ExpirationYear = x.ExpirationYear,
					Type = x.Type,
					State = x.State,
					BankEntity = x.BankEntity,
					Image = x.Purse.Image,
					Login = x.Login,
					PaymentConcessionId = x.PaymentConcessionId
				})
				.Select(x => new MobilePaymentMediaGetAllResult
				{
					Id = x.Id,
					Title = x.Title,
					Subtitle = x.Type.ToString(),
					VisualOrder = x.VisualOrder,
					NumberHash = x.NumberHash,
					ExpirationMonth = x.ExpirationMonth,
					ExpirationYear = x.ExpirationYear,
					Type = x.Type,
					State = x.State,
					BankEntity = x.BankEntity,
					Image = x.Image,
					Login = x.Login,
					Balance = 0,
					PaymentConcessionId = x.PaymentConcessionId
				});
		}
		#endregion GetPaymentMediasAsync

		#region GetServiceCardsAsync
		public async Task<IEnumerable<MobilePaymentMediaGetAllResult_ServiceCard>> GetServiceCardsAsync(DateTime now, string login)
		{
			var result = (await ServiceCardRepository.GetAsync())
				.Where(x =>
					x.State == ServiceCardState.Active &&
					x.Users
						.Any(y =>
							(y.State == ServiceUserState.Active) &&
							(y.Login == login)
						)
				)
				.Select(x => new
				{
					x.Id,
					x.SystemCard.Name,
					x.Uid,
					x.SystemCard.PhotoUrl
				})
				.ToList()
				.Select(x => new MobilePaymentMediaGetAllResult_ServiceCard
				{
					Id = x.Id,
					Name = x.Name,
					PhotoUrl = x.PhotoUrl,
					Code = @"pay[in]/serviceCard:{{uid:{0}}}".FormatString(x.Uid),
					CodeText = x.Uid.ToHexadecimal(),
					CodeType = EntranceSystemType.QR
				});

			return result;
		}
		#endregion GetServiceCardsAsync
	}
}
