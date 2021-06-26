using Microsoft.Practices.Unity;
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

namespace PayIn.Application.Public.Handlers
{
    public class MobileEventGetSelectorOpenedHandler :
        IQueryBaseHandler<MobileEventGetSelectorOpenedArguments, SelectorResult>
    {
        [Dependency] public IEntityRepository<Event> Repository { get; set; }
        [Dependency] public ISessionData SessionData { get; set; }

        #region ExecuteAsync
        public async Task<ResultBase<SelectorResult>> ExecuteAsync(MobileEventGetSelectorOpenedArguments arguments)
        {
            var now = DateTime.UtcNow;
            var items = (await Repository.GetAsync())
                .Where(x =>
                    (x.PaymentConcession.Concession.State == ConcessionState.Active) &&
                    (x.IsVisible) &&
                    (x.EventEnd > now) &&
                    (x.State == EventState.Active)
                );

			if (!arguments.Filter.IsNullOrEmpty())
				items = items
					.Where(x =>
						x.Name.Contains(arguments.Filter)
					);

			var result = items
				.Select(x => new SelectorResult
                {
					Id = x.Id,
					Value = x.Name
				});

            return new ResultBase<SelectorResult> { Data = result };
        }
		#endregion ExecuteAsync
	}
}