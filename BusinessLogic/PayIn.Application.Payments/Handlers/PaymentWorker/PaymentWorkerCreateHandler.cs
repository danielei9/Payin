using Microsoft.Practices.Unity;
using PayIn.Application.Dto.Arguments.Notification;
using PayIn.Application.Dto.Payments.Arguments;
using PayIn.Application.Dto.Security.Arguments;
using PayIn.Application.Public.Handlers;
using PayIn.BusinessLogic.Common;
using PayIn.Common;
using PayIn.Common.Resources;
using PayIn.Domain.Payments;
using PayIn.Infrastructure.Security;
using System;
using System.Linq;
using System.Threading.Tasks;
using Xp.Application.Dto;
using Xp.Domain;

namespace PayIn.Application.Payments.Handlers
{
	public class PaymentWorkerCreateHandler :
		IServiceBaseHandler<PaymentWorkerCreateArguments>
	{
		[Dependency] public IEntityRepository<PaymentWorker> Repository { get; set; }
		[Dependency] public IEntityRepository<PaymentConcession> ConcessionRepository { get; set; }
		[Dependency] public ISessionData SessionData { get; set; }
		[Dependency] public IUnitOfWork UnitOfWork { get; set; }
		[Dependency] public ServiceNotificationCreateHandler ServiceNotificationCreate { get; set; }
		
		#region ExecuteAsync
		async Task<dynamic> IServiceBaseHandler<PaymentWorkerCreateArguments>.ExecuteAsync(PaymentWorkerCreateArguments arguments)
		{
			if (arguments.Login == SessionData.Login)
				throw new Exception(PaymentWorkerResources.EmailPaymentCommerce);

			var PaymentConcession = (await ConcessionRepository.GetAsync("Concession.Supplier"))
			.Where(x => x.Concession.Supplier.Login == SessionData.Login)
			.FirstOrDefault();

			var worker = (await Repository.GetAsync())
			.Where(x => x.Login == arguments.Login)
			.FirstOrDefault();

			if (worker != null && (worker.State == WorkerState.Active )) // || worker.State == WorkerState.Pending))
				throw new ApplicationException(ServiceWorkerResources.ExceptionWorkerMailAlreadyExists);


			var securityRepository = new SecurityRepository();
			var user = await securityRepository.GetUserAsync(arguments.Login);
			if (user == null)
			{
				var item = new PaymentWorker
				{
					Name = arguments.Name,
					Login = arguments.Login,
					ConcessionId = PaymentConcession.Id,
					Tickets = PaymentConcession.Tickets.ToList(),
					State = WorkerState.Active
				};
				await Repository.AddAsync(item);
				await UnitOfWork.SaveAsync();

				await ServiceNotificationCreate.ExecuteAsync(new ServiceNotificationCreateArguments(
					type: NotificationType.ConcessionVinculation,
					message: PaymentWorkerResources.AcceptAssociationMessage.FormatString(PaymentConcession.Concession.Supplier.Name),
					referenceId: item.Id,
					referenceClass: "PaymentWorker",
					senderLogin: PaymentConcession.Concession.Supplier.Login,
					receiverLogin: arguments.Login
				));

				AccountRegisterArguments userModel = new AccountRegisterArguments
				{
					UserName = arguments.Login,
					Name = arguments.Name
				};

				await securityRepository.InviteWorkerAsync(userModel);

				return item;

			} else
			{
				if (worker == null)
				{
					worker = new PaymentWorker
					{
						Name = arguments.Name,
						Login = arguments.Login,
						ConcessionId = PaymentConcession.Id,
						Tickets = PaymentConcession.Tickets.ToList(),
						State = WorkerState.Active
					};
					await Repository.AddAsync(worker);					
				}
				else
				{
					worker.State = WorkerState.Active;
				}
				await UnitOfWork.SaveAsync();

				return worker;				
			}


		}
		#endregion ExecuteAsync
	}
}
