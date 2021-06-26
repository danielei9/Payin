using PayIn.Application.Dto.Internal.Arguments;
using PayIn.Application.Dto.Internal.Results;
using PayIn.BusinessLogic.Common;
using PayIn.Common;
using PayIn.Common.Resources;
using PayIn.Domain.Internal;
using PayIn.Domain.Internal.Infrastructure;
using PayIn.Infrastructure.Sabadell;
using System;
using System.Linq;
using System.Threading.Tasks;
using Xp.Application.Attributes;
using Xp.Application.Dto;
using Xp.Domain;

namespace PayIn.Application.Internal.Handlers
{
	[XpLog("PaymentMedia", "Pay")]
	public class PaymentMediaPayHandler :
		IServiceBaseHandler<PaymentMediaPayArguments>
	{
		private readonly UserCheckPinHandler UserCheckPin;
		private readonly ISessionData SessionData;
		private readonly IEntityRepository<PaymentMedia> Repository;
		private readonly IEntityRepository<User> RepositoryUser;
		private readonly IPaymentGatewayAdapter PaymentGatewayAdapter;

		#region Contructors
		public PaymentMediaPayHandler(
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
		public async Task<dynamic> ExecuteAsync(PaymentMediaPayArguments arguments)
		{						
			var userExist = (await RepositoryUser.GetAsync())
				.Where(x =>
					x.Login == SessionData.Login
				)
				.Any();
			if (!userExist)
				throw new UnauthorizedAccessException();

			// Check Pin
			if (!await UserCheckPin.ExecuteAsync(new UserCheckPinArguments(arguments.Pin)))
				throw new ArgumentException(UserResources.IncorrectPin,"pin");
			
			var paymentMedia = (await Repository.GetAsync("User"))
				.Where(x =>
					x.PublicId == arguments.PublicPaymentMediaId &&
					x.State == PaymentMediaState.Active &&
					x.Login == arguments.Login &&
					x.User.Login == SessionData.Login
				)
				.FirstOrDefault();
			if (paymentMedia == null)
				throw new ArgumentNullException("publicPaymentMediaId");

            dynamic result;
            switch (paymentMedia.Type)
            {
                case PaymentMediaType.WebCard:
                    var data = await PaymentGatewayAdapter.PayAsync(paymentMedia.Id, arguments.PublicPaymentMediaId, arguments.PublicTicketId, arguments.PublicPaymentId, paymentMedia.Reference, arguments.Order, arguments.Amount);
                    result = SabadellGatewayFunctions.GetPaymentResponse(data);
                    break;
                default:
                    result = new PaymentMediaPayResult() { Amount = (int)(paymentMedia.Balance * 100) - (int)(arguments.Amount * 100) };
                    paymentMedia.Balance = paymentMedia.Balance - arguments.Amount;
					break;
            }
			
			return result;
		}
		#endregion ExecuteAsync
	}
}
