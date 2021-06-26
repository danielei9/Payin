using Microsoft.Practices.Unity;
using PayIn.Application.Dto.JustMoney.Arguments;
using PayIn.BusinessLogic.Common;
using PayIn.Domain.JustMoney;
using System.Linq;
using System.Threading.Tasks;
using Xp.Application.Dto;
using Xp.Domain;

namespace PayIn.Application.JustMoney.Handlers
{
	public class JustMoneyMobilePrepaidCardDetailsUpdateHandler : IServiceBaseHandler<JustMoneyMobilePrepaidCardPutDetailsArguments>
	{
		[Dependency] public IEntityRepository<PrepaidCard> PrepaidCardRepository { get; set; }
		[Dependency] public ISessionData SessionData { get; set; }

		#region ExecuteAsync
		public async Task<dynamic> ExecuteAsync(JustMoneyMobilePrepaidCardPutDetailsArguments arguments)
        {
			var cards = (await PrepaidCardRepository.GetAsync())
				.Where(x =>
					x.Login == SessionData.Login &&
					x.Id == arguments.Id
				);

			foreach(var card in cards)
				card.Alias = arguments.Alias;

			return null;
        }
        #endregion ExecuteAsync
    }
}
