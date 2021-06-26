using Microsoft.Practices.Unity;
using PayIn.Application.Dto.JustMoney.Arguments;
using PayIn.Application.JustMoney.Services;
using PayIn.BusinessLogic.Common;
using PayIn.Domain.JustMoney;
using PayIn.Infrastructure.JustMoney.Enums;
using PayIn.Infrastructure.Security;
using System;
using System.Threading.Tasks;
using Xp.Application.Dto;
using Xp.Domain;

namespace PayIn.Application.JustMoney.Handlers
{
	public class JustMoneyApiPrepaidCardUpgradeHandler : IServiceBaseHandler<JustMoneyApiPrepaidCardUpgradeArguments>
	{
		[Dependency] public IEntityRepository<PrepaidCard> PrepaidCardRepository { get; set; }
		[Dependency] public PfsService PfsService { get; set; }
		[Dependency] public SessionData SessionData { get; set; }
		[Dependency] public SecurityRepository SecurityRepository { get; set; }

		#region ExecuteAsync
		public async Task<dynamic> ExecuteAsync(JustMoneyApiPrepaidCardUpgradeArguments arguments)
		{
			var now = DateTime.UtcNow;

			var user = await SecurityRepository.GetUserAsync(SessionData.Login, arguments.Password);
			if (user == null)
				throw new ApplicationException("Password is not correct");

			var card = (await PrepaidCardRepository.GetAsync(arguments.Id));
            if (card == null)
                throw new ArgumentNullException("Id");
			
			var result = await PfsService.UpgradeAsync(card.CardHolderId, CardType.Premium);

			return card;
        }
		#endregion ExecuteAsync
	}
}
