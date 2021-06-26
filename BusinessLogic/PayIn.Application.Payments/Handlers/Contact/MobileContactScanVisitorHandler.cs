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
	public class MobileContactScanVisitorHandler :
		IServiceBaseHandler<MobileContactScanVisitorArguments>
	{
		private readonly ISessionData SessionData;
		private readonly IEntityRepository<Contact> Repository;
		private readonly IEntityRepository<Entrance> EntranceRepository;
		private readonly IEntityRepository<Exhibitor> ExhibitorRepository;

		#region Contructors
		public MobileContactScanVisitorHandler(
			ISessionData sessionData,
			IEntityRepository<Contact> repository,
			IEntityRepository<Entrance> entranceRepository,
			IEntityRepository<Exhibitor> exhibitorRepository
		)
		{
			if (sessionData == null) throw new ArgumentNullException("sessionData");
			if (repository == null) throw new ArgumentNullException("repository");
			if (entranceRepository == null) throw new ArgumentNullException("entranceRepository");
			if (exhibitorRepository == null) throw new ArgumentNullException("exhibitorRepository");

			SessionData = sessionData;
			Repository = repository;
			EntranceRepository = entranceRepository;
			ExhibitorRepository = exhibitorRepository;
		}
		#endregion Contructors

		#region ExecuteAsync
		public async Task<dynamic> ExecuteAsync(MobileContactScanVisitorArguments arguments)
		{
			var exhibitorId = (await ExhibitorRepository.GetAsync())
				.Where(x =>
					x.PaymentConcession.Concession.Supplier.Login == SessionData.Login &&
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

			var eventId = (await EntranceRepository.GetAsync())
				.Where(x =>
					x.Id == arguments.VisitorEntranceId &&
					x.EntranceTypeId == x.EntranceType.Id &&
					x.EntranceType.EventId == x.EntranceType.Event.Id
				)
				.Select(x =>
					(int?)x.EntranceType.Event.Id
				)
				.FirstOrDefault();
			if (eventId == null)
				throw new ApplicationException("No se ha encontrado el evento asociado");


			var entrance = (await EntranceRepository.GetAsync())
				.Where(x =>
					x.Id == arguments.VisitorEntranceId
				)
				.Select(x => new {
					Login = x.Login,
					Name = x.UserName + " " + x.LastName
				})
				.FirstOrDefault();
			if (entrance.Login.IsNullOrEmpty())
				throw new ArgumentNullException("VisitorEntranceId");

			var contact = new Contact
			{
				ExhibitorId = exhibitorId.Value,
				EventId = eventId.Value,
				VisitorEntranceId = arguments.VisitorEntranceId,
				VisitorLogin = entrance.Login,
				VisitorName = entrance.Name,
				State = ContactState.Active
			};
			await Repository.AddAsync(contact);

			return contact;
		}
		#endregion ExecuteAsync
	}
}
