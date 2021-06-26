using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using PayIn.Application.Dto.Arguments.SystemCardMember;
using PayIn.Application.Dto.Security.Arguments;
using PayIn.Application.Dto.Security.Results;
using PayIn.Common;
using PayIn.Common.Security;
using PayIn.Domain.Payments;
using PayIn.Domain.Public;
using PayIn.Domain.Security;
using PayIn.Infrastructure.Security.Db;
using PayIn.Infrastructure.Security.Db.UserManager;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using Xp.Application.Dto;
using Xp.Common;
using Xp.Common.Resources;
using Xp.Domain;
using Xp.Infrastructure.Repositories;
using Xp.Infrastructure.Services;
using PayIn.BusinessLogic.Common;

namespace PayIn.Infrastructure.Security
{
    public class SecurityRepository : IDisposable
    {
#if FALLAS
		string url = "https://fallas.pay-in.es";
#elif FAURA
		string url = "http://faura.pay-in.es";
#elif CARCAIXENT
		string url = "http://carcaixent.pay-in.es";
#elif FINESTRAT
		string url = "http://finestrat.pay-in.es";
#elif JUSTMONEY
		string url = "http://justmoney.pay-in.es";
#elif VILAMARXANT
		string url = "http://vilamarxant.pay-in.es";
#elif VINAROS
		string url = "http://vinaros.pay-in.es";
#elif TEST
		string url = "http://payin-test.cloudapp.net";
#elif HOMO
		string url = "http://payin-homo.cloudapp.net";
#elif DEBUG || EMULATOR
        string url = "http://localhost:8080";
#else
        string url = "https://control.pay-in.es";
#endif

        const string CLAVE_POR_DEFECTO = "!-m31oNc0njamon,.";

        private readonly AuthContext Context;
        private readonly ApplicationUserManager UserManager;
        private readonly RoleManager<IdentityRole> RoleManager;

        #region Constructors
        public SecurityRepository()
        {
            Context = new AuthContext();
            UserManager = ApplicationUserManager.Create(new UserStore<ApplicationUser>(Context));
            RoleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(Context));
        }
        #endregion Constructors

        #region GetUserAsync
        public async Task<ApplicationUser> GetUserAsync(string name)
        {
            var user = await UserManager.FindByNameAsync(name);
            return user;
        }
        public async Task<ApplicationUser> GetUserAsync(string name, string password)
        {
            var user = await UserManager.FindAsync(name, password);
            return user;
        }
        #endregion GetUserAsync

        #region InviteWorkerAsync
        public async Task InviteWorkerAsync(AccountRegisterArguments userModel)
        {
            var roleName = "PaymentWorker";
            var role = Context.Roles
                .Where(x => x.Name == roleName)
                .FirstOrDefault();
            if (role == null)
                throw new ApplicationException(string.Format("Identity role not exists {0}", roleName));

            var user = await InviteUserAsync(userModel);
            if (!user.Roles.Any(x => x.RoleId == role.Id))
                user.Roles.Add(new IdentityUserRole { UserId = user.Id, RoleId = role.Id });
        }
        #endregion InviteWorkerAsync

