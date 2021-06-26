using PayIn.Application.Dto.Payments.Arguments;
using PayIn.BusinessLogic.Common;
using PayIn.Common;
using PayIn.Domain.Payments;
using System;
using System.Linq;
using System.Threading.Tasks;
using Xp.Application.Attributes;
using Xp.Application.Dto;
using Xp.Domain;

namespace PayIn.Application.Payments.Handlers
{
	[XpLog("Ticket", "Create")]
	[XpAnalytics("Ticket", "Create")]
	public class TicketTpvCreateHandler :
		IServiceBaseHandler<TicketTpvCreateArguments>
	{
		private readonly IServiceBaseHandler<TicketMobileCreateArguments> Handler;
		private readonly IEntityRepository<PaymentConcession> PaymentConcessionRepository;
		private readonly ISessionData SessionData;

		#region Contructors
		public TicketTpvCreateHandler(
			IServiceBaseHandler<TicketMobileCreateArguments> handler,
			IEntityRepository<PaymentConcession> paymentConcessionRepository,
			ISessionData sessionData
		)
		{
			if (handler == null) throw new ArgumentNullException("handler");
			if (paymentConcessionRepository == null) throw new ArgumentNullException("paymentConcessionRepository");
			if (sessionData == null) throw new ArgumentNullException("sessionData");

			Handler = handler;
			PaymentConcessionRepository = paymentConcessionRepository;
			SessionData = sessionData;
		}
		#endregion Contructors

		#region ExecuteAsync
		public async Task<dynamic> ExecuteAsync(TicketTpvCreateArguments arguments)
		{
			var concession = (await PaymentConcessionRepository.GetAsync())
				.Where(x =>
					x.Concession.State == ConcessionState.Active &&
					(
						x.PaymentWorkers
							.Where(y => y.State == WorkerState.Active)
							.Select(y => y.Login)
								.Contains(SessionData.Login) ||
						x.Concession.Supplier.Login == SessionData.Login
					)
				)
				.Select(x => new {
					x.Id
				})
				.FirstOrDefault();
			arguments.ConcessionId = concession.Id;

			return await Handler.ExecuteAsync(arguments);
		}
		#endregion ExecuteAsync
	}
}
