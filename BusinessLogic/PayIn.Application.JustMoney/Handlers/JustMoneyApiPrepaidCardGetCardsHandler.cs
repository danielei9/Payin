using Microsoft.Practices.Unity;
using PayIn.Application.Dto.JustMoney.Arguments;
using PayIn.Application.Dto.JustMoney.Results;
using PayIn.BusinessLogic.Common;
using PayIn.Domain.JustMoney;
using PayIn.Domain.JustMoney.Enums;
using System.Linq;
using System.Threading.Tasks;
using Xp.Application.Dto;
using Xp.Domain;

namespace PayIn.Application.JustMoney.Handlers
{
	public class JustMoneyApiPrepaidCardGetCardsHandler : IQueryBaseHandler<JustMoneyApiPrepaidCardGetCardsArguments, JustMoneyApiPrepaidCardGetCardsResult>
	{
		[Dependency] public IEntityRepository<PrepaidCard> PrepaidCardRepository { get; set; }
		[Dependency] public ISessionData SessionData { get; set; }

        #region ExecuteAsync
        public async Task<ResultBase<JustMoneyApiPrepaidCardGetCardsResult>> ExecuteAsync(JustMoneyApiPrepaidCardGetCardsArguments arguments)
		{
			var result = (await PrepaidCardRepository.GetAsync())
				.Where(x =>
					x.State == PrepaidCardState.Active &&
					x.Login == SessionData.Login
				)
				.Select(x => new JustMoneyApiPrepaidCardGetCardsResult
				{
					Id = x.Id,
					CardHolderId = x.CardHolderId,
					Alias = x.Alias
				});

            return new ResultBase<JustMoneyApiPrepaidCardGetCardsResult>
            {
				Data = result
			};
		}
		#endregion ExecuteAsync
	}
}