        #region InviteUserAsync
        public async Task<ApplicationUser> InviteUserAsync(AccountRegisterArguments userModel, string concessionPhotoUrl = "", string concessionName = "Pay[in]", string emailText = "")
        {
            // string userId = "3b068d74-68b5-45a5-b8c6-c0bd397e6ab7";

            var securityRepository = this;
            var user = await securityRepository.GetUserAsync(userModel.UserName);
            if (user == null)
            {
                // Crear el usuario, pero con email no validado
                user = new ApplicationUser
                {
                    UserName = userModel.UserName,
                    Email = userModel.UserName,
                    Name = userModel.Name,
                    Mobile = userModel.Mobile ?? "",
                    TaxName = userModel.TaxName ?? "",
                    TaxNumber = userModel.TaxNumber ?? "",
                    TaxAddress = userModel.TaxAddress ?? "",
                    Sex = SexType.Undefined,
                    Birthday = userModel.Birthday,
                    PhotoUrl = userModel.PhotoUrl ?? "",
                    isBussiness = userModel.isBussiness,
                };

                var roleName = "User";
                var role = Context.Roles
                    .Where(x => x.Name == roleName)
                    .FirstOrDefault();
                if (role == null)
                    throw new ApplicationException(string.Format("Identity role not exists {0}", roleName));
                user.Roles.Add(new IdentityUserRole { UserId = user.Id, RoleId = role.Id });

                userModel.Password = CLAVE_POR_DEFECTO;
                ThrowExceptionIfError(await UserManager.CreateAsync(user, userModel.Password));
            }

            // El usuario ya está creado, por lo que hay que enviar el mail de validación
            var token = await UserManager.GenerateEmailConfirmationTokenAsync(user.Id);

            var subject = string.Format(
                SecurityResources.InviteUserEmailSubject,
                concessionName
            );

            if (emailText == "")
                emailText = SecurityResources.InviteUserEmailContent;

            var content = string.Format(
                emailText,
                user.Id,
                HttpUtility.UrlEncode(token),
                user.Name,
                user.Email,
                user.Mobile,
                url,
                concessionPhotoUrl,
                concessionName
            );
            await UserManager.SendEmailAsync(
                user.Id,
                subject,
                content
            );

            return user;
        }
        #endregion InviteUserAsync

        #region InviteSystemCardMemberAsync
        public async Task InviteSystemCardMemberAsync(SystemCardMemberCreateArguments userModel)
        {
            // string userId = "3b068d74-68b5-45a5-b8c6-c0bd397e6ab7";

            var securityRepository = this;
            var user = await securityRepository.GetUserAsync(userModel.Login);
            if (user == null)
            {
                // Crear el usuario, pero con email no validado
                user = new ApplicationUser
                {
                    UserName = userModel.Login,
                    Email = userModel.Login,
                    Name = userModel.Name,
                    Mobile = "",
                    TaxName = "",
                    TaxNumber = "",
                    TaxAddress = "",
                    Sex = SexType.Undefined,
                    PhotoUrl = ""
                };

                var roleName = "User";
                var role = Context.Roles
                    .Where(x => x.Name == roleName)
                    .FirstOrDefault();
                if (role == null)
                    throw new ApplicationException(string.Format("Identity role not exists {0}", roleName));

                user.Roles.Add(new IdentityUserRole { UserId = user.Id, RoleId = role.Id });

                roleName = "CommercePayment";
                role = Context.Roles
                    .Where(x => x.Name == roleName)
                    .FirstOrDefault();
                if (role == null)
                    throw new ApplicationException(string.Format("Identity role not exists {0}", roleName));

                user.Roles.Add(new IdentityUserRole { UserId = user.Id, RoleId = role.Id });


                ThrowExceptionIfError(await UserManager.CreateAsync(user, CLAVE_POR_DEFECTO));

                // El usuario ya está creado, por lo que hay que enviar el mail de validación
                var token = await UserManager.GenerateEmailConfirmationTokenAsync(user.Id);

                await UserManager.SendEmailAsync(
                    user.Id,
                    SecurityResources.ConfirmCompanyEmailSubject,
                    string.Format(
                        SecurityResources.InviteCompanyUserEmailContent,
                        user.Id,
                        HttpUtility.UrlEncode(token),
                        user.Name,
                        user.Email,
                        url
                    )

                );

            }

        }
        #endregion InviteSystemCardMemberAsync

        #region GetUserIdentityAsync
        public async Task<ApplicationUser> GetUserIdentityAsync()
        {
            var identity = Thread.CurrentPrincipal.Identity as ClaimsIdentity;
            var login = identity.Name;
            //var login = "transport@pay-in.es";

            var user = await UserManager.FindByNameAsync(login);

            return user;
        }
        #endregion GetUserIdentityAsync

