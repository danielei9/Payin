using Microsoft.Practices.Unity;
using PayIn.Application.Dto.JustMoney.Arguments;
using PayIn.Application.JustMoney.Services;
using PayIn.BusinessLogic.Common;
using PayIn.Domain.JustMoney;
using PayIn.Domain.JustMoney.Enums;
using PayIn.Infrastructure.Security;
using System;
using System.Threading.Tasks;
using Xp.Application.Dto;
using Xp.Domain;

namespace PayIn.Application.JustMoney.Handlers
{
	public class JustMoneyApiPrepaidCardRechargeCardHandler : IServiceBaseHandler<JustMoneyApiPrepaidCardRechargeCardArguments>
	{
		[Dependency] public IEntityRepository<PrepaidCard> PrepaidCardRepository { get; set; }
		[Dependency] public PfsService PfsService { get; set; }
		[Dependency] public SessionData SessionData { get; set; }
		[Dependency] public SecurityRepository SecurityRepository { get; set; }
		[Dependency] public IEntityRepository<BankCardTransaction> BankCardRepository { get; set; }
		
		#region ExecuteAsync
		public async Task<dynamic> ExecuteAsync(JustMoneyApiPrepaidCardRechargeCardArguments arguments)
		{
			var now = DateTime.UtcNow;

			var user = await SecurityRepository.GetUserAsync(SessionData.Login, arguments.Password);
			if (user == null)
				throw new ApplicationException("Password is not correct");

			var card = (await PrepaidCardRepository.GetAsync(arguments.Id));
			if (card == null)
				throw new ArgumentNullException("Id");

			var redirectUrl = await RechargeAsync(
				card.CardHolderId, arguments.Amount,
				now
			);

			return new
			{
				RedirectUrl = redirectUrl
			};
		}
		#endregion ExecuteAsync

		#region RechargeAsync
		public async Task<string> RechargeAsync(string cardHolderId, decimal amount, DateTime now)
		{
			var result = await PfsService.RegisterPayByToken(cardHolderId, amount);
			
			var item = new BankCardTransaction
			{
				CreatedAt = now,
				State = PrepaidCardState.Creating,
				Login = SessionData.Login,
				RegistrationMessageId = result.MessageId,
				CardHolderId = cardHolderId,
				Amount = amount
			};
			await BankCardRepository.AddAsync(item);

			return result.RedirectUrl;
        }
		#endregion RechargeAsync
	}
}
