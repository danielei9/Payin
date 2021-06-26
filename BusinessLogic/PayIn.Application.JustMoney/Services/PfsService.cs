using Microsoft.Practices.Unity;
using PayIn.Infrastructure.JustMoney.Enums;
using PayIn.Infrastructure.JustMoney.Results;
using PayIn.Infrastructure.JustMoney.Server;
using System;
using System.Threading.Tasks;

namespace PayIn.Application.JustMoney.Services
{
	public class PfsService
	{
		[Dependency] public PfsServer PfsServer { get; set; }

		public string SuccessUrl = "https://enpxd8nsgyioi.x.pipedream.net";
		public string NonSuccessUrl = "";

		#region CallServer
		public async Task<T> CallServer<T>(Func<Task<T>> call)
			where T : class
		{
			for (var i = 0; i <= 5; i++)
			{
				try
				{
					var result = await call();
					return result;
				}
				catch (Exception e)
				{
					if ((i == 20) || (e.Message != "MessageID is already used"))
						throw e;
				}
			}

			return null;
		}
		#endregion CallServer

		#region CardIssueAsync
		public async Task<PfsServiceCardIssueResult> CardIssueAsync(
			string firstName, string lastName, DateTime birthDay,
			string address1, string address2, string zipCode, string city, string province, JustMoneyCountryEnum country,
			string phone, string mobile, string email
		)
		{
			var result = await CallServer(async () =>
			{
				var resultCall = await PfsServer.CardIssueAsync(
					CardStyle.OrderOnline, "",
					firstName, lastName, birthDay,
					address1, address2, zipCode, city, province, country,
					phone, mobile, email
				);
				return resultCall;
			});

			return result;
		}
		#endregion CardIssueAsync

		#region UpdateCardAsync
		public async Task<PfsServiceUpdateCardResult> UpdateCardAsync(
			string cardHolderId,
			string firstName, string lastName, DateTime birthDay,
			string address1, string address2, string zipCode, string city, string province, JustMoneyCountryEnum country,
			string phone, string mobile, string email
		)
		{
			var result = await CallServer(async () =>
			{
				var resultCall = await PfsServer.UpdateCardAsync(
					cardHolderId, CardStyle.OrderOnline,
					firstName, lastName, birthDay,
					address1, address2, zipCode, city, province, country,
					phone, mobile, email
				);
				return resultCall;
			});

			return result;
		}
		#endregion UpdateCardAsync

		#region CardInquiryAsync
		public async Task<PfsServiceCardInquiryResult> CardInquiryAsync(string cardHolderId)
		{
			var result = await CallServer(async () =>
			{
				var resultCall = await PfsServer.CardInquiryAsync(cardHolderId);
				return resultCall;
			});

			return result;
		}
		#endregion CardInquiryAsync

		#region UpgradeAsync
		public async Task<PfsServiceUpgradeCardResult> UpgradeAsync(string cardHolderId, CardType cardType)
		{
			var result = await CallServer(async () =>
			{
				var resultCall = await PfsServer.UpgradeCardAsync(cardHolderId, cardType);
				return resultCall;
			});

			return result;
		}
		#endregion UpgradeAsync

		#region ViewStatementAsync
		public async Task<PfsServiceViewStatementResult> ViewStatementAsync(string cardHolderId, int sinceYear, int sinceMonth, int sinceDay, int untilYear, int untilMonth, int untilDay)
		{
			var result = await CallServer(async () =>
			{
				var resultCall = await PfsServer.ViewStatementAsync(cardHolderId, sinceYear, sinceMonth, sinceDay, untilYear, untilMonth, untilDay);
				return resultCall;
			});

			return result;
		}
		#endregion ViewStatementAsync

		#region ChangeCardStatusAsync
		public async Task<PfsServiceChangeCardStatusResult> ChangeCardStatusAsync(string cardHolderId, CardStatus oldStatus, CardStatus newStatus)
		{
			var result = await CallServer(async () =>
			{
				var resultCall = await PfsServer.ChangeCardStatusAsync(cardHolderId, oldStatus, newStatus);
				return resultCall;
			});

			return result;
		}
		#endregion ChangeCardStatusAsync

		#region CardToCardAsync
		public async Task<PfsServiceCardToCardResult> CardToCardAsync(string cardHolderIdSource, string cardHolderIdTarget, decimal amount)
		{
			var result = await CallServer(async () =>
			{
				var cardInquiryResult = await CardInquiryAsync(cardHolderIdSource);

				var resultCall = await PfsServer.CardToCardAsync(
					cardHolderIdSource,
					cardHolderIdTarget,
					amount,
					JustMoneyCurrencyCode.EUR, // currencyCode
					cardInquiryResult.CardHolder.FirstName + " " + cardInquiryResult.CardHolder.LastName,
					cardInquiryResult.CardHolder.Address1 + " " + cardInquiryResult.CardHolder.Address2,
					cardInquiryResult.CardHolder.City,
					cardInquiryResult.CardHolder.State,
					"", //terminalId,
					cardInquiryResult.CardHolder.CountryCode,
					JustMoneyCurrencyCode.EUR //settlementCurrencyCode
				);

				return resultCall;
			});

			return result;
		}
		#endregion CardToCardAsync

		#region RegisterPayByToken
		public async Task<PfsServiceRegisterPayByTokenResult> RegisterPayByToken(string cardHolderId, decimal amount)
		{
			var result = await CallServer(async () =>
			{
				var cardInquiryResult = await CardInquiryAsync(cardHolderId);

				var resultCall = await PfsServer.RegisterPayByToken(cardHolderId,
					cardInquiryResult.CardHolder.FirstName,
					cardInquiryResult.CardHolder.MiddleInitial,
					cardInquiryResult.CardHolder.LastName,
					cardInquiryResult.CardHolder.Address1,
					cardInquiryResult.CardHolder.Address2,
					cardInquiryResult.CardHolder.ZipCode,
					cardInquiryResult.CardHolder.City,
					cardInquiryResult.CardHolder.State,
					cardInquiryResult.CardHolder.CountryCode,
					amount,
					SuccessUrl,
					NonSuccessUrl,
					"", // Currency
					cardHolderId // Payload
				);
				
				return resultCall;
			});

			return result;
		}
		#endregion RegisterPayByToken

		#region DepositToCard
		public async Task<PfsServiceDepositToCardResult> DepositToCard(string cardHolderId, decimal amount, string transactionDescription, string terminalLocation, string directFee)
		{
			var result = await CallServer(async () =>
			{
				var resultCall = await PfsServer.DepositToCard(cardHolderId,
					amount,
					transactionDescription,
					terminalLocation,
					directFee,
					JustMoneyCurrencyCode.EUR.ToEnumAlias(), JustMoneyCurrencyCode.EUR.ToEnumAlias() // Currency
				);

				return resultCall;
			});

			return result;
		}
		#endregion DepositToCard
	}
}
