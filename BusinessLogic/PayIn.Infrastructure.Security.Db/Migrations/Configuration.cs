namespace PayIn.Infrastructure.Security.Db.Migrations
{
    using Microsoft.AspNet.Identity;
    using Microsoft.AspNet.Identity.EntityFramework;
    using PayIn.Domain.Security;
    using PayIn.Infrastructure.Security.Db.UserManager;
    using System;
    using System.Data.Entity.Migrations;
    using System.Linq;
    using PayIn.Common;
    using System.Collections.Generic;

    internal sealed class Configuration : DbMigrationsConfiguration<PayIn.Infrastructure.Security.Db.AuthContext>
    {
        #region Constructors

        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
        }

        #endregion

        #region Seed

        protected override void Seed(AuthContext context)
        {
            context.Roles.AddOrUpdate(x => x.Name,
                new IdentityRole { Id = Guid.NewGuid().ToString(), Name = AccountRoles.Superadministrator },
                new IdentityRole { Id = Guid.NewGuid().ToString(), Name = AccountRoles.Administrator },
                new IdentityRole { Id = Guid.NewGuid().ToString(), Name = AccountRoles.User },
                new IdentityRole { Id = Guid.NewGuid().ToString(), Name = AccountRoles.Commerce },
                new IdentityRole { Id = Guid.NewGuid().ToString(), Name = AccountRoles.CommercePayment },
                new IdentityRole { Id = Guid.NewGuid().ToString(), Name = AccountRoles.Operator },
                new IdentityRole { Id = Guid.NewGuid().ToString(), Name = AccountRoles.Tester },
                new IdentityRole { Id = Guid.NewGuid().ToString(), Name = AccountRoles.ControlApi },
                new IdentityRole { Id = Guid.NewGuid().ToString(), Name = AccountRoles.PaymentWorker },
                new IdentityRole { Id = Guid.NewGuid().ToString(), Name = AccountRoles.PaymentWorkerTpv },
                new IdentityRole { Id = Guid.NewGuid().ToString(), Name = AccountRoles.PaymentWorkerCash },
                new IdentityRole { Id = Guid.NewGuid().ToString(), Name = AccountRoles.SystemCardOwner },
                new IdentityRole { Id = Guid.NewGuid().ToString(), Name = AccountRoles.Transport }
            );
            context.Clients.AddOrUpdate(
#if DEBUG
                new Client { Id = AccountClientId.JustMoney, Secret = "JustMoney@PFS".ToHash(), Name = "JustMoney", ApplicationType = ApplicationTypes.NativeConfidential, Active = true, RefreshTokenLifeTime = Convert.ToInt32(TimeSpan.FromDays(360).TotalMinutes), AllowedOrigin = "*" },
#endif
                new Client { Id = AccountClientId.Web, Secret = "PayInWebApp@123456".ToHash(), Name = "PayIn Web Application", ApplicationType = ApplicationTypes.JavaScript, Active = true, RefreshTokenLifeTime = Convert.ToInt32(TimeSpan.FromDays(360).TotalMinutes), AllowedOrigin = "" },
                new Client { Id = AccountClientId.Ios, Secret = "PayInIosApp@123456".ToHash(), Name = "PayIn iOS Application", ApplicationType = ApplicationTypes.NativeConfidential, Active = true, RefreshTokenLifeTime = Convert.ToInt32(TimeSpan.FromDays(360).TotalMinutes), AllowedOrigin = "*" },
                new Client { Id = AccountClientId.Android, Secret = "PayInAndroidApp@123456".ToHash(), Name = "PayIn Android Application", ApplicationType = ApplicationTypes.NativeConfidential, Active = true, RefreshTokenLifeTime = Convert.ToInt32(TimeSpan.FromDays(360).TotalMinutes), AllowedOrigin = "*" },
                new Client { Id = AccountClientId.Wp, Secret = "PayInWpApp@123456".ToHash(), Name = "PayIn WP Application", ApplicationType = ApplicationTypes.NativeConfidential, Active = true, RefreshTokenLifeTime = Convert.ToInt32(TimeSpan.FromDays(360).TotalMinutes), AllowedOrigin = "*" },
                new Client { Id = AccountClientId.Api, Secret = "PayInApi@1492".ToHash(), Name = "PayIn API", ApplicationType = ApplicationTypes.JavaScript, Active = true, RefreshTokenLifeTime = Convert.ToInt32(TimeSpan.FromDays(360).TotalMinutes), AllowedOrigin = "" },
                new Client { Id = AccountClientId.AndroidNative, Secret = "PayInAndroidNativeApp@123456".ToHash(), Name = "PayIn Android Native Application", ApplicationType = ApplicationTypes.NativeConfidential, Active = true, RefreshTokenLifeTime = Convert.ToInt32(TimeSpan.FromDays(360).TotalMinutes), AllowedOrigin = "*" },
                new Client { Id = AccountClientId.AndroidFallesNative, Secret = "PayInAndroidFallesNativeApp@1976".ToHash(), Name = "PayIn Falles Android Native Application", ApplicationType = ApplicationTypes.NativeConfidential, Active = true, RefreshTokenLifeTime = Convert.ToInt32(TimeSpan.FromDays(360).TotalMinutes), AllowedOrigin = "*" },
                new Client { Id = AccountClientId.AndroidVilamarxantNative, Secret = "PayInAndroidVilamarxantNativeApp@1999".ToHash(), Name = "PayIn Vilamarxant Android Native Application", ApplicationType = ApplicationTypes.NativeConfidential, Active = true, RefreshTokenLifeTime = Convert.ToInt32(TimeSpan.FromDays(360).TotalMinutes), AllowedOrigin = "*" },
                new Client { Id = AccountClientId.AndroidFinestratNative, Secret = "PayInAndroidFinestratNativeApp@2014".ToHash(), Name = "PayIn Finestrat Android Native Application", ApplicationType = ApplicationTypes.NativeConfidential, Active = true, RefreshTokenLifeTime = Convert.ToInt32(TimeSpan.FromDays(360).TotalMinutes), AllowedOrigin = "*" },
                new Client { Id = AccountClientId.Tpv, Secret = "PayInTpv@1238".ToHash(), Name = "PayIn TPV", ApplicationType = ApplicationTypes.NativeConfidential, Active = true, RefreshTokenLifeTime = Convert.ToInt32(TimeSpan.FromDays(360).TotalMinutes), AllowedOrigin = "*" },
                new Client { Id = AccountClientId.PaymentApi, Secret = "PayInPayment@1912".ToHash(), Name = "Payin Payment API", ApplicationType = ApplicationTypes.NativeConfidential, Active = true, RefreshTokenLifeTime = Convert.ToInt32(TimeSpan.FromDays(360).TotalMinutes), AllowedOrigin = "*" },
                new Client { Id = AccountClientId.PaymentTsm, Secret = "PayInTsm@1812".ToHash(), Name = "Payin Payment TSM", ApplicationType = ApplicationTypes.NativeConfidential, Active = true, RefreshTokenLifeTime = Convert.ToInt32(TimeSpan.FromDays(360).TotalMinutes), AllowedOrigin = "*" },
                new Client { Id = AccountClientId.FgvTsm, Secret = "FgvTsm@1707".ToHash(), Name = "FGV ValenciaTSM", ApplicationType = ApplicationTypes.NativeConfidential, Active = true, RefreshTokenLifeTime = Convert.ToInt32(TimeSpan.FromDays(360).TotalMinutes), AllowedOrigin = "*" },
                new Client { Id = AccountClientId.AlacantTsm, Secret = "AlacantTsm@1094".ToHash(), Name = "FGV Alacant TSM", ApplicationType = ApplicationTypes.NativeConfidential, Active = true, RefreshTokenLifeTime = Convert.ToInt32(TimeSpan.FromDays(360).TotalMinutes), AllowedOrigin = "*" },
                new Client { Id = AccountClientId.FiraValenciaApp, Secret = "FiraValenciaApp@1815".ToHash(), Name = "Fira Valencia", ApplicationType = ApplicationTypes.NativeConfidential, Active = true, RefreshTokenLifeTime = Convert.ToInt32(TimeSpan.FromDays(360).TotalMinutes), AllowedOrigin = "*" },
                new Client { Id = AccountClientId.CashlessProApp, Secret = "CashlessProApp@1821".ToHash(), Name = "PayIn Cashless Pro", ApplicationType = ApplicationTypes.NativeConfidential, Active = true, RefreshTokenLifeTime = Convert.ToInt32(TimeSpan.FromDays(360).TotalMinutes), AllowedOrigin = "*" },
                new Client { Id = AccountClientId.FallasProApp, Secret = "FallasProApp@1991".ToHash(), Name = "PayIn Fallas Pro", ApplicationType = ApplicationTypes.NativeConfidential, Active = true, RefreshTokenLifeTime = Convert.ToInt32(TimeSpan.FromDays(360).TotalMinutes), AllowedOrigin = "*" },
                new Client { Id = AccountClientId.BusApp, Secret = "BusApp@2019".ToHash(), Name = "PayIn Bus", ApplicationType = ApplicationTypes.NativeConfidential, Active = true, RefreshTokenLifeTime = Convert.ToInt32(TimeSpan.FromDays(360).TotalMinutes), AllowedOrigin = "*" }
            );
            context.SaveChanges();

            #region Superadministrador
            if (!context.Users.Any(x => x.UserName == "superadministrator@pay-in.es"))
            {
                var user = new ApplicationUser
                {
                    UserName = "superadministrator@pay-in.es",
                    Email = "superadministrator@pay-in.es",
                    Name = "Superadministrador",
                    Mobile = "000000000",
                    TaxName = "Superadministrador Pay[in]",
                    TaxNumber = "12345678A",
                    TaxAddress = "Valencia",
                    Birthday = DateTime.Now,
                    Sex = SexType.Undefined,
                    PhotoUrl = ""
                };
                var role = context.Roles
                    .FirstOrDefault(x => x.Name == AccountRoles.Superadministrator);
                user.Roles.Add(new IdentityUserRole { UserId = user.Id, RoleId = role.Id });

                var userManager = new ApplicationUserManager(new UserStore<ApplicationUser>(context));
                userManager.UserValidator = new UserValidator<ApplicationUser>(userManager) { AllowOnlyAlphanumericUserNames = false };

                var result = userManager.Create(user, "Pa$$w0rd");
                if ((!result.Succeeded) && (result.Errors != null))
                    throw new ApplicationException(string.Format("Identity system error: {0}", string.Join("\n", result.Errors)));

                user.EmailConfirmed = true;
                result = userManager.SetLockoutEnabled(user.Id, false);
                if ((!result.Succeeded) && (result.Errors != null))
                    throw new ApplicationException(string.Format("Identity system error: {0}", string.Join("\n", result.Errors)));
            }
            #endregion Superadministrador

#if DEBUG

            #region Commerce

            if (!context.Users.Any(x => x.UserName == "commerce@pay-in.es"))
            {
                var user = new ApplicationUser
                {
                    UserName = "commerce@pay-in.es",
                    Email = "commerce@pay-in.es",
                    Name = "Comercio",
                    Mobile = "000000000",
                    TaxName = "Comercio Pay[in]",
                    TaxNumber = "12345678A",
                    TaxAddress = "Valencia",
                    Birthday = DateTime.Now,
                    Sex = SexType.Undefined,
                    PhotoUrl = ""
                };
                var role = context.Roles
                    .FirstOrDefault(x => x.Name == AccountRoles.Commerce);
                user.Roles.Add(new IdentityUserRole { UserId = user.Id, RoleId = role.Id });
                role = context.Roles
                    .FirstOrDefault(x => x.Name == AccountRoles.User);
                user.Roles.Add(new IdentityUserRole { UserId = user.Id, RoleId = role.Id });
                role = context.Roles
                    .FirstOrDefault(x => x.Name == AccountRoles.CommercePayment);
                user.Roles.Add(new IdentityUserRole { UserId = user.Id, RoleId = role.Id });

                var userManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(context));
                userManager.UserValidator = new UserValidator<ApplicationUser>(userManager) { AllowOnlyAlphanumericUserNames = false };

                var result = userManager.Create(user, "Pa$$w0rd");
                if ((!result.Succeeded) && (result.Errors != null))
                    throw new ApplicationException(string.Format("Identity system error: {0}", string.Join("\n", result.Errors)));

                user.EmailConfirmed = true;
                result = userManager.SetLockoutEnabled(user.Id, false);
                if ((!result.Succeeded) && (result.Errors != null))
                    throw new ApplicationException(string.Format("Identity system error: {0}", string.Join("\n", result.Errors)));
            }

            #endregion

            #region Payment

            if (!context.Users.Any(x => x.UserName == "payment@pay-in.es"))
            {
                var user = new ApplicationUser
                {
                    UserName = "payment@pay-in.es",
                    Email = "payment@pay-in.es",
                    Name = "Comercio pagos",
                    Mobile = "000000000",
                    TaxName = "Comercio pagos Pay[in]",
                    TaxNumber = "12345678A",
                    TaxAddress = "Valencia",
                    Birthday = DateTime.Now,
                    Sex = SexType.Undefined,
                    PhotoUrl = ""
                };
                var role = context.Roles
                    .FirstOrDefault(x => x.Name == AccountRoles.User);
                user.Roles.Add(new IdentityUserRole { UserId = user.Id, RoleId = role.Id });
                role = context.Roles
                    .FirstOrDefault(x => x.Name == AccountRoles.CommercePayment);
                user.Roles.Add(new IdentityUserRole { UserId = user.Id, RoleId = role.Id });

                var userManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(context));
                userManager.UserValidator = new UserValidator<ApplicationUser>(userManager) { AllowOnlyAlphanumericUserNames = false };

                var result = userManager.Create(user, "Pa$$w0rd");
                if ((!result.Succeeded) && (result.Errors != null))
                    throw new ApplicationException(string.Format("Identity system error: {0}", string.Join("\n", result.Errors)));

                user.EmailConfirmed = true;
                result = userManager.SetLockoutEnabled(user.Id, false);
                if ((!result.Succeeded) && (result.Errors != null))
                    throw new ApplicationException(string.Format("Identity system error: {0}", string.Join("\n", result.Errors)));
            }

            #endregion

            #region Operador

            if (!context.Users.Any(x => x.UserName == "operator@pay-in.es"))
            {
                var user = new ApplicationUser
                {
                    UserName = "operator@pay-in.es",
                    Email = "operator@pay-in.es",
                    Name = "Operador",
                    Mobile = "000000000",
                    TaxName = "Operador Pay[in]",
                    TaxNumber = "12345678A",
                    TaxAddress = "Valencia",
                    Birthday = DateTime.Now,
                    Sex = SexType.Undefined,
                    PhotoUrl = ""
                };
                var role = context.Roles
                    .FirstOrDefault(x => x.Name == AccountRoles.Operator);
                user.Roles.Add(new IdentityUserRole { UserId = user.Id, RoleId = role.Id });
                role = context.Roles
                    .FirstOrDefault(x => x.Name == AccountRoles.User);
                user.Roles.Add(new IdentityUserRole { UserId = user.Id, RoleId = role.Id });

                var userManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(context));
                userManager.UserValidator = new UserValidator<ApplicationUser>(userManager) { AllowOnlyAlphanumericUserNames = false };

                var result = userManager.Create(user, "Pa$$w0rd");
                if ((!result.Succeeded) && (result.Errors != null))
                    throw new ApplicationException(string.Format("Identity system error: {0}", string.Join("\n", result.Errors)));

                user.EmailConfirmed = true;
                result = userManager.SetLockoutEnabled(user.Id, false);
                if ((!result.Succeeded) && (result.Errors != null))
                    throw new ApplicationException(string.Format("Identity system error: {0}", string.Join("\n", result.Errors)));
            }

            #endregion

            #region User

            if (!context.Users.Any(x => x.UserName == "user@pay-in.es"))
            {
                var user = new ApplicationUser
                {
                    UserName = "user@pay-in.es",
                    Email = "user@pay-in.es",
                    Name = "Usuario",
                    Mobile = "000000000",
                    TaxName = "Usuario Pay[in]",
                    TaxNumber = "12345678A",
                    TaxAddress = "Valencia",
                    Birthday = DateTime.Now,
                    Sex = SexType.Undefined,
                    PhotoUrl = ""
                };
                var role = context.Roles
                    .FirstOrDefault(x => x.Name == AccountRoles.User);
                user.Roles.Add(new IdentityUserRole { UserId = user.Id, RoleId = role.Id });

                var userManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(context));
                userManager.UserValidator = new UserValidator<ApplicationUser>(userManager) { AllowOnlyAlphanumericUserNames = false };

                var result = userManager.Create(user, "Pa$$w0rd");
                if ((!result.Succeeded) && (result.Errors != null))
                    throw new ApplicationException(string.Format("Identity system error: {0}", string.Join("\n", result.Errors)));

                user.EmailConfirmed = true;
                result = userManager.SetLockoutEnabled(user.Id, false);
                if ((!result.Succeeded) && (result.Errors != null))
                    throw new ApplicationException(string.Format("Identity system error: {0}", string.Join("\n", result.Errors)));
            }

            #endregion

            #region Api

            if (!context.Users.Any(x => x.UserName == "api@pay-in.es"))
            {
                var user = new ApplicationUser
                {
                    UserName = "api@pay-in.es",
                    Email = "api@pay-in.es",
                    Name = "API",
                    Mobile = "000000000",
                    TaxName = "Api Pay[in]",
                    TaxNumber = "12345678A",
                    TaxAddress = "Valencia",
                    Birthday = DateTime.Now,
                    Sex = SexType.Undefined,
                    PhotoUrl = ""
                };
                var role = context.Roles
                    .FirstOrDefault(x => x.Name == AccountRoles.ControlApi);
                user.Roles.Add(new IdentityUserRole { UserId = user.Id, RoleId = role.Id });

                var userManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(context));
                userManager.UserValidator = new UserValidator<ApplicationUser>(userManager) { AllowOnlyAlphanumericUserNames = false };

                var result = userManager.Create(user, "Pa$$w0rd");
                if ((!result.Succeeded) && (result.Errors != null))
                    throw new ApplicationException(string.Format("Identity system error: {0}", string.Join("\n", result.Errors)));

                user.EmailConfirmed = true;
                result = userManager.SetLockoutEnabled(user.Id, false);
                if ((!result.Succeeded) && (result.Errors != null))
                    throw new ApplicationException(string.Format("Identity system error: {0}", string.Join("\n", result.Errors)));
            }

            #endregion

            #region Tpv

            if (!context.Users.Any(x => x.UserName == "tpv@pay-in.es"))
            {
                var user = new ApplicationUser
                {
                    UserName = "tpv@pay-in.es",
                    Email = "tpv@pay-in.es",
                    Name = "TPV",
                    Mobile = "000000000",
                    TaxName = "TPV Pay[in]",
                    TaxNumber = "12345678A",
                    TaxAddress = "Valencia",
                    Birthday = DateTime.Now,
                    Sex = SexType.Undefined,
                    PhotoUrl = ""
                };
                var role = context.Roles
                    .FirstOrDefault(x => x.Name == AccountRoles.User);
                user.Roles.Add(new IdentityUserRole { UserId = user.Id, RoleId = role.Id });
                role = context.Roles
                    .FirstOrDefault(x => x.Name == AccountRoles.PaymentWorker);
                user.Roles.Add(new IdentityUserRole { UserId = user.Id, RoleId = role.Id });

                var userManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(context));
                userManager.UserValidator = new UserValidator<ApplicationUser>(userManager) { AllowOnlyAlphanumericUserNames = false };

                var result = userManager.Create(user, "Pa$$w0rd");
                if ((!result.Succeeded) && (result.Errors != null))
                    throw new ApplicationException(string.Format("Identity system error: {0}", string.Join("\n", result.Errors)));

                user.EmailConfirmed = true;
                result = userManager.SetLockoutEnabled(user.Id, false);
                if ((!result.Succeeded) && (result.Errors != null))
                    throw new ApplicationException(string.Format("Identity system error: {0}", string.Join("\n", result.Errors)));
            }

            #endregion

            #region Transport
            if (!context.Users.Any(x => x.UserName == "transport@pay-in.es"))
            {
                var user = new ApplicationUser
                {
                    UserName = "transport@pay-in.es",
                    Email = "transport@pay-in.es",
                    Name = "Transporte",
                    Mobile = "000000000",
                    TaxName = "Transporte Pay[in]",
                    TaxNumber = "12345678A",
                    TaxAddress = "Valencia",
                    Birthday = DateTime.Now,
                    Sex = SexType.Undefined,
                    PhotoUrl = ""
                };
                var role = context.Roles
                    .FirstOrDefault(x => x.Name == AccountRoles.User);
                user.Roles.Add(new IdentityUserRole { UserId = user.Id, RoleId = role.Id });
                role = context.Roles
                    .FirstOrDefault(x => x.Name == AccountRoles.Transport);
                user.Roles.Add(new IdentityUserRole { UserId = user.Id, RoleId = role.Id });
                role = context.Roles
                    .FirstOrDefault(x => x.Name == AccountRoles.CommercePayment);
                user.Roles.Add(new IdentityUserRole { UserId = user.Id, RoleId = role.Id });

                var userManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(context));
                userManager.UserValidator = new UserValidator<ApplicationUser>(userManager) { AllowOnlyAlphanumericUserNames = false };

                var result = userManager.Create(user, "Pa$$w0rd");
                if ((!result.Succeeded) && (result.Errors != null))
                    throw new ApplicationException(string.Format("Identity system error: {0}", string.Join("\n", result.Errors)));

                user.EmailConfirmed = true;
                result = userManager.SetLockoutEnabled(user.Id, false);
                if ((!result.Succeeded) && (result.Errors != null))
                    throw new ApplicationException(string.Format("Identity system error: {0}", string.Join("\n", result.Errors)));
            }
            #endregion Transport

            #region Vilamarxant

            var vilamarxant = CreateUser(context,
                new CreateUserDto
                {
                    Login = "vilamarxant@pay-in.es",
                    Name = "Aj. Vilamarxant",
                    Email = "vilamarxant@pay-in.es",
                    Phone = "000000000",
                    Birthday = new DateTime(2000, 8, 11),
                    TaxName = "M.I. Ajuntament Vilamarxant",
                    TaxNumber = "12345678A",
                    TaxAddress = "Vilamarxant"
                }
            );
            AddCommercePaymentRole(context, vilamarxant);
            AddPaymentWorkerRole(context, vilamarxant);

            #endregion

            #region Vinaros

            var vinaros = CreateUser(context,
                new CreateUserDto
                {
                    Login = "vinaros@pay-in.es",
                    Name = "Aj. Vinaros",
                    Email = "vinaros@pay-in.es",
                    Phone = "000000000",
                    Birthday = new DateTime(2000, 8, 11),
                    TaxName = "M.I. Ajuntament Vinaros",
                    TaxNumber = "12345678A",
                    TaxAddress = "Vinaros"
                }
            );
            AddCommercePaymentRole(context, vinaros);
            AddPaymentWorkerRole(context, vinaros);

            //var pass = new List<string>() { "", "Ver238", "5iamuv", "ZOZinG", "W41oqn", "8HpdAC", "yam2c1", "Ed5MU6", "YRbhwl", "872cym", "KNGiT1", "ExSXT3", "OpxYGT", "PKrI85", "T70alZ", "CsGvP6", "bAqhtk", "xwlbkD", "JnBuaP", "etxv4k", "kNkcbN", "1TXVwj", "iPuPww", "SXfBbM", "dUBdYJ", "N3lZDY", "oPOSXp", "JZ1Zrk", "BbikNY", "LFOrkG", "TAofd1" };
            //for (int i = 1; i < 31; i++)
            //{
            //    CreateUser(context,
            //        new CreateUserDto
            //        {
            //            Login = $"gestor{i}@vinaros.es",
            //            Name = "Aj. Vinaros",
            //            Email = $"gestor{i}@vinaros.es",
            //            Phone = "000000000",
            //            Birthday = new DateTime(2000, 8, 11),
            //            TaxName = "Gestor",
            //            TaxNumber = "12345678A",
            //            TaxAddress = "Vinaros"
            //        }, pass[i]
            //    );
            //}

            #endregion

            #region Ciudadano

            CreateUser(context,
                new CreateUserDto
                {
                    Login = "ciudadano@pay-in.es",
                    Name = "Ciudadano",
                    Email = "ciudadano@pay-in.es",
                    Phone = "000000000",
                    Birthday = new DateTime(2000, 8, 11),
                    TaxName = "Ciudadano",
                    TaxNumber = "12345678A",
                    TaxAddress = "Vilamarxant"
                }
            );

            #endregion

            #region Fallero

            CreateUser(context,
                new CreateUserDto
                {
                    Login = "fallero@pay-in.es",
                    Name = "Fallero",
                    Email = "fallero@pay-in.es",
                    Phone = "000000000",
                    Birthday = new DateTime(2000, 8, 11),
                    TaxName = "Fallero",
                    TaxNumber = "12345678A",
                    TaxAddress = "Valencia"
                }
            );

            #endregion

            #region Club

            var club = CreateUser(context,
                new CreateUserDto
                {
                    Login = "club@pay-in.es",
                    Name = "Club",
                    Email = "club@pay-in.es",
                    Phone = "000000000",
                    Birthday = new DateTime(2000, 8, 11),
                    TaxName = "Club SL",
                    TaxNumber = "12345678A",
                    TaxAddress = "Valencia"
                }
            );
            AddCommercePaymentRole(context, club);

            #endregion

            #region ClubManager

            var clubManager = CreateUser(context,
                new CreateUserDto
                {
                    Login = "clubmanager@pay-in.es",
                    Name = "Club Manager",
                    Email = "clubmanager@pay-in.es",
                    Phone = "000000000",
                    Birthday = new DateTime(2000, 8, 11),
                    TaxName = "Club Manager SL",
                    TaxNumber = "12345678A",
                    TaxAddress = "Valencia"
                }
            );
            AddCommercePaymentRole(context, clubManager);
            AddPaymentWorkerRole(context, clubManager);

            #endregion

            #region Ciudadano

            var ahorrador = CreateUser(context,
                new CreateUserDto
                {
                    Login = "ahorrador@pay-in.es",
                    Name = "Ahorrador",
                    Email = "ahorrador@pay-in.es",
                    Phone = "000000000",
                    Birthday = new DateTime(1990, 8, 11),
                    TaxName = "Ahorrador",
                    TaxNumber = "12345678A",
                    TaxAddress = "Vilamarxant"
                }
            );

            #endregion

