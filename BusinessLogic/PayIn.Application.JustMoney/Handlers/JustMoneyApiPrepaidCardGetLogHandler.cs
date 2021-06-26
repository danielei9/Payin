using Microsoft.Practices.Unity;
using PayIn.Application.Dto.JustMoney.Arguments;
using PayIn.Application.Dto.JustMoney.Results;
using PayIn.Application.JustMoney.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xp.Application.Dto;

namespace PayIn.Application.JustMoney.Handlers
{
	public class JustMoneyApiPrepaidCardGetLogHandler : IQueryBaseHandler<JustMoneyApiPrepaidCardGetLogArguments, JustMoneyApiPrepaidCardGetLogResult>
	{
		[Dependency] public JustMoneyApiPrepaidCardGetCardsHandler JustMoneyApiPrepaidCardGetCardsHandler { get; set; }
		[Dependency] public PfsService PfsService { get; set; }

		#region ExecuteAsync
		public async Task<ResultBase<JustMoneyApiPrepaidCardGetLogResult>> ExecuteAsync(JustMoneyApiPrepaidCardGetLogArguments arguments)
		{
			// From
			if (arguments.SinceYear < 1900 && arguments.SinceYear > 2100)
				throw new ArgumentException("La fecha 'desde' es no es válida");
			if (arguments.SinceMonth < 1 && arguments.SinceMonth > 12)
				throw new ArgumentException("La fecha 'desde' es no es válida");
			if (arguments.SinceDay < 1 && arguments.SinceDay > 31)
				throw new ArgumentException("La fecha 'desde' es no es válida");
			var from = new DateTime(arguments.SinceYear, arguments.SinceMonth, arguments.SinceDay);
			if (from.Day != arguments.SinceDay)
				throw new ArgumentException("La fecha 'desde' es no es válida");

			// To
			if (arguments.UntilYear < 1900 && arguments.UntilYear > 2100)
				throw new ArgumentException("La fecha 'hasta' es no es válida");
			if (arguments.UntilMonth < 1 && arguments.UntilMonth > 12)
				throw new ArgumentException("La fecha 'hasta' es no es válida");
			if (arguments.UntilDay < 1 && arguments.UntilDay > 31)
				throw new ArgumentException("La fecha 'hasta' es no es válida");
			var to = new DateTime(arguments.UntilYear, arguments.UntilMonth, arguments.UntilDay);
			if (to.Day != arguments.UntilDay)
				throw new ArgumentException("La fecha 'hasta' es no es válida");
			
            if (to < from)
				throw new ArgumentException("La fecha 'hasta' no puede ser anterior a la fecha 'desde'");
			var daysDiff = to - from;
			if (daysDiff.Days > 31)
				throw new ArgumentException("No se pueden devolver más de 31 días de datos");

			var cards = (await JustMoneyApiPrepaidCardGetCardsHandler.ExecuteAsync(new JustMoneyApiPrepaidCardGetCardsArguments("")))
				.Data
				.ToList();

			var result = new List<JustMoneyApiPrepaidCardGetLogResult>();
			foreach(var card in cards)
			{
				var resultPfsService = await PfsService.ViewStatementAsync(card.CardHolderId, arguments.SinceYear, arguments.SinceMonth, arguments.SinceDay, arguments.UntilYear, arguments.UntilMonth, arguments.UntilDay);
				
				var item = new JustMoneyApiPrepaidCardGetLogResult
				{
					Id = card.Id,
					Alias = card.Alias,
					Transactions = resultPfsService.Transactions
						.Select(x => new JustMoneyApiPrepaidCardGetLogResult_Transaction
						{
							Date = x.Date,
							Description = x.Description,
							Amount = x.Amount,
							AvailableBalance = x.AvailableBalance
						})
						.OrderByDescending(x => x.Date)
				};

                if (item.Transactions.Count() > 0)
                {
                    item.Balance = item.Transactions.Sum(y => (decimal?) y.Amount ?? 0m );
                    item.BalanceString = item.Balance.ToString("#,##0.00");
                }
                else
                {
                    item.Balance = 0;
                    item.BalanceString = "";
                }

				result.Add(item);
			}

			return new ResultBase<JustMoneyApiPrepaidCardGetLogResult>
			{
				Data = result
			};
		}
		#endregion ExecuteAsync
	}
}
