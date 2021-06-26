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
	public class JustMoneyMobilePrepaidCardEnableHandler : IServiceBaseHandler<JustMoneyMobilePrepaidCardEnableArguments>
	{
		[Dependency] public ISessionData SessionData { get; set; }
		[Dependency] public IEntityRepository<PrepaidCard> PrepaidCardRepository { get; set; }
		[Dependency] public PfsService PfsService { get; set; }

		#region ExecuteAsync
		public async Task<dynamic> ExecuteAsync(JustMoneyMobilePrepaidCardEnableArguments arguments)
		{
			var item = (await PrepaidCardRepository.GetAsync())
				.Where(x =>
					x.Login == SessionData.Login &&
					x.Id == arguments.Id
				)
				.FirstOrDefault();
			if (item == null)
				throw new ArgumentNullException(nameof(arguments.Id));

			// Get state
			var cardInquiry = await PfsService.CardInquiryAsync(item.CardHolderId);
			if (
				(cardInquiry.CardInfo.CardStatus != CardStatus.Issued) &&
				(cardInquiry.CardInfo.CardStatus != CardStatus.DepositOnly)
			)
				throw new ApplicationException("Solo se pueden activar tarjetas bloqueadas o pendientes de inicialización");

			// Change state
			var result = await PfsService.ChangeCardStatusAsync(item.CardHolderId, cardInquiry.CardInfo.CardStatus, CardStatus.Open);

			return null;
		}
		#endregion ExecuteAsync
	}
}
