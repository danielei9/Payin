using Microsoft.Practices.Unity;
using PayIn.Application.Dto.JustMoney.Arguments;
using PayIn.Application.Dto.Security.Arguments;
using PayIn.Application.JustMoney.Services;
using PayIn.Domain.JustMoney;
using PayIn.Domain.JustMoney.Enums;
using PayIn.Domain.Security;
using PayIn.Infrastructure.JustMoney.Enums;
using PayIn.Infrastructure.Security;
using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Xp.Application.Dto;
using Xp.Common;
using Xp.Domain;
using Xp.Infrastructure.Mail;

namespace PayIn.Application.JustMoney.Handlers
{
	public class JustMoneyApiPrepaidCardCreateUserAndRequestCardHandler : IServiceBaseHandler<JustMoneyApiPrepaidCardCreateUserAndRequestCardArguments>
	{
#if DEBUG || EMULATOR
		public string url = "http://localhost:9090";
#else
		public string url = "http://justmoney.pay-in.es";
#endif

		[Dependency] public IEntityRepository<PrepaidCard> PrepaidCardRepository { get; set; }
		[Dependency] public PfsService PfsService { get; set; }
		[Dependency] public IUnitOfWork UnitOfWork { get; set; }
		[Dependency] public JustMoneyJustMoneyPrepaidCardGetAllHandler JustMoneyJustMoneyPrepaidCardGetAllHandler { get; set; }
		[Dependency] public JustMoneyApiPrepaidCardRechargeCardHandler JustMoneyApiPrepaidCardRechargeCardHandler { get; set; }
		[Dependency] public MailService MailService { get; set; }

		#region ExecuteAsync
		public async Task<dynamic> ExecuteAsync(JustMoneyApiPrepaidCardCreateUserAndRequestCardArguments arguments)
		{
			var now = DateTime.UtcNow;

			await RegisterUserAsync(
				arguments.FirstName, arguments.LastName,
				arguments.Alias,
				arguments.BirthDay,
				arguments.Phone, arguments.Mobile,
				arguments.Email, arguments.ConfirmEmail,
				arguments.Password,
				arguments.Address1, arguments.Address2, arguments.City, arguments.ZipCode, arguments.Province, arguments.Country
			);

			var redirectUrl = await RequestCardAsync(
				arguments.FirstName, arguments.LastName,
				arguments.Alias,
				arguments.BirthDay,
				arguments.Phone, arguments.Mobile,
				arguments.Email,
				arguments.Address1, arguments.Address2, arguments.City, arguments.ZipCode, arguments.Province, arguments.Country,
				now
			);

			return new
			{
				RedirectUrl = redirectUrl
			};
		}
		#endregion ExecuteAsync

		#region RegisterUserAsync
		internal async Task RegisterUserAsync(
			string firstName, string lastName,
			string alias,
			XpDate birthDay,
			string phone, string mobile,
			string email, string confirmEmail,
			string password,
			string address1, string address2, string city, string zipCode, string province, JustMoneyCountryEnum country
		)
		{
			if (email != confirmEmail)
				throw new ArgumentException("El email y su confirmación no son iguales.");
			
			var to = email;
			var route = url + "/app/email/";
			var routeToMail = route + "welcome.html";

			var address = address1 + "<br>";
			if (address2 != "")
				address += address2 + "<br>";
			address += zipCode + " " + city + " (" + province + ")" + "<br>";
			address += country;

			WebClient client = new WebClient();
			client.Encoding = Encoding.UTF8;
			var mail = client.DownloadString(routeToMail);

			// {0} = Nombre. {1} = Alias. {2} = Direccion en envío.
			mail = mail.Replace("{0}", firstName);
			mail = mail.Replace("{1}", alias);
			mail = mail.Replace("{2}", address);
			//MailService.Send("smtp.serviciodecorreo.es", 465, "system@justmoney.es", "System2018", "system@justmoney.es", to, "Usuario creado en Just Money", mail);
			MailService.Send("system@justmoney.es", to, "Usuario creado en JustMoney", mail);
			
			// Crear el usuario
			var securityRepository = new SecurityRepository();
			var newUser = new AccountRegisterArguments
			{
				AcceptTerms = true,
				UserName = email,
				Password = password,
				ConfirmPassword = password,
				Name = firstName + " " + lastName,
				Mobile = mobile + " / " + phone,
				Birthday = birthDay
			};
			await securityRepository.CreateUserAsync(newUser, AccountRoles.User);
		}
		#endregion RegisterUserAsync

		#region RequestCardAsync
		public async Task<string> RequestCardAsync(
			string firstName, string lastName,
			string alias,
			XpDate birthDay,
			string phone, string mobile,
			string email,
			string address1, string address2, string city, string zipCode, string province, JustMoneyCountryEnum country,
			DateTime now
		)
		{
			// Create
			var resultCreate = await PfsService.CardIssueAsync(
				firstName, lastName, birthDay,
				address1, address2, zipCode, city, province, country,
				phone, mobile, email
			);

			// Get
			var resultGet = await PfsService.CardInquiryAsync(resultCreate.CardHolderId);
			var item = new PrepaidCard
			{
				State = PrepaidCardState.Active,
				Login = email,
				Alias = alias,
				CardHolderId = resultCreate.CardHolderId,
				Pan = resultGet.CardHolder.CardNumber
			};
			await PrepaidCardRepository.AddAsync(item);

			// Pay for card
			var redirectUrl = await JustMoneyApiPrepaidCardRechargeCardHandler.RechargeAsync(resultCreate.CardHolderId, 10, now);

			return redirectUrl;
		}
		#endregion RequestCardAsync
	}
}
