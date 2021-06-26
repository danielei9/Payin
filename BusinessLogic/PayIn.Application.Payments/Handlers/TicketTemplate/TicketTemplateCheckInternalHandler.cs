using PayIn.Application.Dto.Payments.Arguments.TicketTemplate;
using PayIn.Application.Dto.Payments.Results.TicketTemplate;
using PayIn.Common;
using PayIn.Domain.Payments;
using System;
using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Xp.Application.Dto;
using Xp.Common;
using Xp.Domain;
using Xp.Infrastructure;

namespace PayIn.Application.Payments.Handlers
{
	public class TicketTemplateCheckInternalHandler :
		IQueryBaseHandler<TicketTemplateCheckInternalArguments, TicketTemplateCheckResult>
	{
		#region ExecuteAsync
		public async Task<ResultBase<TicketTemplateCheckResult>> ExecuteAsync(TicketTemplateCheckInternalArguments arguments)
		{
			return await Task.Run(() =>
			{
				var match = new Regex(arguments.RegEx)
									.Match(arguments.Text);

				var result = new TicketTemplateCheckResultBase
				{
					Success = match.Success
				};
				if (!result.Success)
					return result;

				result.Data = match
						.Groups
						.Cast<Group>()
						.Select(x => new TicketTemplateCheckResult()
						{
							Value = x.Value
						});

				//Selección de cultura  . -> en-US  resto -> es-ES
				if (arguments.AmountPosition != null && arguments.AmountPosition < match.Groups.Count)
				{
					if (arguments.DecimalCharDelimiter == DecimalCharDelimiter.Dot)
					{
						var culture = new CultureInfo("en-US");
						result.Amount = arguments.AmountPosition != null ?
							(decimal?)Convert.ToDecimal(
								match.Groups[arguments.AmountPosition.Value].Value
									.Replace("\r\n", " ")
									.Replace("\t", " ")
									.Replace("  ", " "),
								culture) :
							null;
					}
					else //El resto se considera con cultura es-ES
					{
						var culture = new CultureInfo("es-ES");
						result.Amount = arguments.AmountPosition != null ?
							(decimal?)Convert.ToDecimal(
								match.Groups[arguments.AmountPosition.Value].Value
									.Replace("\r\n", " ")
									.Replace("\t", " ")
									.Replace("  ", " "),
								culture) :
							null;
					}
				}
				else
					result.Amount = 0;
				result.Date = (arguments.DatePosition != null && arguments.DatePosition < match.Groups.Count && arguments.DatePosition != null) ?
					(XpDateTime)DateTime.ParseExact(
						match.Groups[arguments.DatePosition.Value].Value
							.Replace("\r\n", " ")
							.Replace("\t", " ")
							.Replace("  ", " "),
						arguments.DateFormat,
						CultureInfo.InvariantCulture
					) :
					null;
				// yyyy-MM-dd HH:mm:ss,fff
				// http://www.codeproject.com/Articles/14743/Easy-String-to-DateTime-DateTime-to-String-and-For
				result.Reference = (arguments.ReferencePosition != null && arguments.ReferencePosition < match.Groups.Count && arguments.ReferencePosition != null) ?
					match.Groups[arguments.ReferencePosition.Value].Value.Replace("\r\n", " ").Replace("\t", " ") :
					"";
				result.Title = (arguments.TitlePosition != null && arguments.TitlePosition < match.Groups.Count && arguments.TitlePosition != null) ?
					match.Groups[arguments.TitlePosition.Value].Value.Replace("\r\n", " ").Replace("\t", " ") :
					"";
				result.WorkerName = (arguments.WorkerPosition != null && arguments.WorkerPosition < match.Groups.Count && arguments.WorkerPosition != null) ?
					match.Groups[arguments.WorkerPosition.Value].Value.Replace("\r\n", " ").Replace("\t", " ") :
					"";
				return result;
			});
		}
		#endregion ExecuteAsync
	}
}
