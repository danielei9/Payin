using Microsoft.Practices.Unity;
using PayIn.Application.Dto.Arguments.Main;
using PayIn.Application.Dto.Results.Main;
using PayIn.BusinessLogic.Common;
using PayIn.Common;
using PayIn.Common.Resources;
using PayIn.Domain.Payments;
using PayIn.Domain.Public;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xp.Application.Dto;
using Xp.Common;
using Xp.Common.Dto.Arguments;
using Xp.Domain;

namespace PayIn.Application.Public.Handlers.Main
{
	public class MobileMainGetAllHandler :
		IQueryBaseHandler<MainMobileGetAllArguments, MainMobileGetAllResult>
	{
		[Dependency] public ISessionData SessionData { get; set; }
		[Dependency] public IEntityRepository<ControlPlanning> ControlPlanningRepository { get; set; }
		[Dependency] public IEntityRepository<ControlTrack> ControlTrackRepository { get; set; }
		[Dependency] public IEntityRepository<PaymentConcession> PaymentConcessionRepository { get; set; }
		[Dependency] public IEntityRepository<PaymentMedia> PaymentMediaRepository { get; set; }
		[Dependency] public IEntityRepository<ServiceNotification> ServiceNotificationRepository { get; set; }
		[Dependency] public IEntityRepository<ServiceOption> ServiceOptionRepository { get; set; }
		[Dependency] public IEntityRepository<ServiceSupplier> ServiceSupplierRepository { get; set; }
		[Dependency] public IEntityRepository<ServiceWorker> ServiceWorkerRepository { get; set; }
		[Dependency] public IEntityRepository<Ticket> TicketRepository { get; set; }
		[Dependency] public IEntityRepository<PaymentUser> PaymentUserRepository { get; set; }
		[Dependency] public IEntityRepository<PaymentWorker> PaymentWorkerRepository { get; set; }

		public IArgumentsBase a {get;set;}
		
		#region ExecuteAsync
		public async Task<ResultBase<MainMobileGetAllResult>> ExecuteAsync(MainMobileGetAllArguments arguments)
		{
			var now = DateTime.Now;
			var nowUTC = now.ToUTC();

			var favorites = new List<MainMobileGetAllResultBase.Favorite>();
			var paymentMedias = (await PaymentMediaRepository.GetAsync())
				.Where(x => 
					x.State != PaymentMediaState.Error &&
					x.State != PaymentMediaState.Delete &&
					x.Login == SessionData.Login
				)
				.OrderBy(x => x.VisualOrder)
				.Select(x => new MainMobileGetAllResult
				{
					Id              = x.Id,
					Title           = x.Name,
					VisualOrder     = x.VisualOrder,
					NumberHash      = x.NumberHash,
					ExpirationMonth = x.ExpirationMonth,
					ExpirationYear  = x.ExpirationYear,
					Type            = x.Type,
					BankEntity      = x.BankEntity,
					State           = x.State
				});

			// Parte de presence
			var worker = (await ServiceWorkerRepository.GetAsync())
				.Where(x => x.Login == SessionData.Login && x.Supplier.Concessions.Any(y => y.Type == ServiceType.Control))
				.Select(x => x.Id);

			var sumChecks = 0d;
			var checkDuration = 0d;
			var working = false;
			if (worker.Count() > 0)
			{
				favorites.Add(new MainMobileGetAllResultBase.Favorite
					{
						Id = 0,
						Title = ControlPresenceResources.GetAll,
						Subtitle = "",
						VisualOrder = 1,
						Type = PinType.ControlPresence
					});

				sumChecks = (await ControlTrackRepository.GetAsync())
					.Where(x =>
						x.Item.CheckTimetable == true &&
						x.PresenceSince.Date.Year == now.Year &&
						x.PresenceSince.Date.Month == now.Month &&
						x.PresenceSince.Date.Day == now.Day &&
						worker.Contains(x.WorkerId)
					)
					.Select(x => new
					{
						Start = x.PresenceSince.Date,
						End = x.PresenceUntil != null ? x.PresenceUntil.Date : nowUTC
					})
					.ToList()
					.Select(x => new {
						Start = (XpDuration) x.Start,
						End   = (XpDuration) x.End
					})
					.Sum(x => x.End._Value.TimeOfDay.TotalMinutes - x.Start._Value.TimeOfDay.TotalMinutes);

				checkDuration = (await ControlPlanningRepository.GetAsync())
					.Where(x =>
						x.Date.Year == now.Year &&
						x.Date.Month == now.Month &&
						x.Date.Day == now.Day &&
						worker.Contains(x.WorkerId) &&
						x.CheckDuration != null
					)
					.Select(x => x.CheckDuration)
					.ToList()
					.Select(x => (XpDuration)x)
					.Sum(x => x._Value.TimeOfDay.TotalMinutes);

				working = (await ControlTrackRepository.GetAsync())
					.Where(x =>
						x.Item.CheckTimetable == true &&
						x.PresenceSince.Date.Year == now.Year &&
						x.PresenceSince.Date.Month == now.Month &&
						x.PresenceSince.Date.Day == now.Day &&
						worker.Contains(x.WorkerId) &&
						x.PresenceUntil == null
					)
					.Count() > 0 ? true : false;
			}
			// Payments
			var tickets = (await TicketRepository.GetAsync());
			//numero de recibos
			var numReceipts = tickets.Where(x =>
				x.PaymentUser.Login == SessionData.Login &&
				now >= x.Since &&
				!(x.Payments.Any(z => z.State != PaymentState.Active) && x.Amount == 0))
				.Count();
			// Parte para pagos
			var commerce = (await PaymentConcessionRepository.GetAsync())
			.Where(x => 
				(
					x.Concession.Supplier.Login == SessionData.Login || 
					x.PaymentWorkers.Any(y => 
						y.Login == SessionData.Login && 
						y.State == WorkerState.Active
					)
				) && 
				x.Concession.Type == ServiceType.Charge && 
				x.Concession.State == ConcessionState.Active)
			.Select(x => x.Id);
			
			if (commerce.Count() > 0)
			{
				favorites.Add(new MainMobileGetAllResultBase.Favorite
				{
					Id = 1,
					Title = TicketResources.CreateTicket,
					Subtitle = "",
					VisualOrder = 2,
					Type = PinType.Ticket
				});

				tickets = tickets
					.Where(x =>
						(x.Concession.Concession.Supplier.Login == SessionData.Login && x.Concession.Concession.Type == ServiceType.Charge &&  x.Concession.Concession.State == ConcessionState.Active) || 
						(x.PaymentWorker.Login == SessionData.Login && x.PaymentWorker.State == WorkerState.Active )||
						x.Payments.Any(y => y.UserLogin == SessionData.Login) ||
						(x.Payments.Count() == 0 && (x.Concession.PaymentWorkers.Any(y => y.Login == SessionData.Login && x.PaymentWorker.State == WorkerState.Active)))
					);
			} else {
				tickets = tickets
					.Where(x => x.Payments.Any(y => y.UserLogin == SessionData.Login));
			}

			var resTickets = tickets
				.Where(x => !(x.Payments.Any(z => z.State != PaymentState.Active) && x.Amount == 0))
				.Select(x => new
				{
					x.Id,
					x.Reference,
					x.Date,
					x.Amount,
					PayedAmount = x.Payments
						.Where(y => y.State == PaymentState.Active)
						.Sum(y => (decimal?) y.Amount) ?? 0m,
					x.SupplierName,
					x.Until,
					x.Since,
					x.Type,
					x.State
				})
				.OrderByDescending(x => x.Date)
				.ToList()
				.Skip(arguments.Skip)
				.Take(arguments.Top)
				.Select(x => new MainMobileGetAllResultBase.Ticket
				{
					Id = x.Id,
					Reference = x.Reference,
					Date = x.Date.ToUTC(),
					Amount = x.Amount,
					PayedAmount = x.PayedAmount,
					SupplierName = x.SupplierName,
					Until = x.Until.ToUTC(),
					Since = x.Since.ToUTC(),
					Type = x.Type,
					State = x.State
				})
				.ToList();

			var appVersion = (await ServiceOptionRepository.GetAsync())
				.Where(x => x.Name == "AndroidVersionCode")
				.Select(x => x.Value)
				.FirstOrDefault();

			var numNotifications = (await ServiceNotificationRepository.GetAsync())
				.Where(x => x.ReceiverLogin == SessionData.Login && x.State == NotificationState.Actived)
				.Count();

			var workerHasConcession = (await PaymentWorkerRepository.GetAsync())
				.Where(x => x.Login == SessionData.Login && x.Concession != null);

			var userHasConcession = (await PaymentUserRepository.GetAsync())
				.Where(x => x.Login == SessionData.Login && x.Concession != null);

			return new MainMobileGetAllResultBase {
				NumNotifications = numNotifications,
				NumReceipts = numReceipts,
				AppVersion = appVersion,
				Favorites = favorites.OrderBy(x => x.VisualOrder),
				Tickets = resTickets,
				Data = paymentMedias,
				SumChecks = TimeSpan.FromMinutes(sumChecks),
				CheckDuration = TimeSpan.FromMinutes(checkDuration),
				Working = working,
				WorkerHasConcession = (workerHasConcession.Count() > 0) ? true : false,
				UserHasConcession = (userHasConcession.Count() > 0) ? true : false
			};
		}
		#endregion ExecuteAsync
	}
}
