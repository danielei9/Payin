using Microsoft.Practices.Unity;
using PayIn.Application.Dto.JustMoney.Arguments;
using PayIn.Application.Dto.JustMoney.Results;
using PayIn.Application.JustMoney.Services;
using PayIn.BusinessLogic.Common;
using PayIn.Domain.JustMoney;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xp.Application.Dto;
using Xp.Domain;

namespace PayIn.Application.JustMoney.Handlers
{
	public class JustMoneyJustMoneyPrepaidCardGetAllHandler : IQueryBaseHandler<JustMoneyJustMoneyPrepaidCardGetAllArguments, JustMoneyJustMoneyPrepaidCardGetAllResult>
	{
		[Dependency] public IEntityRepository<PrepaidCard> PrepaidCardRepository { get; set; }
		[Dependency] public PfsService PfsService { get; set; }
		[Dependency] public ISessionData SessionData { get; set; }

		#region ExecuteAsync
		public async Task<ResultBase<JustMoneyJustMoneyPrepaidCardGetAllResult>> ExecuteAsync(JustMoneyJustMoneyPrepaidCardGetAllArguments arguments)
		{
			var list = (await PrepaidCardRepository.GetAsync())
				.Where(x =>
					x.Login == SessionData.Login
				)
				.Select(x => new
				{
					x.Id,
					x.Login,
					x.CardHolderId
				})
				.ToList();

			var result = new List<JustMoneyJustMoneyPrepaidCardGetAllResult>();
			foreach (var item in list)
			{
				var resultPfs = await PfsService.CardInquiryAsync(item.CardHolderId);

				var resultItem = new JustMoneyJustMoneyPrepaidCardGetAllResult
				{
					Id = item.Id,
					Login = item.Login,
					Balance = resultPfs.CardInfo.AvailBal,
					EspirationCodeYear = resultPfs.CardInfo.EspirationCodeYear,
					EspirationCodeMonth = resultPfs.CardInfo.EspirationCodeMonth,
					CardStatus = resultPfs.CardInfo.CardStatus,
                    CardName = (resultPfs.CardHolder.FirstName + " " + resultPfs.CardHolder.LastName).Trim(),
                    Pan = resultPfs.CardHolder.CardNumber,
					FirstName = resultPfs.CardHolder.FirstName,
					LastName = resultPfs.CardHolder.LastName,
					BirthDay = resultPfs.CardHolder.Dob,
					Address1 = resultPfs.CardHolder.Address1,
					Address2 = resultPfs.CardHolder.Address2,
					ZipCode = resultPfs.CardHolder.ZipCode,
					City = resultPfs.CardHolder.City,
					Province = resultPfs.CardHolder.State,
					Country = ((int)resultPfs.CardHolder.CountryCode).ToString(),
					Mobile = resultPfs.CardHolder.Phone,
					Phone = resultPfs.CardHolder.Phone2
				};
				result.Add(resultItem);
			}
			
			return new ResultBase<JustMoneyJustMoneyPrepaidCardGetAllResult>
			{
				Data = result
			};
		}
		#endregion ExecuteAsync
	}
}
