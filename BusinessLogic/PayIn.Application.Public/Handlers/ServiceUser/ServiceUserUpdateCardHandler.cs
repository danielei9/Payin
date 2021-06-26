using Microsoft.Practices.Unity;
using PayIn.Application.Dto.Arguments;
using PayIn.BusinessLogic.Common;
using PayIn.Common;
using PayIn.Common.Resources;
using PayIn.Domain.Public;
using PayIn.Domain.Transport;
using System;
using System.Linq;
using System.Threading.Tasks;
using Xp.Application.Dto;
using Xp.Domain;
using Xp.Infrastructure.Repositories;

namespace PayIn.Application.Public.Handlers
{
	public class ServiceUserUpdateCardHandler : 
		IServiceBaseHandler<ServiceUserUpdateCardArguments>
	{
		[Dependency] public ISessionData SessionData { get; set; }
		[Dependency] public IEntityRepository<ServiceUser> Repository { get; set; }
		[Dependency] public IEntityRepository<ServiceCard> RepositoryServiceCard { get; set; }
		[Dependency] public IEntityRepository<ServiceConcession> RepositoryServiceConcession { get; set; }
		[Dependency] public IEntityRepository<GreyList> RepositoryGreyList { get; set; }
		
		#region ExecuteAsync
		public async Task<dynamic> ExecuteAsync(ServiceUserUpdateCardArguments arguments)
		{
			var concession = (await RepositoryServiceConcession.GetAsync())
				.Where(x =>
					x.Supplier.Login == SessionData.Login &&
					x.State == ConcessionState.Active
				)
				.Select(x => new { x.Id })
				.FirstOrDefault();

			var serviceUser = (await Repository.GetAsync("Card"))
				.Where(x => x.Id == arguments.Id)
				.FirstOrDefault();

			// Cambio de tarjeta
			if (serviceUser.Card.Uid != arguments.Uid)
			{
				var oldCardUid = serviceUser.Card.Uid;
				var newCardUid = arguments.Uid;

				var serviceCard = (await RepositoryServiceCard.GetAsync("OwnerUser"))
					.Where(x =>
						x.Uid == newCardUid &&
						x.State == ServiceCardState.Active &&
						x.SystemCardId == serviceUser.Card.SystemCardId
					)
					.FirstOrDefault();
				if (serviceCard == null)
					throw new ApplicationException("El identificador de la tarjeta no está dentro del sistema de tarjetas");
				
				//if (serviceCard.OwnerUser != null)
				//	throw new ApplicationException("La tarjeta seleccionada ya está asignada a otra persona");

				var myMembersCount = serviceCard.Users
					.Where(y => y.ConcessionId == concession.Id)
					.Count();
				if (myMembersCount > 0)
					throw new ApplicationException("La tarjeta seleccionada ya está asignada");

				serviceUser.Card = serviceCard;
				serviceUser.VatNumber = serviceCard.OwnerUser.VatNumber;
				serviceUser.Name = serviceCard.OwnerUser.Name;
				serviceUser.LastName = serviceCard.OwnerUser.LastName;
				serviceUser.Photo = serviceCard.OwnerUser.Photo;

				var now = DateTime.Now.ToUTC();
				foreach(ServiceGroup serviceGroup in serviceUser.Groups)
				{
					// Añadir los grupos de la persona a la LG para la nueva tarjeta primaria, y eliminarlos de la tarjeta primaria anterior (ahora secundaria)
					var addGroup = new GreyList
					{
						Uid = serviceCard.Uid,
						RegistrationDate = now,
						Action = PayIn.Domain.Transport.GreyList.ActionType.IncreaseUnities,
						Field = "GI",
						NewValue = serviceGroup.Id.ToString(),
						Resolved = false,
						ResolutionDate = null,
						Machine = PayIn.Domain.Transport.GreyList.MachineType.All,
						Source = PayIn.Domain.Transport.GreyList.GreyListSourceType.Payin,
						State = GreyList.GreyListStateType.Active
					};
					await RepositoryGreyList.AddAsync(addGroup);

					// Enviar orden para borrar los grupos de las tarjetas secundarias
					var removeGroup = new GreyList
					{
						Uid = oldCardUid,
						RegistrationDate = now,
						Action = PayIn.Domain.Transport.GreyList.ActionType.DiscountUnities,
						Field = "GI",
						NewValue = serviceGroup.Id.ToString(),
						Resolved = false,
						ResolutionDate = null,
						Machine = PayIn.Domain.Transport.GreyList.MachineType.All,
						Source = PayIn.Domain.Transport.GreyList.GreyListSourceType.Payin,
						State = GreyList.GreyListStateType.Active
					};
					await RepositoryGreyList.AddAsync(addGroup);
				}

				// Cambiar la anterior tarjeta por secundaria, y la nueva tarjeta por primaria
				var setSecondaryCard = new GreyList
				{
					Uid = oldCardUid,
					RegistrationDate = now,
					Action = PayIn.Domain.Transport.GreyList.ActionType.IncreaseUnities,
					Field = "TYPE",
					NewValue = "2",
					Resolved = false,
					ResolutionDate = null,
					Machine = PayIn.Domain.Transport.GreyList.MachineType.All,
					Source = PayIn.Domain.Transport.GreyList.GreyListSourceType.Payin,
					State = GreyList.GreyListStateType.Active
				};
				await RepositoryGreyList.AddAsync(setSecondaryCard);

				var setPrimaryCard = new GreyList
				{
					Uid = newCardUid,
					RegistrationDate = now,
					Action = PayIn.Domain.Transport.GreyList.ActionType.IncreaseUnities,
					Field = "TYPE",
					NewValue = "1",
					Resolved = false,
					ResolutionDate = null,
					Machine = PayIn.Domain.Transport.GreyList.MachineType.All,
					Source = PayIn.Domain.Transport.GreyList.GreyListSourceType.Payin,
					State = GreyList.GreyListStateType.Active
				};
				await RepositoryGreyList.AddAsync(setPrimaryCard);
			}

			if (serviceUser.Card.ConcessionId == serviceUser.ConcessionId) // Tarjeta propia
				serviceUser.Card.State = arguments.CardState;
			if (serviceUser.Card.OwnerUserId == null) // Emitir tarjeta
			{
				serviceUser.Card.State = arguments.CardState;
				serviceUser.Card.ConcessionId = serviceUser.ConcessionId;
				serviceUser.Card.OwnerUserId = serviceUser.Id;
			}
			return serviceUser;
		}
		#endregion ExecuteAsync
	}
}
