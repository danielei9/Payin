using PayIn.Application.Dto.Arguments;
using PayIn.Application.Dto.Results;
using PayIn.BusinessLogic.Common;
using PayIn.Common;
using PayIn.Domain.Public;
using System;
using System.Linq;
using System.Threading.Tasks;
using Xp.Application.Dto;
using Xp.Domain;

namespace PayIn.Application.Public.Handlers
{
	public class ServiceUserUpdateCardGetHandler : 
		IQueryBaseHandler<ServiceUserUpdateCardGetArguments, ServiceUserUpdateCardGetResult>
	{
		private readonly ISessionData SessionData;
		private readonly IEntityRepository<ServiceUser> Repository;
		private readonly IEntityRepository<ServiceCard> RepositoryServiceCard;
		private readonly IEntityRepository<ServiceConcession> RepositoryServiceConcession;

		#region Constructors
		public ServiceUserUpdateCardGetHandler(
			ISessionData sessionData,
			IEntityRepository<ServiceUser> repository,
			IEntityRepository<ServiceCard> repositoryServiceCard,
			IEntityRepository<ServiceConcession> repositoryServiceConcession
		)
		{
			if (sessionData == null) throw new ArgumentNullException("sessionData");
			if (repository == null) throw new ArgumentNullException("repository");
			if (repositoryServiceCard == null) throw new ArgumentNullException("repositoryServiceCard");
			if (repositoryServiceConcession == null) throw new ArgumentNullException("repositoryServiceConcession");

			SessionData = sessionData;
			Repository = repository;
			RepositoryServiceCard = repositoryServiceCard;
			RepositoryServiceConcession = repositoryServiceConcession;
		}
		#endregion Constructors

		#region ExecuteAsync
		public async Task<ResultBase<ServiceUserUpdateCardGetResult>> ExecuteAsync(ServiceUserUpdateCardGetArguments arguments)
		{
			// Cargando concession
			var concession = (await RepositoryServiceConcession.GetAsync())
				.Where(x =>
					x.Supplier.Login == SessionData.Login &&
					x.State == ConcessionState.Active
				)
				.Select(x => new { x.Id })
				.FirstOrDefault();

			// Service user
			var serviceUser1 = (await Repository.GetAsync())
				.Where(x => x.Id == arguments.Id)
				.Select(x => new
				{
					Id = x.Id,
					Card = x.Card,
					ConcessionId = x.ConcessionId,
				})
				.FirstOrDefault();

			if (serviceUser1 == null)
			{
				throw new ApplicationException("No existe una tarjeta original que deba ser modificada");
			}			

			var serviceUser = (await Repository.GetAsync())
				.Where(x => x.Id == arguments.Id)
				.Select(x => new
				{
					Id = x.Id,
					ConcessionId = x.ConcessionId,
					Uid = x.Card.Uid,
					CardState = x.Card.State,
					SystemCardId = x.Card.SystemCardId,
					CardConcessionId = x.Card.ConcessionId,
					CardConcessionName = x.Card.Concession.Name
				})
				.FirstOrDefault();
		
						// Cambio de tarjeta
			if (serviceUser.Uid != arguments.Uid)
			{
				var serviceCard = (await RepositoryServiceCard.GetAsync())
					.Where(x =>
						x.Uid == arguments.Uid &&
						x.State == ServiceCardState.Active &&
						x.SystemCardId == serviceUser.SystemCardId
					)
					.Select(x => new
					{
						x.Id,
						x.State,
						x.ConcessionId,
						x.OwnerUserId,
						OwnerVatNumber = x.OwnerUser.VatNumber,
						OwnerName = x.OwnerUser.Name,
						OwnerLastName = x.OwnerUser.LastName,
						ConcessionName = x.Concession.Name,
						UserCount = x.Users
							.Where(y => y.ConcessionId == concession.Id)
							.Count()
					})
					.FirstOrDefault();
				if (serviceCard == null)
					throw new ApplicationException("El identificador de la tarjeta no está dentro del sistema de tarjetas");
				if (
					(serviceCard.OwnerUserId != null) && // Tarjeta emitida
					(serviceCard.UserCount > 0) // Tarjeta propia utilizada
				)
					throw new ApplicationException("La tarjeta seleccionada ya está asignada por esta misma organización");

				return new ResultBase<ServiceUserUpdateCardGetResult>
				{
					Data = new ServiceUserUpdateCardGetResult[] {
						new ServiceUserUpdateCardGetResult
						{
							Id = serviceUser.Id,
							Uid = arguments.Uid,
							CardState = serviceCard.State,
							NewCardConcessionName = serviceCard.ConcessionName,
							NewCard = (serviceCard.OwnerUserId == null),
							OwnCard = (serviceCard.OwnerUserId == null) || (serviceCard.ConcessionId == concession.Id),
							OwnerVatNumber = serviceCard.OwnerVatNumber,
							OwnerName = serviceCard.OwnerName,
							OwnerLastName = serviceCard.OwnerLastName
						}
					}
				};
			}

			return new ResultBase<ServiceUserUpdateCardGetResult> {
				Data = new ServiceUserUpdateCardGetResult[] { 
					new ServiceUserUpdateCardGetResult
					{
						Id = serviceUser.Id,
						Uid = arguments.Uid,
						CardState = serviceUser.CardState,
						NewCardConcessionName = serviceUser.CardConcessionName,
						NewCard = false,
						OwnCard = (serviceUser.CardConcessionId == concession.Id)
					}
				}
			};
		}
		#endregion ExecuteAsync
	}
}