        #region GetUsersByNIF
        public async Task<List<AccountGetUsersResult>> GetUsersByNIF(string nif)
        {
            return await Task.Run(() =>
            {
                var users = Context.Users
                    .Select(x => new AccountGetUsersResult
                    {
                        Id = x.Id,
                        Name = x.Name,
                        UserName = x.UserName,
                        Email = x.Email,
                        TaxNumber = x.TaxNumber
                    })
                    .Where(x => x.TaxNumber == nif)
                    .OrderBy(x => x.Email)
                    .ToList();

                return users;
            });
        }
        #endregion GetUsersByNIF

        #region CreateUserAsync
        public async Task CreateUserAsync(AccountRegisterArguments userModel, string roleName)
        {
            if (!userModel.AcceptTerms)
                throw new ApplicationException(string.Format("To register you must accept terms of service"));

            var role = Context.Roles
                .Where(x => x.Name == roleName)
                .FirstOrDefault();
            if (role == null)
                throw new ApplicationException(string.Format("Identity role not exists {0}", roleName));

            var oldUser = await UserManager.FindByNameAsync(userModel.UserName);

            if (userModel.isBussiness == UserType.User)
            {
                if (oldUser != null && !oldUser.EmailConfirmed)
                    UserManager.Delete(oldUser);
            }
            else if ((userModel.isBussiness != UserType.User))
            {
                if (oldUser != null)
                    throw new ApplicationException(string.Format("{0} ya existe , adquiera sus productos desde el panel de control", userModel.UserName));
            }

            var user = new ApplicationUser
            {
                UserName = userModel.UserName,
                Email = userModel.UserName,
                Name = userModel.Name,
                Mobile = userModel.Mobile,
                TaxName = userModel.TaxName ?? "",
                TaxNumber = userModel.TaxNumber ?? "",
                TaxAddress = userModel.TaxAddress ?? "",
                Sex = SexType.Undefined,
                Birthday = userModel.Birthday,
                PhotoUrl = userModel.PhotoUrl ?? "",
                isBussiness = userModel.isBussiness
            };

            user.Roles.Add(new IdentityUserRole { UserId = user.Id, RoleId = role.Id });
            ThrowExceptionIfError(await UserManager.CreateAsync(user, userModel.Password));

            var token = await UserManager.GenerateEmailConfirmationTokenAsync(user.Id);

            if (userModel.isBussiness == UserType.User)
                await UserManager.SendEmailAsync(
                    user.Id,
                    SecurityResources.ConfirmEmailSubject,
                    string.Format(
                        SecurityResources.ConfirmEmailContentUser,
                        user.Id,
                        HttpUtility.UrlEncode(token),
                        user.Name,
                        user.Email,
                        url
                     )
                 );
            else if (userModel.isBussiness == UserType.Company)
                await UserManager.SendEmailAsync(
                    user.Id,
                    SecurityResources.ConfirmEmailSubject,
                    string.Format(
                        SecurityResources.ConfirmEmailContentCompany,
                        user.Id,
                        HttpUtility.UrlEncode(token),
                        user.Name,
                        user.Email,
                        url
                    )
                );
            else
            {
#if VILAMARXANT
                await UserManager.SendEmailAsync(
                	user.Id,
                	SecurityResources.ConfirmEmailSubject_Vilamarxant,
                	string.Format(
                		SecurityResources.ConfirmEmailContent_Vilamarxant,
                		user.Id,
                		HttpUtility.UrlEncode(token),
                		user.Name,
                		user.Email,
                		url
                	)
                );
#elif FINESTRAT
                await UserManager.SendEmailAsync(
                        user.Id,
                        SecurityResources.ConfirmEmailSubject_Finestrat,
                        string.Format(
                            SecurityResources.ConfirmEmailContent_Finestrat,
                            user.Id,
                            HttpUtility.UrlEncode(token),
                            user.Name,
                            user.Email,
                            url
                        )
                    );
#else
                await UserManager.SendEmailAsync(
                    user.Id,
                    SecurityResources.ConfirmEmailSubject,
                    string.Format(
                        SecurityResources.ConfirmEmailContentSelfEmployed,
                        user.Id,
                        HttpUtility.UrlEncode(token),
                        user.Name,
                        user.Email,
                        url
                    )
                );
#endif
            }
            var analytics = new MixPanelService(null);
            await analytics.TrackEventAsync("AccountCreate", new Dictionary<string, object>() { { "distinct_id", user.Email } });
            await analytics.TrackUserAsync(user.Email, user.Name, user.Email);
        }

