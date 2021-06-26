using Microsoft.Practices.Unity;
using PayIn.Application.Dto.JustMoney.Arguments;
using PayIn.BusinessLogic.Common;
using PayIn.Domain.JustMoney;
using PayIn.Infrastructure.JustMoney.Enums;
using PayIn.Infrastructure.JustMoney.Results;
using PayIn.Infrastructure.JustMoney.Services;
using System;
using System.Linq;
using System.Threading.Tasks;
using Xp.Application.Dto;
using Xp.Domain;

namespace PayIn.Application.JustMoney.Handlers
{
	public class JustMoneyJustMoneyPrepaidCardUpdateHandler : IServiceBaseHandler<JustMoneyJustMoneyPrepaidCardUpdateArguments>
	{
		[Dependency] public ISessionData SessionData { get; set; }
		[Dependency] public IEntityRepository<Option> OptionRepository { get; set; }
		[Dependency] public IEntityRepository<PrepaidCard> PrepaidCardRepository { get; set; }
		[Dependency] public PfsService PfsService { get; set; }
		[Dependency] public IUnitOfWork UnitOfWork { get; set; }

		#region ExecuteAsync
		public async Task<dynamic> ExecuteAsync(JustMoneyJustMoneyPrepaidCardUpdateArguments arguments)
		{
			var item = (await PrepaidCardRepository.GetAsync())
				.Where(x =>
					x.Login == SessionData.Login
				)
				.FirstOrDefault() ??
				throw new ArgumentNullException();

			var result = await UpdateCardAsync(
				item.CardHolderId,
				arguments.FirstName, arguments.LastName, arguments.BirthDay,
				arguments.Address1, arguments.Address2, arguments.ZipCode, arguments.City, arguments.Province, arguments.Country,
				arguments.Phone, arguments.Mobile, SessionData.Email
			);

			item.FirstName = arguments.FirstName;
			item.LastName = arguments.LastName;

			return item;
		}
		#endregion ExecuteAsync

		#region UpdateCardAsync
		public async Task<PfsServiceUpdateCardResult> UpdateCardAsync(
			string cardHolderId,
			string firstName, string lastName, DateTime birthDay,
			string address1, string address2, string zipCode, string city, string province, JustMoneyCountryEnum country,
			string phone, string mobile, string email
		)
		{
			for (var i = 0; i <= 20; i++)
			{
				try
				{
					var option = (await OptionRepository.GetAsync(1));

					var messageId = int.Parse(option.Value) + 1;
					option.Value = messageId.ToString();
					await UnitOfWork.SaveAsync();

					var result = await PfsService.UpdateCardAsync(
						messageId, cardHolderId, CardStyle.OrderOnline,
						firstName, lastName, birthDay,
						address1, address2, zipCode, city, province, country,
						phone, mobile, email
					);
					return result;
				}
				catch (Exception e)
				{
					if ((i == 20) || (e.Message != "MessageID is already used"))
						throw e;
				}
			}

			return null;
		}
		#endregion UpdateCardAsync
	}
}
