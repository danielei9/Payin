using PayIn.Application.Dto.Payments.Arguments;
using PayIn.BusinessLogic.Common;
using PayIn.Common;
using PayIn.Domain.Payments;
using System;
using System.Linq;
using System.Threading.Tasks;
using Xp.Application.Dto;
using Xp.Domain;

namespace PayIn.Application.Payments.Handlers
{
	public class MobileContactScanExhibitorHandler :
		IServiceBaseHandler<MobileContactScanExhibitorArguments>
	{
		private readonly ISessionData SessionData;
		private readonly IEntityRepository<Contact> Repository;
		private readonly IEntityRepository<Exhibitor> ExhibitorRepository;

		#region Contructors
		public MobileContactScanExhibitorHandler(
			ISessionData sessionData,
			IEntityRepository<Contact> repository,
			IEntityRepository<Exhibitor> exhibitorRepository
		)
		{
			if (sessionData == null) throw new ArgumentNullException("sessionData");
			if (repository == null) throw new ArgumentNullException("repository");
			if (exhibitorRepository == null) throw new ArgumentNullException("exhibitorRepository");
		
			SessionData = sessionData;
			Repository = repository;
			ExhibitorRepository = exhibitorRepository;
		}
		#endregion Contructors

		#region ExecuteAsync
		public async Task<dynamic> ExecuteAsync(MobileContactScanExhibitorArguments arguments)
		{
			var exhibitorId = (await ExhibitorRepository.GetAsync())
				.Where(x =>
					x.Id == arguments.ExhibitorId &&
					x.State == ExhibitorState.Active &&
					x.Events.Any(y =>
						y.State == EventState.Active
					)
				)
				.Select(x =>
					(int?)x.Id
				)
				.FirstOrDefault();
			if (exhibitorId == null)
				throw new ApplicationException("No se ha encontrado el puesto escaneado");

			var eventId = (await ExhibitorRepository.GetAsync())
				.Where(x =>
					x.Events.Any(y =>
						y.Id == arguments.EventId &&
					y.State == EventState.Active
					)
				)
				.Select(x =>
						x.Events.
						Select(y =>
							(int?)y.Id
							).FirstOrDefault()
					)
					.FirstOrDefault();
			if (eventId == null)
				throw new ApplicationException("No se ha encontrado el evento asociado");

			//var qrLogin = (await Repository.GetAsync())
			//	.Where(x => x.Entrance.Login == SessionData.Login)
			//	.Select(x => x.Entrance.Login)
			//	.FirstOrDefault();

			var contacts = (await Repository.GetAsync())
				.Where(x => x.ExhibitorId == arguments.ExhibitorId);
				
			var existContact= contacts.Any(y => y.VisitorName == SessionData.Name);


			if (existContact == true)
			{
				foreach (var updateContact in contacts)
					if (updateContact.State == ContactState.Deleted)
						updateContact.State = ContactState.Active; //añadir excepcion con mensaje
			}
			else
			{

				var contact = new Contact
				{
					ExhibitorId = exhibitorId.Value,
					EventId = eventId.Value,
					VisitorLogin = SessionData.Login,
					VisitorName = SessionData.Name,
					State = ContactState.Active
				};
				await Repository.AddAsync(contact);

				return contact;
			}
			return null;
		}
		#endregion ExecuteAsync
	}
}
