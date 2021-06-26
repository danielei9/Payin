using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;

namespace Xp.Infrastructure
{
	public class EmailService
	{
		public Task SendAsync(string destinations, string subject, string body)
		{
			var msg = new MailMessage("system@swat-id.com", destinations)
            {
                Subject = subject,
                Body = body,
                IsBodyHtml = true
            };

            var client = new SmtpClient("smtp.gmail.com", 587)
			{
				DeliveryMethod = SmtpDeliveryMethod.Network,
				UseDefaultCredentials = false,
				EnableSsl = true,
				Credentials = new NetworkCredential("system@swat-id.com", "SwatKukut2020!")
			};

			return client.SendMailAsync(msg);
		}
	}
}