#endif

            base.Seed(context);
        }

        #endregion

        #region CreateUser

        public class CreateUserDto
        {
            public string Name { get; set; }
            public string Login { get; set; }
            public string Email { get; set; }
            public string Phone { get; set; }
            public DateTime? Birthday { get; set; }
            public string TaxName { get; set; }
            public string TaxNumber { get; set; }
            public string TaxAddress { get; set; }
        }

        public ApplicationUser CreateUser(AuthContext context, CreateUserDto arguments, string password = "Pa$$w0rd")
        {
            if (!context.Users.Any(x => x.UserName == arguments.Login))
            {
                var user = new ApplicationUser
                {
                    UserName = arguments.Login,
                    Email = arguments.Email,
                    Name = arguments.Name,
                    Mobile = arguments.Phone,
                    TaxName = arguments.TaxName,
                    TaxNumber = arguments.TaxNumber,
                    TaxAddress = arguments.TaxAddress,
                    Birthday = arguments.Birthday,
                    Sex = SexType.Undefined,
                    PhotoUrl = ""
                };
                var role = context.Roles
                    .FirstOrDefault(x => x.Name == AccountRoles.User);
                user.Roles.Add(new IdentityUserRole { UserId = user.Id, RoleId = role.Id });

                var userManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(context));
                userManager.UserValidator = new UserValidator<ApplicationUser>(userManager) { AllowOnlyAlphanumericUserNames = false };

                var result = userManager.Create(user, password);
                if ((!result.Succeeded) && (result.Errors != null))
                    throw new ApplicationException(string.Format("Identity system error: {0}", string.Join("\n", result.Errors)));

                user.EmailConfirmed = true;
                result = userManager.SetLockoutEnabled(user.Id, false);
                if ((!result.Succeeded) && (result.Errors != null))
                    throw new ApplicationException(string.Format("Identity system error: {0}", string.Join("\n", result.Errors)));

                context.SaveChanges();
            }

            return context.Users.First(x => x.UserName == arguments.Login);
        }

        #endregion

        #region AddCommercePaymentRole

        public void AddCommercePaymentRole(AuthContext context, ApplicationUser user)
        {
            var role = context.Roles
                .FirstOrDefault(x => x.Name == AccountRoles.CommercePayment);
            if (!user.Roles.Any(x => x.RoleId == role.Id))
                user.Roles.Add(new IdentityUserRole { UserId = user.Id, RoleId = role.Id });
        }

        #endregion

        #region AddPaymentWorkerRole

        public void AddPaymentWorkerRole(AuthContext context, ApplicationUser user)
        {
            var role = context.Roles
                .FirstOrDefault(x => x.Name == AccountRoles.PaymentWorker);
            if (!user.Roles.Any(x => x.RoleId == role.Id))
                user.Roles.Add(new IdentityUserRole { UserId = user.Id, RoleId = role.Id });
        }

        #endregion
    }
}
