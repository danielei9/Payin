using PayIn.Application.Dto.Payments.Arguments;
using PayIn.Application.Dto.Payments.Results;
using PayIn.BusinessLogic.Common;
using PayIn.Common;
using PayIn.Domain.Payments;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xp.Application.Dto;
using Xp.Domain;

namespace PayIn.Application.Payments.Handlers
{
	public class TicketGraphHandler :
		IQueryBaseHandler<TicketGraphArguments, TicketGraphResult>
	{
		public class Temp
		{
			public decimal TicketAmount { get; set; }
			public decimal ChargedAmount { get; set; }
			public decimal RefundedAmount { get; set; }
			public DateTime Day { get; set; }
		}

		private readonly IEntityRepository<Ticket> Repository;
		private readonly IEntityRepository<Payment> PaymentRepository;
		private readonly ISessionData SessionData;

		#region Constructors
		public TicketGraphHandler(
			ISessionData sessionData,
			IEntityRepository<Ticket> repository,
			IEntityRepository<Payment> paymentrepository
		)
		{
			if (sessionData == null) throw new ArgumentNullException("sessionData");
			if (repository == null) throw new ArgumentNullException("repository");
			if (paymentrepository == null) throw new ArgumentNullException("paymentrepository");

			SessionData = sessionData;
			Repository = repository;
			PaymentRepository = paymentrepository;
		}
		#endregion Constructors

		#region ExecuteAsync
		public async Task<ResultBase<TicketGraphResult>> ExecuteAsync(TicketGraphArguments arguments)
		{
			var untilNextDay = arguments.Until.AddDays(1);

			var tickets = (await Repository.GetAsync())
				.Where(x =>
					x.Date >= arguments.Since &&
					x.Date < untilNextDay &&
					x.State == TicketStateType.Active &&
					x.Concession.Concession.Supplier.Login == SessionData.Email

				)
				.Select(x => new Temp
				{
					Day = x.Date,
					TicketAmount = x.Amount,
					ChargedAmount = 0,
					RefundedAmount = 0
				});

			var payments = (await PaymentRepository.GetAsync())
				.Where(x =>
					x.Date >= arguments.Since &&
					x.Date < untilNextDay &&
					x.State == PaymentState.Active &&
					x.Ticket.Concession.Concession.Supplier.Login == SessionData.Email

				)
				.Select(x => new Temp
				{
					Day = x.Date,
					TicketAmount = 0,
					ChargedAmount = x.RefundFromId == null ? x.Amount : 0,
					RefundedAmount = x.RefundFromId != null ? -x.Amount : 0
				});

			var dates = new List<Temp>();
			for (var date = arguments.Since.Value.Date; date <= arguments.Until.Value.Date; date = date.AddDays(1))
					dates.Add(new Temp {
						Day = date.Date,
						TicketAmount = 0,
						ChargedAmount = 0,
						RefundedAmount = 0
					});

			var result =
				tickets
				.Union(payments)
				.ToList()
				.Union(dates)
				.GroupBy(x => x.Day.Date)
				.Select(x => new TicketGraphResult
				{
					Day = x.Key,
					ChargedAmount = x.Sum(y => y.ChargedAmount),
					RefundedAmount = x.Sum(y => y.RefundedAmount),
					TicketAmount = x.Sum(y => y.TicketAmount)
				})
				.OrderBy(x => (DateTime)x.Day)
				.ToList();

			return new ResultBase<TicketGraphResult> { Data = result };
		}
		#endregion ExecuteAsync
	}
}

