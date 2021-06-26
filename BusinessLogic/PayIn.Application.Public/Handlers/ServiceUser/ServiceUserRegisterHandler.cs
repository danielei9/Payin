using Microsoft.Practices.Unity;
using PayIn.Application.Dto.Arguments;
using PayIn.Application.Dto.Security.Arguments;
using PayIn.BusinessLogic.Common;
using PayIn.Common;
using PayIn.Domain.Public;
using PayIn.Infrastructure.Security;
using System;
using System.Linq;
using System.Threading.Tasks;
using Xp.Application.Dto;
using Xp.Domain;
using Xp.Infrastructure;

namespace PayIn.Application.Public.Handlers
{
	public class ServiceUserRegisterHandler :
		IServiceBaseHandler<ServiceUserRegisterArguments>
	{
		[Dependency] public IEntityRepository<ServiceUser> Repository { get; set; }
		[Dependency] public SecurityRepository SecurityRepository { get; set; }
		[Dependency] public ISessionData SessionData { get; set; }
		[Dependency] public IEntityRepository<ServiceConcession> ServiceConcessionRepository { get; set; }
		[Dependency] public IEntityRepository<SystemCard> SystemCardRepository { get; set; }

		#region ExecuteAsync
		public async Task<dynamic> ExecuteAsync(ServiceUserRegisterArguments arguments)
		{
			var serviceUser = await Repository.GetAsync(arguments.Id);
			if (serviceUser == null)
				throw new ArgumentNullException("Id");

			await RegisterAsync(serviceUser);

            return new { serviceUser.Id };
		}
		#endregion ExecuteAsync

		#region RegisterAsync
		public async Task<dynamic> RegisterAsync(ServiceUser serviceUser)
		{
			var concessionUser = await SecurityRepository.GetUserAsync(SessionData.Login);
			var concessionPhotoUrl = concessionUser?.PhotoUrl ?? "";
			var concessionName = concessionUser?.Name ?? "";

			string emailText = "";
			// ServiceConcession
			var serviceConcession = (await ServiceConcessionRepository.GetAsync())
				.Where(x =>
					x.Supplier.Login == SessionData.Login &&
					x.State == ConcessionState.Active
				)
				.FirstOrDefault();

			if (serviceConcession != null)
			{
				var systemCard = (await SystemCardRepository.GetAsync())
					.Where(x =>
						x.ConcessionOwnerId == serviceConcession.Id
					)
					.FirstOrDefault();
				if (systemCard != null)
					emailText = systemCard.AffiliationEmailBody ?? "";
			}

			var user = await SecurityRepository.InviteUserAsync(
				new AccountRegisterArguments
				{
					UserName = serviceUser.Email,
					Name = serviceUser.Name + " " + serviceUser.LastName,
					Mobile = serviceUser.Phone?.ToString() ?? "",
					TaxName = serviceUser.Name + " " + serviceUser.LastName ?? "",
					isBussiness = UserType.User,
				},
				concessionPhotoUrl,
				concessionName,
				emailText
			);

			return null;
		}
		#endregion RegisterAsync
	}
}
