using PayIn.Application.Dto.Payments.Arguments;
using PayIn.Application.Dto.Payments.Results;
using PayIn.BusinessLogic.Common;
using PayIn.Common;
using PayIn.Domain.Payments;
using System;
using System.Collections.Generic;
using System.Data.Entity.SqlServer;
using System.Linq;
using System.Threading.Tasks;
using Xp.Application.Dto;
using Xp.Domain;

namespace PayIn.Application.Payments.Handlers
{
	public class PaymentsGetGraphHandler :
		IQueryBaseHandler<PaymentGetGraphArguments, PaymentsGetGraphResult>
	{
		private readonly IEntityRepository<Payment> Repository;
		private readonly ISessionData SessionData;

		#region Constructors
		public PaymentsGetGraphHandler(
			ISessionData sessionData,
			IEntityRepository<Payment> repository
		)
		{
			if (sessionData == null) throw new ArgumentNullException("sessionData");
			SessionData = sessionData;

			if (repository == null)
				throw new ArgumentNullException("repository");
			Repository = repository;
		}
		#endregion Constructors

		#region ExecuteAsync
		async Task<ResultBase<PaymentsGetGraphResult>> IQueryBaseHandler<PaymentGetGraphArguments, PaymentsGetGraphResult>.ExecuteAsync(PaymentGetGraphArguments arguments)
		{
			var untilNextDay = arguments.Until.AddDays(1);

			var returnedPayments = (await Repository.GetAsync())
				.Where(x =>
					x.Date >= arguments.Since &&
					x.Date < untilNextDay &&
					x.State == PaymentState.Active &&
					x.RefundFromId != null 
				)
				.Select(x => new
				{
					Day = x.Date,
					PayedAmount = 0.0m,
					ReturnedAmount = x.Amount
				});

			var payedPayments = (await Repository.GetAsync())
				.Where(x =>
					x.Date >= arguments.Since &&
					x.Date < untilNextDay &&
					x.State == PaymentState.Active &&
					x.RefundFromId == null 
				)
				.Select(x => new
				{
					Day = x.Date,
					PayedAmount = x.Amount,
					ReturnedAmount = 0.0m
				});

			var payments = payedPayments
				.Union(returnedPayments)
				.GroupBy(x => SqlFunctions.DatePart("yyyy", x.Day).Value + "-" + SqlFunctions.DatePart("mm", x.Day).Value + "-" + SqlFunctions.DatePart("dd", x.Day).Value)
				.Select(x => new
				{
					Day = x.FirstOrDefault().Day,
					PayedAmount = x.Sum(y => y.PayedAmount),
					ReturnedAmount = x.Sum(y => y.ReturnedAmount)
				})
				.ToList()
				.Select(x => new PaymentsGetGraphResult
				{
					Day = x.Day,
					PayedAmount = x.PayedAmount,
					ReturnedAmount = x.ReturnedAmount
				});

			var dates = new List<DateTime>();
			for (var date = arguments.Since.Value.Date; date <= arguments.Until.Value.Date; date = date.AddDays(1))
				if (!payments.Any(x => x.Day.Value == date))
					dates.Add(date);

			var result =
				payments
				.Union(
					dates.Select(x => new PaymentsGetGraphResult
					{
						Day = x,
						PayedAmount = 0.0m,
						ReturnedAmount = 0.0m
					})
				)
				.OrderBy(x => (DateTime)x.Day);

			return new ResultBase<PaymentsGetGraphResult> { Data = result };
		}
		#endregion ExecuteAsync
	}
}
