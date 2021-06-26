using PayIn.Application.Dto.Payments.Arguments;
using PayIn.Application.Dto.Payments.Results;
using PayIn.BusinessLogic.Common;
using PayIn.Domain.Payments;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xp.Application.Dto;
using Xp.Domain;

namespace PayIn.Application.Public.Handlers
{
    public class ExhibitorGetSelectorHandler :
        IQueryBaseHandler<ExhibitorGetSelectorArguments, ExhibitorGetSelectorResult>
    {
        private readonly IEntityRepository<PaymentConcession> Repository;
        private readonly ISessionData SessionData;

        #region Constructors
        public ExhibitorGetSelectorHandler(IEntityRepository<PaymentConcession> repository, ISessionData sessionData)
        {
            if (repository == null) throw new ArgumentNullException("repository");
			if (sessionData == null) throw new ArgumentNullException("sessionData");
			Repository = repository;
            SessionData = sessionData;
        }
        #endregion Constructors

        #region ExecuteAsync
        public async Task<ResultBase<ExhibitorGetSelectorResult>> ExecuteAsync(ExhibitorGetSelectorArguments arguments)
        {
            var items = (await Repository.GetAsync());

			if (!arguments.Filter.IsNullOrEmpty())
				items = items
					.Where(x =>
						x.Concession.Name.Contains(arguments.Filter)
					);

			var result = items
				.Select(x => new ExhibitorGetSelectorResult
                {
					Id = x.Id,
					Value = x.Concession.Name
				});

            return new ResultBase<ExhibitorGetSelectorResult> { Data = result };
        }
		#endregion ExecuteAsync
	}
}