using Microsoft.Practices.Unity;
using PayIn.Application.Dto.JustMoney.Arguments;
using PayIn.Application.JustMoney.Services;
using PayIn.BusinessLogic.Common;
using PayIn.Domain.JustMoney;
using PayIn.Infrastructure.JustMoney.Enums;
using System.Linq;
using System.Threading.Tasks;
using Xp.Application.Dto;
using Xp.Domain;

namespace PayIn.Application.JustMoney.Handlers
{
	public class JustMoneyApiPrepaidCardEnableDisableHandler : IServiceBaseHandler<JustMoneyApiPrepaidCardEnableDisableArguments>
	{
		[Dependency] public ISessionData SessionData { get; set; }
		[Dependency] public IEntityRepository<PrepaidCard> PrepaidCardRepository { get; set; }
		[Dependency] public PfsService PfsService { get; set; }

		#region ExecuteAsync
		public async Task<dynamic> ExecuteAsync(JustMoneyApiPrepaidCardEnableDisableArguments arguments)
		{
			var ids = arguments.Cards
				.Select(x => x.Id)
				.ToList();
			var cards = (await PrepaidCardRepository.GetAsync())
				.Where(x =>
					x.Login == SessionData.Login &&
					ids.Contains(x.Id)
				)
				.ToList();

			foreach (var card in cards)
			{
				var newCardState = arguments.Cards
					.Where(x => x.Id == card.Id)
					.Select(x => (bool?)x.Enable)
					.FirstOrDefault();

				// Change state
				if (newCardState == null)
					continue;
				var newCardStateValue = newCardState == true ? CardStatus.Open : CardStatus.DepositOnly;

				// Get state
				var cardInquiry = await PfsService.CardInquiryAsync(card.CardHolderId);
				if (cardInquiry.CardInfo.CardStatus == newCardStateValue)
					continue;

				await PfsService.ChangeCardStatusAsync(
					card.CardHolderId,
					cardInquiry.CardInfo.CardStatus,
					newCardStateValue
				);
			}

			return null;
		}
		#endregion ExecuteAsync
	}
}
