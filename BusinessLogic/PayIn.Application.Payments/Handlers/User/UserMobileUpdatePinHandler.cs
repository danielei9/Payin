using PayIn.Application.Dto.Payments.Arguments;
using PayIn.BusinessLogic.Common;
using PayIn.Common.Resources;
using PayIn.Domain.Payments.Infrastructure;
using System;
using System.Threading.Tasks;
using Xp.Application.Attributes;
using Xp.Application.Dto;

namespace PayIn.Application.Payments.Handlers.User
{
	[XpLog("User", "Update")]
	public class UserMobileUpdatePinHandler : 
		IServiceBaseHandler<UserMobileUpdatePinArguments>
	{
		public ISessionData SessionData { get; set; }
		public IInternalService InternalService  { get; set; }

		#region Constructors
		public UserMobileUpdatePinHandler(
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
		public async Task<dynamic> ExecuteAsync(UserMobileUpdatePinArguments arguments)
		{
			if (arguments.Pin != arguments.ConfirmPin)
				throw new ArgumentException(UserResources.ConfirmPin, "confirmPin");

			await InternalService.UserUpdatePinAsync(arguments.OldPin, arguments.Pin, arguments.ConfirmPin);
			return null;
		}
		#endregion ExecuteAsync
	}
}
