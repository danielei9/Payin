using Microsoft.AspNet.Identity;
using System;
using System.Net;
using System.Net.Mail;
using System.Net.Mime;
using System.Threading.Tasks;

namespace PayIn.DistributedServices.Security.Db.Services
{
	public class EmailService : IIdentityMessageService
	{
		public Task SendAsync(IdentityMessage message)
		{
			var msg = new MailMessage("system@swat-id.com", message.Destination)
			{
				Subject = message.Subject,
				Body = message.Body,
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