        #endregion CreateUserAsync

        #region ConfirmEmail
        public async Task ConfirmEmail(AccountConfirmArguments arguments)
        {
            var response = await UserManager.ConfirmEmailAsync(arguments.userId, arguments.code);
            ThrowExceptionIfError(response);

            var user = UserManager.FindById(arguments.userId);

            var analytics = new MixPanelService(null);
            await analytics.TrackEventAsync("AccountConfirmEmail", new Dictionary<string, object>() { { "distinct_id", user.Email } });
        }
        #endregion ConfirmEmail

        #region ConfirmEmailAndData
        public async Task ConfirmEmailAndData(AccountConfirmEmailAndDataArguments arguments)
        {
            var response = await UserManager.ConfirmEmailAsync(arguments.userId, arguments.code);
            ThrowExceptionIfError(response);

            var user = UserManager.FindById(arguments.userId);
            user.Mobile = arguments.mobile;
            var result = await UserManager.UpdateAsync(user);

            var userModel = new AccountOverridePasswordArguments();
            userModel.UserName = user.UserName;
            userModel.Password = arguments.password;
            userModel.ConfirmPassword = arguments.password;
            await OverridePassword(userModel);

            var analytics = new MixPanelService(null);
            await analytics.TrackEventAsync("AccountConfirmEmailAndData", new Dictionary<string, object>() { { "distinct_id", user.Email } });
        }
        #endregion ConfirmEmailAndData

        #region UpdatePassword
        public async Task UpdatePassword(AccountChangePasswordArguments userModel)
        {
            var identity = Thread.CurrentPrincipal.Identity as ClaimsIdentity;
            var login = identity.Name;

            var user = await UserManager.FindByNameAsync(login);

            var result = await UserManager.ChangePasswordAsync(user.Id, userModel.OldPassword, userModel.Password);
            ThrowExceptionIfError(result);
        }
        #endregion UpdatePassword

        #region ForgotPassword
        public async Task ForgotPassword(AccountForgotPasswordArguments arguments)
        {
            var user = await UserManager.FindByNameAsync(arguments.Email);

            if (user == null)
                throw new ApplicationException("Usuario inexistente");

            var token = UserManager.GeneratePasswordResetToken(user.Id);

            var forBody = "";
            var body = "";

#if VILAMARXANT
			forBody = SecurityResources.ForgotPasswordContent_Vilamarxant;
#elif FINESTRAT
            forBody = SecurityResources.ForgotPasswordContent_Finestrat;
#else
            forBody = SecurityResources.ForgotPasswordContent;
#endif

#if JUSTMONEY
			// forBody = "";

			if (url == "http://localhost:8080")
				url = "http://localhost:9090"; //Para DEBUG, ya que el servidor está en el puerto 9090

			var host = url;
			var routeToMail = host + "/app/email/forgotPassword.html";

			var userid_0 = user.Id;
			var code_1 = HttpUtility.UrlEncode(token);
			var url_2 = host + "/#/";
			var to_3 = arguments.Email;

			System.Net.WebClient client = new System.Net.WebClient();
			client.Encoding = System.Text.Encoding.UTF8;
			body = client.DownloadString(routeToMail);

			body = body.Replace("{0}", userid_0);
			body = body.Replace("{1}", code_1);
			body = body.Replace("{2}", url_2);
			body = body.Replace("{3}", to_3);
#endif

            if (forBody != "")
                body = string.Format(
                    forBody,
                    user.Id,
                    HttpUtility.UrlEncode(token),
                    user.Name,
                    user.Email,
                    user.Mobile,
                    url
                );

            await UserManager.SendEmailAsync(
                user.Id,
                SecurityResources.ForgotPasswordSubject,
                body
            );
        }
        #endregion ForgotPassword

