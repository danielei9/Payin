using PayIn.Application.Dto.Internal.Arguments;
using PayIn.BusinessLogic.Common;
using PayIn.Common;
using PayIn.Common.Security;
using PayIn.Domain.Internal;
using System;
using System.Linq;
using System.Threading.Tasks;
using Xp.Application.Attributes;
using Xp.Application.Dto;
using Xp.Domain;
using Xp.Infrastructure.Mail;

namespace PayIn.Application.Internal.Handlers
{
    [XpLog("User", "CheckPin")]
    public class UserCheckPinHandler :
		IServiceBaseHandler<UserCheckPinArguments> 
	{
		private readonly MailService MailService;
		private readonly SessionData SessionData;
		private readonly IEntityRepository<User> Repository;
		private readonly IUnitOfWork UnitOfWork;

		#region Construtors
		public UserCheckPinHandler(
			MailService mailService,
			SessionData sessionData,
			IEntityRepository<User> repository,
			IUnitOfWork unitOfWork
		)
		{
			if (mailService == null) throw new ArgumentNullException("mailService");
			if (sessionData == null) throw new ArgumentNullException("sessionData");
			if (repository == null) throw new ArgumentNullException("repository");
			if (unitOfWork == null) throw new ArgumentNullException("unitOfWork");

			MailService = mailService;
			SessionData = sessionData;
			Repository = repository;
			UnitOfWork = unitOfWork;
		}
		#endregion Construtors

		#region ExecuteAsync
		public async Task<dynamic> ExecuteAsync(UserCheckPinArguments arguments)
		{
			var pin = arguments.Pin.ToHash();
			var item = (await Repository.GetAsync())
				.Where(x =>
					x.Login == SessionData.Login &&
					x.State != UserState.Blocked
				)
				.FirstOrDefault();
			if (item == null)
				throw new ApplicationException(SecurityResources.AccountBlockedBefore);

			if (item.Pin == pin)
			{
				item.PinRetries = 0;
				return true;
			}
			else
			{
				item.PinRetries++;
				if (item.PinRetries == 3)
				{
					item.State = UserState.Blocked;
					await UnitOfWork.SaveAsync();
					MailService.Send(SessionData.Login, SecurityResources.HeadMessageError, SecurityResources.UserBlocked);
					item.PinRetries = 0;
					throw new ArgumentException(SecurityResources.UserBlocked, "pin");
				}
				else if (item.PinRetries > 3)
				{
					await UnitOfWork.SaveAsync();
					throw new ArgumentException(SecurityResources.UserBlocked, "pin");
				}
				return false;
			}
		}
		#endregion ExecuteAsync
	}
}
