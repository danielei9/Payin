using PayIn.Application.Dto.Payments.Arguments;
using PayIn.BusinessLogic.Common;
using PayIn.Domain.Payments.Infrastructure;
using System;
using System.Threading.Tasks;
using Xp.Application.Dto;

namespace PayIn.Application.Payments.Handlers.User
{
	public class UserMobileHasPaymentHandler :
		IServiceBaseHandler<UserMobileHasPaymentArguments>
	{
		public ISessionData SessionData { get; set; }
		public IInternalService InternalService { get; set; }

		#region Constructors
		public UserMobileHasPaymentHandler(
			ISessionData sessionData,
			IInternalService internalService
		)
		{
			if (sessionData == null) throw new ArgumentNullException("sessionData");
			if (internalService == null) throw new ArgumentNullException("internalService");

			SessionData = sessionData;
			InternalService = internalService;
		}
		#endregion Constructors

		#region ExecuteAsync
		public async Task<dynamic> ExecuteAsync(UserMobileHasPaymentArguments arguments)
		{
			return await InternalService.UserHasPaymentAsync();
		}
		#endregion ExecuteAsync
	}
}
