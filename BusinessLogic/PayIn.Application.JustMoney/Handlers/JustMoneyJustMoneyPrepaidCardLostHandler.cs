using Microsoft.Practices.Unity;
using PayIn.Application.Dto.JustMoney.Arguments;
using PayIn.Application.JustMoney.Services;
using PayIn.BusinessLogic.Common;
using PayIn.Domain.JustMoney;
using PayIn.Infrastructure.JustMoney.Enums;
using System;
using System.Linq;
using System.Threading.Tasks;
using Xp.Application.Dto;
using Xp.Domain;

namespace PayIn.Application.JustMoney.Handlers
{
	public class JustMoneyJustMoneyPrepaidCardLostHandler : IServiceBaseHandler<JustMoneyJustMoneyPrepaidCardLostArguments>
	{
		[Dependency] public ISessionData SessionData { get; set; }
		[Dependency] public PfsService PfsService { get; set; }
		[Dependency] public IEntityRepository<PrepaidCard> PrepaidCardRepository { get; set; }

		#region ExecuteAsync
		public async Task<dynamic> ExecuteAsync(JustMoneyJustMoneyPrepaidCardLostArguments arguments)
		{
			var item = (await PrepaidCardRepository.GetAsync())
				.Where(x =>
					x.Login == SessionData.Login
				)
				.FirstOrDefault();
			if (item == null)
				return null;

			// Get state
			var cardInquiry = await PfsService.CardInquiryAsync(item.CardHolderId);
			if (
				(cardInquiry.CardInfo.CardStatus != CardStatus.Open)
			)
				throw new ApplicationException("Solo se pueden perder tarjetas activas");
			
			// Change state
			await PfsService.ChangeCardStatusAsync(item.CardHolderId, cardInquiry.CardInfo.CardStatus, CardStatus.Lost);

			return item;
		}
		#endregion ExecuteAsync
	}
}
