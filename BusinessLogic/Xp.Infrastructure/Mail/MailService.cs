using System;
using System.Net;
using System.Net.Mail;
using System.Net.Mime;

namespace Xp.Infrastructure.Mail
{
	public class MailService : IMailService
	{
		public void Send(string to, string subject, string body)
		{
			Send("system@swat-id.com", to, subject, body);
		}

		public void Send(string from, string to, string subject, string body)
		{
			Send("smtp.gmail.com", 587, "system@swat-id.com", "SwatKukut2020!", from, to, subject, body);
		}

		public void Send(string smtp, int port, string senderUser, string senderPassword, string from, string to, string subject, string body)
		{
			var message = new MailMessage(from, to);
			message.Subject = subject;
			message.AlternateViews.Add(AlternateView.CreateAlternateViewFromString(body, null, MediaTypeNames.Text.Plain));
			message.AlternateViews.Add(AlternateView.CreateAlternateViewFromString(body, null, MediaTypeNames.Text.Html));

			var client = new SmtpClient(smtp, port)
			{
				DeliveryMethod = SmtpDeliveryMethod.Network,
				Credentials = new NetworkCredential(senderUser, senderPassword)
			};

			client.Send(message);
		}
	}
}
