using PayIn.Application.Dto.Payments.Arguments.TransportLog;
using PayIn.Application.Dto.Payments.Results.TransportLog;
using PayIn.BusinessLogic.Common;
using PayIn.Common;
using PayIn.Domain.Payments;
using PayIn.Domain.Security;
using PayIn.Infrastructure.Security;
using System;
using System.Linq;
using System.Threading.Tasks;
using Xp.Application.Dto;
using Xp.Domain;

namespace PayIn.Application.Public.Handlers
{
	public class TransportLogStateHandler :
		IQueryBaseHandler<TransportLogStateArguments, TransportLogStateResult>
	{
		private readonly IEntityRepository<TransportConcession> _Repository;
		private readonly IEntityRepository<LogArgument> LogArgumentRepository;
		private readonly IEntityRepository<Ticket> TicketRepository;
		private readonly ISessionData SessionData;

		#region Constructors
		public TransportLogStateHandler(
			ISessionData sessionData,
			IEntityRepository<PayIn.Domain.Payments.TransportConcession> repository,
			IEntityRepository<LogArgument> logArgumentRepository,
			IEntityRepository<Ticket> ticketRepository
		)
		{
			if (sessionData == null) throw new ArgumentNullException("sessionData");
			SessionData = sessionData;

			if (repository == null)
				throw new ArgumentNullException("repository");
			_Repository = repository;

			if (logArgumentRepository == null)
				throw new ArgumentNullException("LogArgumentRepository");
			LogArgumentRepository = logArgumentRepository;

			if (ticketRepository == null)
				throw new ArgumentNullException("TicketRepository");
			TicketRepository = ticketRepository;
		}
		#endregion Constructors

		#region ExecuteAsync
		async Task<ResultBase<TransportLogStateResult>> IQueryBaseHandler<TransportLogStateArguments, TransportLogStateResult>.ExecuteAsync(TransportLogStateArguments arguments)
		{


			return null;
		}
		#endregion ExecuteAsync
	}
}
