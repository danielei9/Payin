using PayIn.Application.Dto.Internal.Arguments;
using PayIn.BusinessLogic.Common;
using PayIn.Common;
using PayIn.Domain.Internal;
using System;
using System.Linq;
using System.Threading.Tasks;
using Xp.Application.Attributes;
using Xp.Application.Dto;
using Xp.Domain;

namespace PayIn.Application.Internal.Handlers
{
	[XpLog("User", "ForgotPin")]
    public class UserForgotPinHandler :
        IServiceBaseHandler<UserForgotPinArguments>
    {
        private readonly SessionData SessionData;
        private readonly IEntityRepository<User> Repository;
        private readonly IEntityRepository<PaymentMedia> PaymentMediaRepository;

        #region Construtors
        public UserForgotPinHandler(
            SessionData sessionData,
            IEntityRepository<User> repository,
            IEntityRepository<PaymentMedia> repositoryPaymentMedia
        )
        {
            if (sessionData == null) throw new ArgumentNullException("sessionData");
            if (repository == null) throw new ArgumentNullException("repository");
            if (repositoryPaymentMedia == null) throw new ArgumentNullException("repository");

            SessionData = sessionData;
            PaymentMediaRepository = repositoryPaymentMedia;
            Repository = repository;
        }
        #endregion Construtors

        #region ExecuteAsync
        public async Task<dynamic> ExecuteAsync(UserForgotPinArguments arguments)
        {
           //Comprobamos pin
            var item = (await Repository.GetAsync())
                .Where(x => x.Login == SessionData.Login)
                .FirstOrDefault();
            if (item == null)
                throw new ArgumentNullException("login");

            item.Pin = arguments.Pin.ToHash();

            int userId = item.Id;

            var items = (await PaymentMediaRepository.GetAsync("User"))
                .Where(x => 
                    x.State != PaymentMediaState.Delete &&
                    x.State != PaymentMediaState.Error &&
                    x.UserId == userId &&
					x.User.Login == SessionData.Login
				)
                .ToList();

            foreach (var res in items)
            {
                await PaymentMediaRepository.DeleteAsync(res);
            }

            return item;
        }
        #endregion ExecuteAsync
    }
}
