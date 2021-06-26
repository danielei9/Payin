using Microsoft.Practices.Unity;
using PayIn.Application.Dto.JustMoney.Arguments;
using PayIn.Application.JustMoney.Services;
using PayIn.BusinessLogic.Common;
using PayIn.Domain.JustMoney;
using PayIn.Domain.JustMoney.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xp.Application.Dto;
using Xp.Domain;
using Xp.Infrastructure.Mail;

namespace PayIn.Application.JustMoney.Handlers
{
	public class JustMoneyApiPrepaidCardCreateCardHandler : IServiceBaseHandler<JustMoneyApiPrepaidCardCreateCardArguments>
	{
		[Dependency] public ISessionData SessionData { get; set; }
		[Dependency] public IEntityRepository<PrepaidCard> PrepaidCardRepository { get; set; }
		[Dependency] public PfsService PfsService { get; set; }
		[Dependency] public MailService MailService { get; set; }

		#region ExecuteAsync
		public async Task<dynamic> ExecuteAsync(JustMoneyApiPrepaidCardCreateCardArguments arguments)
		{
			var cardHolderId = (await PrepaidCardRepository.GetAsync())
				.Where(x =>
					x.Login == SessionData.Login &&
					x.Id == arguments.CardId &&
					x.State == PrepaidCardState.Active
				)
				.Select(x => x.CardHolderId)
				.FirstOrDefault();
			if (cardHolderId == null)
				throw new ArgumentNullException("CardId");

			var resultPfs = await PfsService.CardInquiryAsync(cardHolderId);

			var aliases = new List<string>();
			if (arguments.Alias1 != null && arguments.Alias1 != "")
				aliases.Add(arguments.Alias1);
			if (arguments.Alias2 != null && arguments.Alias2 != "")
				aliases.Add(arguments.Alias2);
			if (arguments.Alias3 != null && arguments.Alias3 != "")
				aliases.Add(arguments.Alias3);

			foreach (var alias in aliases)
			{
				// Create
				var resultCreate = await PfsService.CardIssueAsync(
					resultPfs.CardHolder.FirstName, resultPfs.CardHolder.LastName, resultPfs.CardHolder.Dob,
					arguments.Address1, arguments.Address2, arguments.ZipCode, arguments.City, arguments.Province, arguments.Country,
					resultPfs.CardHolder.Phone2, resultPfs.CardHolder.Phone, SessionData.Email
				);

				// Get
				var resultGet = await PfsService.CardInquiryAsync(resultCreate.CardHolderId);

				var item = new PrepaidCard
				{
					State = PrepaidCardState.Active,
					Login = SessionData.Login,
					Alias = alias,
					CardHolderId = resultCreate.CardHolderId,
					Pan = resultGet.CardHolder.CardNumber
				};
				await PrepaidCardRepository.AddAsync(item);
			}

			//var to = SessionData.Login;
			//MailService.Send(to, "Añadidas tarjetas", "Se han añadido tarjetas al usuario " + SessionData.Login);

			return null;
		}
		#endregion ExecuteAsync
	}
}
