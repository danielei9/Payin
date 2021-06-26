using Microsoft.Practices.Unity;
using PayIn.Application.Dto.SmartCity.Arguments;
using PayIn.Application.Dto.SmartCity.Results;
using PayIn.Domain.SmartCity;
using System;
using System.Linq;
using System.Threading.Tasks;
using Xp.Application.Dto;
using Xp.Domain;

namespace PayIn.Application.SmartCity.Handlers
{
	public class SentiloAlertGetAllHandler :
        IQueryBaseHandler<SentiloAlertGetAllArguments, SentiloAlertGetAllResult>
    {
        [Dependency] public IEntityRepository<Alarm> Repository { get; set; }

        #region ExecuteAsync
        public async Task<ResultBase<SentiloAlertGetAllResult>> ExecuteAsync(SentiloAlertGetAllArguments arguments)
		{
			var items = (await Repository.GetAsync());

			// Entre fechas
			if (arguments.From != null)
				items = items
					.Where(x => x.Timestamp >= arguments.From);
			if (arguments.To != null)
				items = items
					.Where(x => x.Timestamp >= arguments.To);

			// Order
			items = items
				.OrderByDescending(x => x.Timestamp);

			// Take
			if (arguments.Limit != null)
				items = items
					.Take(arguments.Limit.Value);

			// Select
			var result = items
				.Select(y => new
				{
					y.Message,
					y.Timestamp,
					y.Sender
				})
				.ToList()
				.Select(y => new SentiloAlertGetAllResult_Message
				{
					Message = y.Message,
					Timestamp = y.Timestamp.ToUTC(),
					Sender = y.Sender
				});

			return new ResultBase<SentiloAlertGetAllResult>
			{
				Data = new SentiloAlertGetAllResult[] {
					new SentiloAlertGetAllResult
					{
						Messages = result
					}
				}
			};
		}
		#endregion ExecuteAsync
	}
}
