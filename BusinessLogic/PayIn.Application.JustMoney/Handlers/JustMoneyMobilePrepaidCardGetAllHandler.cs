using Microsoft.Practices.Unity;
using PayIn.Application.Dto.JustMoney.Arguments;
using PayIn.Application.Dto.JustMoney.Results;
using System.Linq;
using System.Threading.Tasks;
using Xp.Application.Dto;

namespace PayIn.Application.JustMoney.Handlers
{
	public class JustMoneyMobilePrepaidCardGetAllHandler : IQueryBaseHandler<JustMoneyMobilePrepaidCardGetAllArguments, JustMoneyMobilePrepaidCardGetAllResult>
	{
		[Dependency] public JustMoneyApiPrepaidCardGetCardsHandler JustMoneyApiPrepaidCardGetCards { get; set; }
		
		#region ExecuteAsync
		public async Task<ResultBase<JustMoneyMobilePrepaidCardGetAllResult>> ExecuteAsync(JustMoneyMobilePrepaidCardGetAllArguments arguments)
		{
			var result = (await JustMoneyApiPrepaidCardGetCards.ExecuteAsync(new JustMoneyApiPrepaidCardGetCardsArguments("")))
				.Data
				.ToList()
				.Select(x => new JustMoneyMobilePrepaidCardGetAllResult
				{
					Id = x.Id,
					Alias = x.Alias
				});

			return new ResultBase<JustMoneyMobilePrepaidCardGetAllResult>
			{
				Data = result
			};
		}
		#endregion ExecuteAsync
	}
}
