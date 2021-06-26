using Microsoft.Practices.Unity;
using PayIn.Application.Dto.JustMoney.Arguments;
using PayIn.Application.Dto.JustMoney.Results;
using PayIn.Application.JustMoney.Services;
using PayIn.BusinessLogic.Common;
using PayIn.Domain.JustMoney;
using PayIn.Domain.JustMoney.Enums;
using PayIn.Infrastructure.JustMoney.Enums;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xp.Application.Dto;
using Xp.Domain;

namespace PayIn.Application.JustMoney.Handlers
{
	public class JustMoneyApiPrepaidCardGetEnableDisableHandler : IQueryBaseHandler<JustMoneyApiPrepaidCardGetEnableDisableArguments, JustMoneyApiPrepaidCardGetEnableDisableResult>
	{
		[Dependency] public ISessionData SessionData { get; set; }
		[Dependency] public IEntityRepository<PrepaidCard> PrepaidCardRepository { get; set; }
		[Dependency] public JustMoneyApiPrepaidCardGetAllHandler JustMoneyApiUserGetAllHandler { get; set; }
		[Dependency] public PfsService PfsService { get; set; }

		#region ExecuteAsync
		public async Task<ResultBase<JustMoneyApiPrepaidCardGetEnableDisableResult>> ExecuteAsync(JustMoneyApiPrepaidCardGetEnableDisableArguments arguments)
		{
			var cards = (await PrepaidCardRepository.GetAsync())
				.Where(x =>
					x.State == PrepaidCardState.Active &&
					x.Login == SessionData.Login
				)
				.ToList();

			var result = new List<JustMoneyApiPrepaidCardGetEnableDisableResult>();
			foreach (var card in cards)
			{
				// Get state
				var cardInquiry = await PfsService.CardInquiryAsync(card.CardHolderId);

				// Return state
				result.Add(new JustMoneyApiPrepaidCardGetEnableDisableResult
				{
					Id = card.Id,
					Alias = card.Alias,
					Enable =
						cardInquiry.CardInfo.CardStatus == CardStatus.Open ? true :
						cardInquiry.CardInfo.CardStatus == CardStatus.DepositOnly ? false :
						(bool?)null
				});
			}

			return new ResultBase<JustMoneyApiPrepaidCardGetEnableDisableResult>
			{
				Data = result
			};
		}
		#endregion ExecuteAsync
	}
}
