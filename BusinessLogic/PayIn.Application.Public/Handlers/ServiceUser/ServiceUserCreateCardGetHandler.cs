using PayIn.Application.Dto.Arguments;
using PayIn.Application.Dto.Results;
using PayIn.BusinessLogic.Common;
using PayIn.Common;
using PayIn.Common.Resources;
using PayIn.Domain.Public;
using System;
using System.Linq;
using System.Threading.Tasks;
using Xp.Application.Dto;
using Xp.Domain;
using Xp.Infrastructure.Repositories;

namespace PayIn.Application.Public.Handlers
{
	public class ServiceUserCreateCardGetHandler : 
		IQueryBaseHandler<ServiceUserCreateCardGetArguments, ServiceUserCreateCardGetResult>
	{
		private readonly ISessionData SessionData;
		private readonly IEntityRepository<ServiceUser> Repository;
		private readonly IEntityRepository<ServiceCard> RepositoryServiceCard;
		private readonly IEntityRepository<ServiceConcession> RepositoryServiceConcession;

		#region Constructors
		public ServiceUserCreateCardGetHandler(
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
		public async Task<ResultBase<ServiceUserCreateCardGetResult>> ExecuteAsync(ServiceUserCreateCardGetArguments arguments)
		{
			// Cargando concession
			var concession = (await RepositoryServiceConcession.GetAsync())
				.Where(x =>
					x.Supplier.Login == SessionData.Login &&
					x.State == ConcessionState.Active
				)
				.Select(x => new {
					x.Id
				})
				.FirstOrDefault()
			;

			// Cargando tarjeta
			var serviceCard = (await RepositoryServiceCard.GetAsync())
				.Where(x =>
					x.Uid == arguments.Uid &&
					x.State == ServiceCardState.Active &&
					x.SystemCardId == arguments.SystemCardId
				)
				.Select(x => new
				{
					x.Id,
					x.State,
					x.ConcessionId,
					x.OwnerUserId,
					x.OwnerUser.Name,
					x.OwnerUser.LastName,
					x.OwnerUser.VatNumber,
					x.OwnerUser.Photo,
					SystemCardId = x.SystemCardId,
					SystemCardName = x.SystemCard.Name,
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

			var ownCard = (serviceCard.OwnerUserId == null) || (serviceCard.ConcessionId == concession.Id);
			return new ResultBase<ServiceUserCreateCardGetResult>
			{
				Data = new ServiceUserCreateCardGetResult[] {
					new ServiceUserCreateCardGetResult
					{
						Uid = arguments.Uid,
						CardId = serviceCard.Id,
						CardState = serviceCard.State,
						Name = ownCard ? "" : serviceCard.Name,
						LastName = ownCard ? "" : serviceCard.LastName,
						VatNumber = ownCard ? "" : serviceCard.VatNumber,
						Photo = ownCard ? "" : serviceCard.Photo,
						SystemCardId = serviceCard.SystemCardId,
						SystemCardName = serviceCard.SystemCardName,
						ConcessionName = serviceCard.ConcessionName,
						NewCard = (serviceCard.OwnerUserId == null),
						OwnCard = ownCard
					}
				}
			};
		}
		#endregion ExecuteAsync
	}
}
