using Microsoft.Practices.Unity;
using PayIn.Application.Dto.JustMoney.Arguments;
using PayIn.Application.Dto.Security.Arguments;
using PayIn.Application.JustMoney.Services;
using PayIn.BusinessLogic.Common;
using PayIn.Domain.JustMoney;
using PayIn.Domain.JustMoney.Enums;
using PayIn.Domain.Security;
using PayIn.Infrastructure.JustMoney.Enums;
using PayIn.Infrastructure.Security;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using Xp.Application.Dto;
using Xp.Domain;
using Xp.Infrastructure.Mail;

namespace PayIn.Application.JustMoney.Handlers
{
	public class JustMoneyApiPrepaidCardForgotPasswordHandler : IServiceBaseHandler<JustMoneyApiPrepaidCardForgotPasswordArguments>
	{
		//[Dependency] public ISessionData SessionData { get; set; }
		[Dependency] public IEntityRepository<PrepaidCard> PrepaidCardRepository { get; set; }
		[Dependency] public PfsService PfsService { get; set; }
		[Dependency] public IUnitOfWork UnitOfWork { get; set; }
		[Dependency] public JustMoneyJustMoneyPrepaidCardGetAllHandler JustMoneyJustMoneyPrepaidCardGetAllHandler { get; set; }
		[Dependency] public MailService MailService { get; set; }

		#region ExecuteAsync
		public async Task<dynamic> ExecuteAsync(JustMoneyApiPrepaidCardForgotPasswordArguments arguments)
		{
			var to = arguments.Email;
			var changePasswordUri = "http://localhost:9090/#!/ChangePassword?email=" + to;
			if (!(new EmailAddressAttribute().IsValid(to)))
				throw new ArgumentException("La dirección de correo electrónico no es válida");

			var route = "http://localhost:9090/app/email/";
			var routeToMail = route + "resetPassword.html";

			System.Net.WebClient client = new System.Net.WebClient();
			client.Encoding = System.Text.Encoding.UTF8;
			var mail = client.DownloadString(routeToMail);

			// {0} = Nombre.
			mail = mail.Replace("{0}", changePasswordUri);
			//MailService.Send("smtp.serviciodecorreo.es", 465, "system@justmoney.es", "System2018", "system@justmoney.es", to, "Usuario creado en Just Money", mail);
			MailService.Send("system@justmoney.es", to, "Cambio de contraseña en Just Money", mail);

			return null;
		}
		#endregion ExecuteAsync
	}
}
