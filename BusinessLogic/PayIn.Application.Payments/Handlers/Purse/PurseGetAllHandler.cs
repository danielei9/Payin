using PayIn.Application.Dto.Payments.Arguments.Purse;
using PayIn.Application.Dto.Payments.Results.Purse;
using PayIn.BusinessLogic.Common;
using PayIn.Common;
using PayIn.Domain.Payments;
using System;
using System.Linq;
using System.Threading.Tasks;
using Xp.Application.Dto;
using Xp.Common;
using Xp.Domain;

namespace PayIn.Application.Public.Handlers
{
	public class PurseGetAllHandler :
		IQueryBaseHandler<PurseGetAllArguments, PurseGetAllResult>
	{
		private readonly IEntityRepository<Purse> Repository;
		private readonly ISessionData SessionData;

        #region Constructors
        public PurseGetAllHandler(IEntityRepository<Purse> repository, ISessionData sessionData)
		{
			if (repository == null) throw new ArgumentNullException("repository");
			if (sessionData == null) throw new ArgumentNullException("sessionData");

            Repository = repository;
			SessionData = sessionData;
        }
		#endregion Constructors

		#region ExecuteAsync
		async Task<ResultBase<PurseGetAllResult>> IQueryBaseHandler<PurseGetAllArguments, PurseGetAllResult>.ExecuteAsync(PurseGetAllArguments arguments)
		{
			var now = new XpDate(DateTime.Now);

			var result = (await Repository.GetAsync())
				.Where(x => 
					(
						x.Concession.Concession.Supplier.Login == SessionData.Login &&
						x.State == PurseState.Active &&
						x.Validity >= now
					) ||
					(
						x.PaymentConcessionPurses
							.Any(y =>
								y.PaymentConcession.Concession.Supplier.Login == SessionData.Login &&
								y.State == PaymentConcessionPurseState.Active &&
								x.State == PurseState.Active && x.Validity >= now
							)
					)
				)
				.Select(x => new
				{
					Id = x.Id,
					Name = x.Name,
					Validity = x.Validity,
					Expiration = x.Expiration,
					NumberPaymentConcessions = (x.PaymentConcessionPurses.Count()-1),
					NumberActivePaymentConcessions = (x.PaymentConcessionPurses.Where(y => y.State== PaymentConcessionPurseState.Active).Count() - 1),
					IsSupplier = x.Concession.Concession.Supplier.Login == SessionData.Login,
					Supplier = (x.Concession.Concession.Supplier.Login != SessionData.Login)? (x.Concession.Concession.Supplier.Name) : null,
                    Total = x.PaymentMedias						
						.Select(y => y.Payments				
							.Select(z => (decimal?)z.Amount)
                            .Sum()
                        )
                        .Sum() ?? 0
				})
				.OrderByDescending(x => x.Expiration)
				.ToList()
				.Select(x => new PurseGetAllResult
				{
					Id = x.Id,
					Name = x.Name,
					Validity = x.Validity,
					Expiration = x.Expiration != XpDate.MaxValue ? x.Expiration : (DateTime?)null,
					NumberPaymentConcessions = x.NumberPaymentConcessions,
					NumberActivePaymentConcessions = x.NumberActivePaymentConcessions,
					IsSupplier = x.IsSupplier,
					Supplier = x.Supplier,
					Total = x.Total
				});

            return new ResultBase<PurseGetAllResult> { Data = result };
		}
		#endregion ExecuteAsync
	}
}