        #region OverwritePassword
        public async Task OverwritePassword(AccountOverwritePasswordArguments arguments)
        {
            var user = UserManager.FindById(arguments.Id);

            if (user == null)
                throw new ArgumentNullException("user");

            await UserManager.RemovePasswordAsync(user.Id);
            await UserManager.AddPasswordAsync(user.Id, arguments.Password);

            await UserManager.SendEmailAsync(
                user.Id,
                SecurityResources.OverridePasswordSubject,
                string.Format(
                    SecurityResources.OverridePasswordContent,
                        user.Name
                    )
            );
        }
        #endregion OverwritePassword

        #region UnlockUser
        public async Task UnlockUser(AccountUnlockUserArguments arguments)
        {
            var user = UserManager.FindById(arguments.Id);

            await UnlockUsr(user);
        }

        public async Task UnlockUserByEmail(AccountUnlockUserByEmailArguments arguments)
        {
            var user = UserManager.FindByEmail(arguments.Email);

            await UnlockUsr(user);
        }

        public async Task UnlockUsr(ApplicationUser user)
        {
            if (user == null)
                throw new ArgumentNullException("user");

            user.LockoutEnabled = false;
            user.LockoutEndDateUtc = null;
            user.AccessFailedCount = 0;

            await UserManager.UpdateAsync(user);

            await UserManager.SendEmailAsync(
                user.Id,
                SecurityResources.UnlockUserSubject,
                string.Format(
                    SecurityResources.UnlockUserContent,
                        user.Name
                    )
            );
        }
        #endregion UnlockUser

        #region LockUser
        public async Task LockUser(AccountLockUserArguments arguments)
        {
            var user = UserManager.FindByEmail(arguments.Email);

            if (user == null)
                throw new ArgumentNullException("user");

            user.AccessFailedCount = 99;

            await UserManager.UpdateAsync(user);
        }
        #endregion LockUser

        #region GetUsers
        public async Task<ResultBase<AccountGetUsersResult>> GetUsers()
        {
            return await Task.Run(() =>
            {
                var users = Context.Users
                    .Select(x => new AccountGetUsersResult
                    {
                        Id = x.Id,
                        Name = x.Name,
                        UserName = x.UserName,
                        Email = x.Email,
                        FailedCount = x.AccessFailedCount,
                        Block = false,
                        BlockDate = x.LockoutEndDateUtc,
                        EmailConfirmed = x.EmailConfirmed,
                        Phone = x.Mobile
                    })
                    .OrderBy(x => x.Email);

                return new ResultBase<AccountGetUsersResult> { Data = users };
            });
        }
        #endregion GetUsers

        #region ConfirmForgotPassword
        public async Task ConfirmForgotPassword(AccountConfirmForgotPasswordArguments arguments)
        {
            var result = await UserManager.ResetPasswordAsync(arguments.UserId, arguments.Code, arguments.Password);
            ThrowExceptionIfError(result);
        }
        #endregion ConfirmForgotPassword

