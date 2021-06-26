using Microsoft.Practices.Unity;
using PayIn.Application.Dto.JustMoney.Arguments;
using PayIn.Application.Dto.JustMoney.Results;
using PayIn.Application.JustMoney.Services;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xp.Application.Dto;

namespace PayIn.Application.JustMoney.Handlers
{
	public class JustMoneyApiPrepaidCardGetAllHandler : IQueryBaseHandler<JustMoneyApiPrepaidCardGetAllArguments, JustMoneyApiPrepaidCardGetAllResult>
	{
		[Dependency] public JustMoneyApiPrepaidCardGetCardsHandler JustMoneyApiPrepaidCardGetCardsHandler { get; set; }
		[Dependency] public PfsService PfsService { get; set; }
		
		#region ExecuteAsync
		public async Task<ResultBase<JustMoneyApiPrepaidCardGetAllResult>> ExecuteAsync(JustMoneyApiPrepaidCardGetAllArguments arguments)
		{
			var cards = (await JustMoneyApiPrepaidCardGetCardsHandler.ExecuteAsync(new JustMoneyApiPrepaidCardGetCardsArguments("")))
				.Data
				.ToList();

			var result = new List<JustMoneyApiPrepaidCardGetAllResult>();
			foreach (var item in cards)
			{
				var resultPfs = await PfsService.CardInquiryAsync(item.CardHolderId);

				var resultItem = new JustMoneyApiPrepaidCardGetAllResult
				{
					Id = item.Id,
					Alias = item.Alias,
					AvailableBalance = resultPfs.CardInfo.AvailBal.ToString("#,##0.00"),
					CardHolderId = item.CardHolderId,
					EspirationCodeYear = resultPfs.CardInfo.EspirationCodeYear,
					EspirationCodeMonth = resultPfs.CardInfo.EspirationCodeMonth,
					CardStatus = resultPfs.CardInfo.CardStatus,
					CardType = resultPfs.CardHolder.CardType,
					Pan = resultPfs.CardHolder.CardNumber,
					FirstName = resultPfs.CardHolder.FirstName,
					LastName = resultPfs.CardHolder.LastName,
					Address1 = resultPfs.CardHolder.Address1,
					Address2 = resultPfs.CardHolder.Address2,
					ZipCode = resultPfs.CardHolder.ZipCode,
					City = resultPfs.CardHolder.City,
					Country = ((int)resultPfs.CardHolder.CountryCode).ToString(),
					Mobile = resultPfs.CardHolder.Phone,
					Phone = resultPfs.CardHolder.Phone2,
					Iban = resultPfs.CardInfo.Iban
				};
				result.Add(resultItem);
			}
			return new ResultBase<JustMoneyApiPrepaidCardGetAllResult>
			{
				Data = result
			};
		}
		#endregion ExecuteAsync
	}
}
