using Microsoft.Practices.Unity;
using PayIn.Application.Dto.JustMoney.Arguments;
using PayIn.BusinessLogic.Common;
using PayIn.Domain.JustMoney;
using System;
using System.Linq;
using System.Threading.Tasks;
using Xp.Application.Dto;
using Xp.Domain;

namespace PayIn.Application.JustMoney.Handlers
{
	public class JustMoneyApiPrepaidCardPutDetailsHandler : IServiceBaseHandler<JustMoneyApiPrepaidCardPutDetailsArguments>
	{
		[Dependency] public IEntityRepository<PrepaidCard> PrepaidCardRepository { get; set; }
		[Dependency] public ISessionData SessionData { get; set; }

		#region ExecuteAsync
		public async Task<dynamic> ExecuteAsync(JustMoneyApiPrepaidCardPutDetailsArguments arguments)
        {
			if (arguments.Cards == null)
				throw new ArgumentNullException("Cards");

			var ids = arguments.Cards
				.Select(x => x.Id)
				.ToList();

			var cards = (await PrepaidCardRepository.GetAsync())
				.Where(x =>
					x.Login == SessionData.Login &&
					ids.Contains(x.Id)
				);

			foreach(var card in cards)
			{
				card.Alias = arguments.Cards
					.Where(x => x.Id == card.Id)
					.Select(x => x.Alias)
					.FirstOrDefault();
			}

			return null;
        }
        #endregion ExecuteAsync
    }
}
