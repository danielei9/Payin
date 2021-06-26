using Microsoft.Practices.Unity;
using PayIn.Application.Dto.JustMoney.Arguments;
using PayIn.Application.JustMoney.Services;
using PayIn.BusinessLogic.Common;
using PayIn.Domain.JustMoney;
using PayIn.Domain.JustMoney.Enums;
using PayIn.Infrastructure.JustMoney.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xp.Application.Dto;
using Xp.Common;
using Xp.Domain;
using Xp.Infrastructure.Mail;

namespace PayIn.Application.JustMoney.Handlers
{
	public class JustMoneyApiPrepaidCardRegisterCardCreateHandler : IServiceBaseHandler<JustMoneyApiPrepaidCardRegisterCardArguments>
	{
		[Dependency] public ISessionData SessionData { get; set; }
		[Dependency] public IEntityRepository<PrepaidCard> PrepaidCardRepository { get; set; }
		[Dependency] public PfsService PfsService { get; set; }
		[Dependency] public MailService MailService { get; set; }

		#region ExecuteAsync
		public async Task<dynamic> ExecuteAsync(JustMoneyApiPrepaidCardRegisterCardArguments arguments)
		{
			var otherCardHolderId = (await PrepaidCardRepository.GetAsync())
				.Where(x =>
					x.Login == SessionData.Login &&
					x.Id == arguments.CardId &&
					x.State == PrepaidCardState.Active
				)
				.Select(x => x.CardHolderId)
				.FirstOrDefault();
			if (otherCardHolderId == null)
				throw new ArgumentNullException("CardId");

			var resultPfs = await PfsService.CardInquiryAsync(otherCardHolderId);

			await RegisterCardAsync(
				arguments.CardHolderId,
				resultPfs.CardHolder.FirstName, resultPfs.CardHolder.LastName,
				arguments.Alias,
				resultPfs.CardHolder.Dob,
				resultPfs.CardHolder.Phone2, resultPfs.CardHolder.Phone,
				SessionData.Email,
				resultPfs.CardHolder.Address1, resultPfs.CardHolder.Address2, resultPfs.CardHolder.City, resultPfs.CardHolder.ZipCode, resultPfs.CardHolder.State, resultPfs.CardHolder.CountryCode
			);

			return null;
		}
		#endregion ExecuteAsync

		#region RegisterCardAsync
		public async Task<PrepaidCard> RegisterCardAsync(
			string cardHolderId,
			string firstName, string lastName,
			string alias,
			XpDate birthDay,
			string phone, string mobile,
			string email,
			string address1, string address2, string city, string zipCode, string province, JustMoneyCountryEnum country
		)
		{
			var existe = (await PrepaidCardRepository.GetAsync())
				.Where(x =>
					x.Login == SessionData.Login &&
					x.CardHolderId == cardHolderId &&
					x.State == PrepaidCardState.Active
				)
				.Any();
			if (existe)
				throw new ArgumentException("La tarjeta ya està registrarda en el sistema", "CardHolderId");
			
			// Create
			var resultCreate = await PfsService.UpdateCardAsync(
				cardHolderId,
				firstName, lastName, birthDay,
				address1, address2, zipCode, city, province, country,
				phone, mobile, email
			);

			// Get
			var resultGet = await PfsService.CardInquiryAsync(cardHolderId);

			var item = new PrepaidCard
			{
				State = PrepaidCardState.Active,
				Login = email,
				Alias = alias,
				CardHolderId = cardHolderId,
				Pan = resultGet.CardHolder.CardNumber
			};
			await PrepaidCardRepository.AddAsync(item);

			return item;
		}
		#endregion RegisterCardAsync
	}
}
