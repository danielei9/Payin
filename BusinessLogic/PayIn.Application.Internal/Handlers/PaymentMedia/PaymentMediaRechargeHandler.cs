using PayIn.Application.Dto.Internal.Arguments;
using PayIn.BusinessLogic.Common;
using PayIn.Common;
using PayIn.Domain.Internal;
using PayIn.Domain.Internal.Infrastructure;
using System;
using System.Linq;
using System.Threading.Tasks;
using Xp.Application.Attributes;
using Xp.Application.Dto;
using Xp.Domain;


namespace PayIn.Application.Internal.Handlers
{
	[XpLog("PaymentMedia", "Pay")]
    public class PaymentMediaRechargeHandler :
		IServiceBaseHandler<PaymentMediaRechargeArguments>
    {
        private readonly UserCheckPinHandler UserCheckPin;
        private readonly ISessionData SessionData;
        private readonly IEntityRepository<PaymentMedia> Repository;
        private readonly IEntityRepository<User> RepositoryUser;
        private readonly IPaymentGatewayAdapter PaymentGatewayAdapter;

        #region Contructors
        public PaymentMediaRechargeHandler(
            UserCheckPinHandler userCheckPin,
            ISessionData sessionData,
            IEntityRepository<PaymentMedia> repository,
            IEntityRepository<User> repositoryUser,
            IPaymentGatewayAdapter paymentGatewayAdapter
        )
        {
            if (userCheckPin == null) throw new ArgumentNullException("userCheckPinHandler");
            if (sessionData == null) throw new ArgumentNullException("sessionData");
            if (repository == null) throw new ArgumentNullException("repository");
            if (repositoryUser == null) throw new ArgumentNullException("repositoryUser");
            if (paymentGatewayAdapter == null) throw new ArgumentNullException("paymentGatewayAdapter");

            UserCheckPin = userCheckPin;
            SessionData = sessionData;
            Repository = repository;
            RepositoryUser = repositoryUser;
            PaymentGatewayAdapter = paymentGatewayAdapter;
        }
        #endregion Contructors

        #region ExecuteAsync
        public async Task<dynamic> ExecuteAsync(PaymentMediaRechargeArguments arguments)
        {
            decimal quantity = arguments.Quantity;

            var user = (await RepositoryUser.GetAsync())
                .Where(x =>
                    x.Login == SessionData.Login
                )
                .FirstOrDefault();

            var paymentMedia = (await Repository.GetAsync("User"))
                .Where(x =>
                    x.PublicId == arguments.Purse &&
                    x.State == PaymentMediaState.Active /*&&
					x.User.Login == SessionData.Login*/
				)
                .FirstOrDefault();

            if (paymentMedia == null)
            {
                paymentMedia = new PaymentMedia
                {
                    Name = arguments.Name,
                    State = PaymentMediaState.Active,
                    Type = PaymentMediaType.Purse,
                    PublicId = arguments.Purse,
                    Number = "**** **** **** ****",
                    BankEntity = arguments.BankEntity,
                    Reference = "",
                    User = user,
                    Balance = quantity,
					Login = ""
                };
                await Repository.AddAsync(paymentMedia);
            }
            else
                paymentMedia.Balance += quantity;

            return paymentMedia;
        }
        #endregion ExecuteAsync
    }
}
