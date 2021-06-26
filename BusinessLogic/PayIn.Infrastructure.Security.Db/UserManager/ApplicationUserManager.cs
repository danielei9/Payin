using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security.DataProtection;
using PayIn.DistributedServices.Security.Db.Services;
using PayIn.Domain.Security;
using System;

namespace PayIn.Infrastructure.Security.Db.UserManager
{
	public class ApplicationUserManager : UserManager<ApplicationUser>
	{
		public ApplicationUserManager(IUserStore<ApplicationUser> store)
			: base(store)
		{
		}

		public static ApplicationUserManager Create(IUserStore<ApplicationUser> store /*IdentityFactoryOptions<ApplicationUserManager> options, IOwinContext context*/)
		{
			var manager = new ApplicationUserManager(store);

			// Configure user lockout defaults
			manager.UserLockoutEnabledByDefault = true;
			manager.DefaultAccountLockoutTimeSpan = TimeSpan.FromMinutes(5);
			manager.MaxFailedAccessAttemptsBeforeLockout = 5;
			// Register two factor authentication providers
			manager.RegisterTwoFactorProvider(
				"EmailCode",
				new EmailTokenProvider<ApplicationUser>
				{
					Subject = "SecurityCode",
					BodyFormat = "Your security code is {0}"
				}
			);
			manager.EmailService = new EmailService();
			//manager.RegisterTwoFactorProvider(
			//	"PhoneCode",
			//	new PhoneNumberTokenProvider<ApplicationUser> {
			//		MessageFormat = "Your security code is: {0}"
			//	});
			//manager.SmsService = new SmsService();

			// Configure la lógica de validación de nombres de usuario
			manager.UserValidator = new UserValidator<ApplicationUser>(manager)
			{
				AllowOnlyAlphanumericUserNames = false,
				RequireUniqueEmail = true
			};

			// Configure la lógica de validación de contraseñas
			manager.PasswordValidator = new PasswordValidator
			{
				RequiredLength = 6,
				//RequireNonLetterOrDigit = true,
				//RequireDigit = true,
				//RequireLowercase = true,
				//RequireUppercase = true,
			};

			var provider = new DpapiDataProtectionProvider("Sample");
			if (provider != null)
				manager.UserTokenProvider = new DataProtectorTokenProvider<ApplicationUser>(provider.Create("EmailConfirmation"));

			return manager;
		}
	}
}
