using Microsoft.Practices.Unity;
using PayIn.Application.Dto.JustMoney.Arguments;
using PayIn.Application.JustMoney.Services;
using PayIn.Domain.JustMoney;
using PayIn.Domain.JustMoney.Enums;
using System;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Xp.Application.Dto;
using Xp.Domain;
using Xp.Infrastructure.Mail;

namespace PayIn.Application.JustMoney.Handlers
{
	public class JustMoneyApiPrepaidCardCreateUserRegisterCardHandler : IServiceBaseHandler<JustMoneyApiPrepaidCardCreateUserRegisterCardArguments>
	{
		[Dependency] public IEntityRepository<PrepaidCard> PrepaidCardRepository { get; set; }
		[Dependency] public PfsService PfsService { get; set; }
		[Dependency] public IUnitOfWork UnitOfWork { get; set; }
		[Dependency] public JustMoneyJustMoneyPrepaidCardGetAllHandler JustMoneyJustMoneyPrepaidCardGetAllHandler { get; set; }
		[Dependency] public MailService MailService { get; set; }

		#region ExecuteAsync
		public async Task<dynamic> ExecuteAsync(JustMoneyApiPrepaidCardCreateUserRegisterCardArguments arguments)
		{
			if (arguments.Email != arguments.ConfirmEmail)
				throw new ArgumentException("El email y su confirmación no son iguales.");

#if DEBUG || EMULATOR
			string url = "http://localhost:9090";
#else
			string url = "http://justmoney.pay-in.es";
#endif

			var to = arguments.Email;
			var route = url + "/app/email/";
			var routeToMail = route + "welcome.html";

			var address = arguments.Address1 + "<br>";
			if (arguments.Address2 != "")
				address += arguments.Address2 + "<br>";
			address += arguments.ZipCode + " " + arguments.City + " (" + arguments.Province + ")" + "<br>";
			address += arguments.Country;

			WebClient client = new WebClient();
			client.Encoding = Encoding.UTF8;
			var mail = client.DownloadString(routeToMail);

			// {0} = Nombre. {1} = Alias. {2} = Direccion en envío.
			mail = mail.Replace("{0}", arguments.FirstName);
			mail = mail.Replace("{1}", arguments.Alias1);
			mail = mail.Replace("{2}", address);
			//MailService.Send("smtp.serviciodecorreo.es", 465, "system@justmoney.es", "System2018", "system@justmoney.es", to, "Usuario creado en Just Money", mail);
			MailService.Send("system@justmoney.es", to, "Usuario creado en Just Money", mail);
			
			// Update card
			var resultUpdate = await PfsService.UpdateCardAsync(
				arguments.CardHolderId,
				arguments.FirstName, arguments.LastName, arguments.BirthDay,
				arguments.Address1, arguments.Address2, arguments.ZipCode, arguments.City, arguments.Province, arguments.Country,
				arguments.Phone, arguments.Mobile, arguments.Email
			);

			// Get
			var resultGet = await PfsService.CardInquiryAsync(arguments.CardHolderId);
			var item = new PrepaidCard
			{
				State = PrepaidCardState.Active,
				Login = arguments.Email,
				Alias = arguments.Alias1,
				CardHolderId = arguments.CardHolderId,
				Pan = resultGet.CardHolder.CardNumber
			};
			await PrepaidCardRepository.AddAsync(item);

			return null;
		}
		#endregion ExecuteAsync
	}
}
