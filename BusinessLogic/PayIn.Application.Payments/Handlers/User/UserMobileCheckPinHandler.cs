using PayIn.Application.Dto.Payments.Arguments.User;
using PayIn.BusinessLogic.Common;
using PayIn.Domain.Payments.Infrastructure;
using System;
using System.Threading.Tasks;
using Xp.Application.Dto;

namespace PayIn.Application.Payments.Handlers.User
{
	public class UserMobileCheckPinHandler : 
		IServiceBaseHandler<UserMobileCheckPinArguments>
	{
		public ISessionData SessionData { get; set; }
		public IInternalService InternalService  { get; set; }

		#region Constructors
		public UserMobileCheckPinHandler(
			ISessionData sessionData,
			IInternalService internalService
		)
		{
			if (sessionData == null)
				throw new ArgumentNullException("sessionData");
			SessionData = sessionData;

			if (internalService == null)
				throw new ArgumentNullException("internalService");
			InternalService = internalService;
		}
		#endregion Constructors

		#region ExecuteAsync
		public async Task<dynamic> ExecuteAsync(UserMobileCheckPinArguments arguments)
		{
			var result = await InternalService.UserCheckPin(SessionData.Login, arguments.Pin);
			return result;
		}
		#endregion ExecuteAsync
	}
}