        #region OverridePassword
        public async Task OverridePassword(AccountOverridePasswordArguments userModel)
        {
            var user = await UserManager.FindByNameAsync(userModel.UserName);

            var result = await UserManager.RemovePasswordAsync(user.Id);
            ThrowExceptionIfError(result);

            result = await UserManager.AddPasswordAsync(user.Id, userModel.Password);
            ThrowExceptionIfError(result);
        }
        #endregion OverridePassword

        #region AutenticateUserAsync
        public async Task<ApplicationUser> AutenticateUserAsync(string userName, string password)
        {
            var item = await UserManager.FindAsync(userName, password);
            return item;
        }
        #endregion AutenticateUserAsync

        #region GetRoleAsync
        public async Task<IEnumerable<IdentityRole>> GetRoleAsync(IEnumerable<string> ids)
        {
            var result = new List<IdentityRole>();
            foreach (var id in ids)
                result.Add(await RoleManager.FindByIdAsync(id));

            return result;
        }
        #endregion GetRoleAsync

        #region GetClientAsync
        public async Task<Client> GetClientAsync(string clientId)
        {
            var client = await Context.Clients.FindAsync(clientId);
            return client;
        }
        #endregion GetClientAsync

        #region GetRefreshTokenAsync
        public async Task<RefreshToken> GetRefreshTokenAsync(string refreshTokenId)
        {
            var refreshToken = await Context.RefreshTokens.FindAsync(refreshTokenId);

            return refreshToken;
        }
        #endregion GetRefreshTokenAsync

        #region UpdateRefreshTokenAsync
        public async Task<bool> UpdateRefreshTokenAsync(RefreshToken token)
        {
            var previousToken = Context.RefreshTokens
                .Where(x => x.Subject == token.Subject && x.ClientId == token.ClientId)
                .FirstOrDefault();

            if (previousToken != null)
                await DeleteRefreshTokenAsync(previousToken);

            Context.RefreshTokens.Add(token);

            return await Context.SaveChangesAsync() > 0;
        }
        #endregion UpdateRefreshTokenAsync

        #region DeleteRefreshTokenAsync
        public async Task<bool> DeleteRefreshTokenAsync()
        {
            var identity = Thread.CurrentPrincipal.Identity as ClaimsIdentity;

            var userClientIds = (Thread.CurrentPrincipal as ClaimsPrincipal).Claims
                    .Where(x => x.Type == XpClaimTypes.ClientId)
                    .Select(x => x.Value)
                    .FirstOrDefault();

            var refreshToken = Context.RefreshTokens.Where(x => x.Subject == identity.Name && x.ClientId == userClientIds).FirstOrDefault();
            if (refreshToken == null)
                return false;

            return await DeleteRefreshTokenAsync(refreshToken);
        }
        public async Task<bool> DeleteRefreshTokenAsync(string refreshTokenId)
        {
            var refreshToken = await Context.RefreshTokens.FindAsync(refreshTokenId);
            if (refreshToken == null)
                return false;

            return await DeleteRefreshTokenAsync(refreshToken);
        }
        public async Task<bool> DeleteRefreshTokenAsync(RefreshToken refreshToken)
        {
            Context.RefreshTokens.Remove(refreshToken);
            return await Context.SaveChangesAsync() > 0;
        }
        #endregion DeleteRefreshTokenAsync

        #region UpdateRoles
        public async Task<bool> AddRole(String userName, String roleName)
        {
            var user = await UserManager.FindByNameAsync(userName);
            if (user == null)
                return false;

            var role = await RoleManager.FindByNameAsync(roleName);
            if (role == null)
                return false;

            await UserManager.AddToRolesAsync(user.Id, role.Name);

            return true;
        }
        public async Task<bool> RemoveRole(String userName, String roleName)
        {
            var user = await UserManager.FindByNameAsync(userName);
            if (user == null)
                return false;

            var role = await RoleManager.FindByNameAsync(roleName);
            if (role == null)
                return false;

            await UserManager.RemoveFromRolesAsync(user.Id, role.Name);

            return true;
        }
        #endregion UpdateRoles

