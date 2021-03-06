using Microsoft.Practices.Unity;
using PayIn.Application.Dto.JustMoney.Arguments;
using PayIn.Application.Dto.JustMoney.Results;
using PayIn.Application.JustMoney.Services;
using System;
using System.Linq;
using System.Threading.Tasks;
using Xp.Application.Dto;

namespace PayIn.Application.JustMoney.Handlers
{
	public class JustMoneyMobilePrepaidCardGetLogHandler : IQueryBaseHandler<JustMoneyMobilePrepaidCardGetLogArguments, JustMoneyMobilePrepaidCardGetLogResult>
	{
		[Dependency] public JustMoneyApiPrepaidCardGetCardsHandler JustMoneyApiPrepaidCardGetCardsHandler { get; set; }
		[Dependency] public PfsService PfsService { get; set; }

		#region ExecuteAsync
		public async Task<ResultBase<JustMoneyMobilePrepaidCardGetLogResult>> ExecuteAsync(JustMoneyMobilePrepaidCardGetLogArguments arguments)
		{
			// From
			if (arguments.SinceYear < 1900 && arguments.SinceYear > 2100)
				throw new ApplicationException("La fecha 'desde' es no es válida");
			if (arguments.SinceMonth < 1 && arguments.SinceMonth > 12)
				throw new ApplicationException("La fecha 'desde' es no es válida");
			if (arguments.SinceDay < 1 && arguments.SinceDay > 31)
				throw new ApplicationException("La fecha 'desde' es no es válida");
			var from = new DateTime(arguments.SinceYear, arguments.SinceMonth, arguments.SinceDay);
			if (from.Day != arguments.SinceDay)
				throw new ApplicationException("La fecha 'desde' es no es válida");

			// To
			if (arguments.UntilYear < 1900 && arguments.UntilYear > 2100)
				throw new ArgumentException("La fecha 'hasta' es no es válida");
			if (arguments.UntilMonth < 1 && arguments.UntilMonth > 12)
				throw new ArgumentException("La fecha 'hasta' es no es válida");
			if (arguments.UntilDay < 1 && arguments.UntilDay > 31)
				throw new ArgumentException("La fecha 'hasta' es no es válida");
			var to = new DateTime(arguments.UntilYear, arguments.UntilMonth, arguments.UntilDay);
			if (to.Day != arguments.UntilDay)
				throw new ApplicationException("La fecha 'hasta' es no es válida");
			
            if (to < from)
				throw new ApplicationException("La fecha 'hasta' no puede ser anterior a la fecha 'desde'");
			var daysDiff = to - from;
			if (daysDiff.Days > 31)
				throw new ApplicationException("No se pueden devolver más de 31 días de datos");

			var card = (await JustMoneyApiPrepaidCardGetCardsHandler.ExecuteAsync(new JustMoneyApiPrepaidCardGetCardsArguments("")))
				.Data
				.Where(x => x.Id == arguments.Id)
				.FirstOrDefault() ??
				throw new ArgumentNullException(nameof(arguments.Id));

			var resultPfs = await PfsService.CardInquiryAsync(card.CardHolderId);

			var viewStatementResult = await PfsService.ViewStatementAsync(card.CardHolderId, arguments.SinceYear, arguments.SinceMonth, arguments.SinceDay, arguments.UntilYear, arguments.UntilMonth, arguments.UntilDay);
			var item = new JustMoneyMobilePrepaidCardGetLogResult
			{
				Id = card.Id,
				AvailableBalance = resultPfs.CardInfo.AvailBal,
				CardStatus = resultPfs.CardInfo.CardStatus,
				Transactions = viewStatementResult.Transactions
					.Select(x => new JustMoneyMobilePrepaidCardGetLogResult_Transaction
					{
						Date = x.Date,
						Description = x.Description,
						Amount = x.Amount,
						AvailableBalance = x.AvailableBalance
					})
			};

			return new ResultBase<JustMoneyMobilePrepaidCardGetLogResult>
			{
				Data = new[] { item }
			};
		}
		#endregion ExecuteAsync
	}
}
