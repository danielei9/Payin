using Microsoft.Practices.Unity;
using PayIn.Application.Dto.Arguments.Main;
using PayIn.Application.Dto.Results.Main;
using PayIn.BusinessLogic.Common;
using PayIn.Common;
using PayIn.Domain.Payments;
using PayIn.Domain.Public;
using System;
using System.Linq;
using System.Threading.Tasks;
using Xp.Application.Dto;
using Xp.Domain;

namespace PayIn.Application.Public.Handlers.Main
{
	public class MobileMainGetAllV2Handler : 
		IQueryBaseHandler<MainMobileGetAllV2Arguments, MainMobileGetAllV2Result>
	{
		[Dependency] public ISessionData                           SessionData                   { get; set; }
		[Dependency] public IEntityRepository<ControlPlanning>     ControlPlanningRepository     { get; set; }
		[Dependency] public IEntityRepository<ControlTrack>        ControlTrackRepository        { get; set; }
		[Dependency] public IEntityRepository<PaymentConcession>   PaymentConcessionRepository   { get; set; }
		[Dependency] public IEntityRepository<PaymentMedia>        PaymentMediaRepository        { get; set; }
		[Dependency] public IEntityRepository<ServiceNotification> ServiceNotificationRepository { get; set; }
		[Dependency] public IEntityRepository<ServiceOption>       ServiceOptionRepository       { get; set; }
		[Dependency] public IEntityRepository<ServiceSupplier>     ServiceSupplierRepository     { get; set; }
		[Dependency] public IEntityRepository<ServiceWorker>       ServiceWorkerRepository       { get; set; }
		[Dependency] public IEntityRepository<Ticket>              TicketRepository              { get; set; }
		[Dependency] public IEntityRepository<PaymentUser>		   PaymentUserRepository		 { get; set; }
		[Dependency] public IEntityRepository<PaymentWorker>	   PaymentWorkerRepository		 { get; set; }
        
		#region ExecuteAsync
		public async Task<ResultBase<MainMobileGetAllV2Result>> ExecuteAsync(MainMobileGetAllV2Arguments arguments)
        {
            var now = DateTime.Now;
            
            var numReceipts = (await TicketRepository.GetAsync())
                .Where(x =>
				    x.PaymentUser.Login == SessionData.Login &&
				    now >= x.Since &&
				    !(x.Payments.Any(z => z.State != PaymentState.Active) && x.Amount == 0)
                )
				.Count();

			var appVersion = (await ServiceOptionRepository.GetAsync())
				.Where(x => x.Name == "AndroidVersionCode")
				.Select(x => x.Value)
				.FirstOrDefault();

			var numNotifications = (await ServiceNotificationRepository.GetAsync())
				.Where(x => x.ReceiverLogin == SessionData.Login && x.State == NotificationState.Actived)
				.Count();

			var workerHasConcession = (await PaymentWorkerRepository.GetAsync())
				.Where(x => x.Login == SessionData.Login && x.Concession != null)
                .Count();

			var userHasConcession = (await PaymentUserRepository.GetAsync())
				.Where(x => x.Login == SessionData.Login && x.Concession != null)
                .Count();

			return new MainMobileGetAllV2ResultBase {
				NumNotifications = numNotifications,
				NumReceipts = numReceipts,
				AppVersion = appVersion,
				WorkerHasConcession = (workerHasConcession > 0) ? true : false,
				UserHasConcession = (userHasConcession > 0) ? true : false
			};
		}
		#endregion ExecuteAsync
	}
}
