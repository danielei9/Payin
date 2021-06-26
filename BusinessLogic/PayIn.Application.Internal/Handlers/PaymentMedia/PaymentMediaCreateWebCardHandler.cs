using PayIn.Application.Dto.Internal.Arguments;
using PayIn.Application.Dto.Internal.Results;
using PayIn.BusinessLogic.Common;
using PayIn.Common;
using PayIn.Common.Resources;
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
	[XpLog("PaymentMedia", "CreateWebCard")]
	public class PaymentMediaCreateWebCardHandler :
		IServiceBaseHandler<PaymentMediaCreateWebCardArguments>
	{
		public readonly UserCheckPinHandler UserCheckPin;
		public readonly IUnitOfWork UnitOfWork;
		public readonly ISessionData SessionData;
		public readonly IEntityRepository<PaymentMedia> Repository;
		public readonly IEntityRepository<User> RepositoryUser;
		public readonly IPaymentGatewayAdapter PaymentGatewayAdapter;

		#region Contructors
		public PaymentMediaCreateWebCardHandler(
			UserCheckPinHandler userCheckPin,
			IUnitOfWork unitOfWork,
			ISessionData sessionData,
			IEntityRepository<PaymentMedia> repository,
			IEntityRepository<User> repositoryUser,
			IPaymentGatewayAdapter paymentGatewayAdapter
		)
		{
			if (userCheckPin == null) throw new ArgumentNullException("userCheckPin");
			if (sessionData == null) throw new ArgumentNullException("sessionData");
			if (unitOfWork == null) throw new ArgumentNullException("unitOfWork");
			if (repository == null) throw new ArgumentNullException("repository");
			if (repositoryUser == null) throw new ArgumentNullException("repositoryUser");
			if (paymentGatewayAdapter == null) throw new ArgumentNullException("paymentGatewayAdapter");
			
			UserCheckPin = userCheckPin;
			SessionData = sessionData;
			UnitOfWork = unitOfWork;
			Repository = repository;
			RepositoryUser = repositoryUser;
			PaymentGatewayAdapter = paymentGatewayAdapter;
		}
		#endregion Contructors

		#region ExecuteAsync
		public async Task<dynamic> ExecuteAsync(PaymentMediaCreateWebCardArguments arguments)
		{
			var user = (await RepositoryUser.GetAsync())
				.Where(x =>
					x.Login == SessionData.Login
				)
				.FirstOrDefault();
			if (user == null)
				throw new ArgumentNullException("Login");

			//Check Pin
			if (!await UserCheckPin.ExecuteAsync(new UserCheckPinArguments(arguments.Pin)))
				throw new ArgumentException(UserResources.IncorrectPin, "pin");

			var paymentMedia = new PaymentMedia
			{
				Name = arguments.Name,
				Type = PaymentMediaType.WebCard,
				Number = "**** **** **** ****",
				PublicId = arguments.PublicPaymentMediaId,
				State = PaymentMediaState.Pending,
				BankEntity = arguments.BankEntity,
				Reference = "",
				User = user,
				Login = arguments.Login
			};
			await Repository.AddAsync(paymentMedia);
			await UnitOfWork.SaveAsync();

			var request = await PaymentGatewayAdapter.WebCardRequestAsync(
				arguments.CommerceCode,
				paymentMedia.Id, arguments.PublicPaymentMediaId,
                arguments.PublicTicketId, arguments.PublicPaymentId,
                arguments.OrderId, arguments.PaymentMediaCreateType, arguments.Amount
            );
			return new PaymentMediaCreateWebCardResult
			{
				Id = paymentMedia.Id,
				TicketId = arguments.PublicTicketId,
				Verb = "POST",
#if PRODUCTION
				Url = "https://sis.redsys.es/sis/realizarPago",
#else
				Url = "https://sis-t.redsys.es:25443/sis/realizarPago",
#endif
				Request = request,
				Arguments = request
					.SplitString("&")
					.ToDictionary(
						x => x.RightError(x.IndexOf(":")),
						x => x.LeftError(x.Length - x.IndexOf(":") - 1)
					)
			};
		}
#endregion ExecuteAsync
	}
}
