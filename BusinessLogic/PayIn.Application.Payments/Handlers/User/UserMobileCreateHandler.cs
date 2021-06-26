using PayIn.Application.Dto.Payments.Arguments;
using PayIn.BusinessLogic.Common;
using PayIn.Domain.Payments.Infrastructure;
using PayIn.Infrastructure.Security;
using System;
using System.Threading.Tasks;
using Xp.Application.Attributes;
using Xp.Application.Dto;

namespace PayIn.Application.Payments.Handlers.User
{
	[XpLog("User", "Create")]
	[XpAnalytics("User", "Create")]
	public class UserMobileCreateHandler : 
		IServiceBaseHandler<UserMobileCreateArguments>
	{
		public ISessionData SessionData { get; set; }
		public IInternalService InternalService  { get; set; }

		#region Constructors
		public UserMobileCreateHandler(
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
		public async Task<dynamic> ExecuteAsync(UserMobileCreateArguments arguments)
		{
			await InternalService.UserCreateAsync(arguments.Pin);

			var securityRepository = new SecurityRepository();
			await securityRepository.UpdateTaxDataAsync(
				SessionData.Login,
				arguments.TaxNumber,
				arguments.TaxName,
				arguments.TaxAddress,
				arguments.Birthday
			);
			return null;
		}
		#endregion ExecuteAsync
	}
}
