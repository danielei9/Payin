using PayIn.Application.Dto.Payments.Arguments;
using PayIn.BusinessLogic.Common;
using PayIn.Common;
using PayIn.Common.Security;
using PayIn.Domain.Payments;
using PayIn.Domain.Payments.Infrastructure;
using PayIn.Infrastructure.Security;
using System;
using System.Linq;
using System.Threading.Tasks;
using Xp.Application.Attributes;
using Xp.Application.Dto;
using Xp.Domain;

namespace PayIn.Application.Payments.Handlers.User
{
	[XpLog("User", "ForgotPin")]
    public class UserMobileForgotPinHandler :
        IServiceBaseHandler<UserMobileForgotPinArguments>
    {
        public ISessionData SessionData { get; set; }
        public IInternalService InternalService { get; set; }
        public IEntityRepository<PaymentMedia> RepositoryPaymentMediaMobile;

        #region Constructors
        public UserMobileForgotPinHandler(
            ISessionData sessionData,
            IInternalService internalService,
            IEntityRepository<PaymentMedia> repository
        )
        {
            if (sessionData == null) throw new ArgumentNullException("sessionData");
            if (internalService == null) throw new ArgumentNullException("internalService");
            if (repository == null) throw new ArgumentNullException("repository");

            SessionData = sessionData;
            InternalService = internalService;
            RepositoryPaymentMediaMobile = repository;
        }
        #endregion Constructors

        #region ExecuteAsync
        public async Task<dynamic> ExecuteAsync(UserMobileForgotPinArguments arguments)
        {
            var securityRep = new SecurityRepository();
            var item = await securityRep.GetUserAsync(SessionData.Login, arguments.Password);
			if (item == null)
			    throw new ApplicationException(SecurityResources.ErrorPassword);

			var resultUser = await InternalService.UserForgotPinAsync(arguments.Pin);

            var items = (await RepositoryPaymentMediaMobile.GetAsync())
			.Where(x =>
				x.State != PaymentMediaState.Error &&
				x.State != PaymentMediaState.Delete &&
				x.Login == SessionData.Login
			);

            foreach (var res in items)
                res.State = Common.PaymentMediaState.Delete;

            return null;
        }
        #endregion ExecuteAsync
    }
}
