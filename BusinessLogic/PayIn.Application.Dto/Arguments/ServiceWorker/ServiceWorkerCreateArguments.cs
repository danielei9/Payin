using PayIn.Common.Resources;
using System.ComponentModel.DataAnnotations;
using Xp.Common.Dto.Arguments;
using Xp.Infrastructure.Mail;

namespace PayIn.Application.Dto.Arguments.ServiceWorker
{
	public class ServiceWorkerCreateArguments : IArgumentsBase
	{
		[Display(Name="resources.serviceWorker.login")] [DataType(DataType.EmailAddress)] public string Login { get; set; }
		[Display(Name="resources.serviceWorker.name")]                                      public string Name { get; set; }

		#region Constructor
		public ServiceWorkerCreateArguments(string login, string name)
		{
			Login = login;
			Name = name;
			var mailService = new MailService();
			mailService.Send(Login,
				ServiceWorkerResources.AsignationMailSubject,
				string.Format(
					ServiceWorkerResources.AsignationMailContent,
					name
				)
			);

		}
		#endregion Constructor
	}
}