        #region Dispose
        public void Dispose()
        {
            if (Context != null)
                Context.Dispose();
            if (UserManager != null)
                UserManager.Dispose();
        }
        #endregion Dispose

        #region ThrowExceptionIfError
        private void ThrowExceptionIfError(IdentityResult result)
        {
            if ((!result.Succeeded) && (result.Errors != null))
                throw new ApplicationException(string.Format("Identity system error: {0}", string.Join("\n", result.Errors)));
        }
        #endregion ThrowExceptionIfError

        #region UpdateProfileAsync
        public async Task UpdateProfileAsync(AccountUpdateArguments arguments)
        {
            var identity = Thread.CurrentPrincipal.Identity as ClaimsIdentity;
            var login = identity.Name;
            var now = DateTime.Now;
            var user = await UserManager.FindByNameAsync(login);
            if (arguments.Birthday != null && arguments.Birthday >= now)
                throw new Exception(AccountResources.BirthdayException);

            user.Name = arguments.Name;
            user.Mobile = arguments.Mobile;
            user.Sex = arguments.Sex ?? user.Sex;
            user.TaxNumber = arguments.TaxNumber;
            user.TaxName = arguments.TaxName;
            user.TaxAddress = arguments.TaxAddress;
            user.Birthday = arguments.Birthday != null ? arguments.Birthday.Value : (DateTime?)null;

            await UserManager.UpdateAsync(user);

        }
        #endregion UpdateProfileAsync

        #region UpdateTaxDataAsync
        public async Task UpdateTaxDataAsync(string login, string taxNumber, string taxName, string taxAddress, DateTime? BirthDay)
        {
            var user = await UserManager.FindByNameAsync(login);
            user.TaxNumber = taxNumber;
            user.TaxName = taxName;
            user.TaxAddress = taxAddress;
            if (BirthDay != null) user.Birthday = BirthDay;

            await UserManager.UpdateAsync(user);
        }
        #endregion UpdateTaxDataAsync

        #region UpdateImageUrlAsync
        //Falta testeo -- israel.perez@Pay-in.es
        public async Task UpdateImageUrlAsync(byte[] image)
        {
            var identity = Thread.CurrentPrincipal.Identity as ClaimsIdentity;
            var login = identity.Name;
            var user = await UserManager.FindByNameAsync(login);
            var repository = new AzureBlobRepository();

            user.PhotoUrl = repository.SaveImage(SecurityResources.FotoShortUrl.FormatString(user.Id), image);

            await UserManager.UpdateAsync(user);
        }

        public async Task UpdateImageUrlAsync_FromString(string image)
        {
            var identity = Thread.CurrentPrincipal.Identity as ClaimsIdentity;
            var login = identity.Name;
            var user = await UserManager.FindByNameAsync(login);
            var repository = new AzureBlobRepository();

            byte[] b1 = System.Text.Encoding.UTF8.GetBytes(image);

            string name = user.Id.ToString();
            user.PhotoUrl = repository.SaveImage(SecurityResources.FotoShortUrl.FormatString(name), b1);

            await UserManager.UpdateAsync(user);
        }
        #endregion UpdateImageUrlAsync

        #region DeleteImageAsync
        public async Task DeleteImageAsync()
        {
            var identity = Thread.CurrentPrincipal.Identity as ClaimsIdentity;
            var login = identity.Name;
            var user = await UserManager.FindByNameAsync(login);
            var repository = new AzureBlobRepository();

            if (user.PhotoUrl != "")
            {
                var route = Regex.Split(user.PhotoUrl, "[/?]");
                var fileName = route[route.Length - 2] + "/" + route[route.Length - 1];
                repository.DeleteFile(fileName);
                user.PhotoUrl = "";

                await UserManager.UpdateAsync(user);
            }
        }
        #endregion DeleteImageAsync
    }
}