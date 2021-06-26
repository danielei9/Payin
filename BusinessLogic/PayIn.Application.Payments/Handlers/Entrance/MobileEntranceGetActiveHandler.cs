using Microsoft.Practices.Unity;
using PayIn.Application.Dto.Payments.Arguments;
using PayIn.Application.Dto.Payments.Results;
using PayIn.Application.Public.Handlers;
using PayIn.Domain.Payments;
using System;
using System.Linq;
using System.Threading.Tasks;
using Xp.Application.Dto;
using Xp.Domain;

namespace PayIn.Application.Payments.Handlers
{
    public class MobileEntranceGetActiveHandler : IQueryBaseHandler<MobileEntranceGetActiveArguments, MobileEntranceGetActiveResult>
	{
		[Dependency] public IEntityRepository<Entrance> Repository { get; set; }
        [Dependency] public MobileServiceCardGetHandler MobileServiceCardGetHandler { get; set; }

        #region ExecuteAsync
        public async Task<ResultBase<MobileEntranceGetActiveResult>> ExecuteAsync(MobileEntranceGetActiveArguments arguments)
		{
			var now = DateTime.UtcNow;

			var entrances_ = (await Repository.GetAsync())
				.Where(x =>
					x.Uid == arguments.Uid
				);
            var entrances = (await MobileServiceCardGetHandler.GetEntrancesAsync(entrances_, now))
                .Select(x => new MobileEntranceGetActiveResult
                {
                    Id = x.Id,
                    Name = x.Name,
                    LastName = x.LastName,
                    Code = x.Code,
                    EntranceTypeName = x.EntranceTypeName,
                    EventName = x.EventName,
                    Amount = x.Amount
                });

            return new ResultBase<MobileEntranceGetActiveResult>
			{
				Data = entrances
            };
        }
        #endregion ExecuteAsync
    }
}
