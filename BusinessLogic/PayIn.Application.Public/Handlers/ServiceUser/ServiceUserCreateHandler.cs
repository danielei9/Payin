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
using Xp.Infrastructure.Repositories;

namespace PayIn.Application.Public.Handlers
{
	public class ServiceUserCreateHandler : IServiceBaseHandler<ServiceUserCreateArguments>
	{
		[Dependency] public IUnitOfWork UnitOfWork { get; set; }
		[Dependency] public ISessionData SessionData { get; set; }
		[Dependency] public IEntityRepository<ServiceUser> Repository { get; set; }
		[Dependency] public IEntityRepository<ServiceConcession> RepositoryServiceConcession { get; set; }
		[Dependency] public SecurityRepository SecurityRepository { get; set; }
		[Dependency] public ServiceUserRegisterHandler ServiceUserRegisterHandler { get; set; }
		[Dependency] public IEntityRepository<ServiceConcession> ServiceConcessionRepository { get; set; }
		[Dependency] public IEntityRepository<SystemCard> SystemCardRepository { get; set; }

		#region ExecuteAsync
		public async Task<dynamic> ExecuteAsync(ServiceUserCreateArguments arguments)
		{
			// ServiceConcession
			var concession = (await RepositoryServiceConcession.GetAsync())
				.Where(x =>
					x.Supplier.Login == SessionData.Login &&
					x.State == ConcessionState.Active
				)
				.Select(x => new
				{
					x.Id,
					x.Supplier.Login,
					x.Name
				})
				.FirstOrDefault();
			if (concession == null)
				throw new ArgumentNullException("Login");

			var concessionUser = await SecurityRepository.GetUserAsync(SessionData.Login);
			var concessionPhotoUrl = concessionUser?.PhotoUrl ?? "";
			var concessionName = concessionUser?.Name ?? "";

			// ServiceUser
			var serviceUser = new ServiceUser
			{
				VatNumber = arguments.VatNumber,
				Name = arguments.Name,
				LastName = arguments.LastName,
				Login = arguments.Email,
				State = ServiceUserState.Active,
				Phone = arguments.Phone,
				Email = arguments.Email,
				BirthDate = arguments.BirthDate,
				Address = arguments.Address,
				ConcessionId = concession.Id,
				AssertDoc = "",
				//AssertDocument = null,
				Code = arguments.Code,
				Observations = arguments.Observations,
				Photo = ""
			};
			await Repository.AddAsync(serviceUser);

			//// Emitir tarjeta
			//if (serviceCard.OwnerUserId == null)
			//	serviceCard.OwnerUser = serviceUser;
			await UnitOfWork.SaveAsync();

			// Save doc in Azure
			var repositoryAzure = new AzureBlobRepository();
			var guid = Guid.NewGuid();

			// El usuario ya ha sido creado. Ahora se enviará un mail con el contenido del campo SystemCard.AffiliationEmailBody
			if (!serviceUser.Email.IsNullOrEmpty())
			{
				var user = await SecurityRepository.GetUserAsync(serviceUser.Email);
				if (user != null)
					return serviceUser;

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

				await SecurityRepository.InviteUserAsync(
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
			}

			return serviceUser;
		}
#endregion ExecuteAsync
	}
}
