using Microsoft.Practices.Unity;
using PayIn.Application.Dto.Payments.Arguments;
using PayIn.Application.Dto.Payments.Results;
using PayIn.BusinessLogic.Common;
using PayIn.Common;
using PayIn.Domain.Payments;
using PayIn.Domain.Public;
using System;
using System.Linq;
using System.Threading.Tasks;
using Xp.Application.Dto;
using Xp.Common;
using Xp.Domain;

namespace PayIn.Application.Payments.Handlers
{
	public class MobileEntranceTypeGetPursesHandler :
        IQueryBaseHandler<MobileEntranceTypeGetPursesArguments, MobileEntranceTypeGetPursesResult>
    {
		[Dependency] public IEntityRepository<Purse> Repository { get; set; }
		[Dependency] public ISessionData SessionData { get; set; }

        #region ExecuteAsync
        public async Task<ResultBase<MobileEntranceTypeGetPursesResult>> ExecuteAsync(MobileEntranceTypeGetPursesArguments arguments)
        {
			var now = new XpDate(DateTime.Now);

			var result = (await Repository.GetAsync())
				.Where(x =>
					(
						x.SystemCard.Cards
							.Any(y => y.Uid == arguments.Uid) &&
						x.State == PurseState.Active &&
						x.Validity >= now
					//) || (
					//	x.PaymentConcessionPurses
					//		.Any(y =>
					//			y.PaymentConcession.Concession.Supplier.Login == SessionData.Login &&
					//			y.State == PaymentConcessionPurseState.Active &&
					//			x.State == PurseState.Active && x.Validity >= now
					//		)
					)
				)
				.OrderByDescending(x => x.Expiration)
				.Select(x => new MobileEntranceTypeGetPursesResult
				{
					Id = x.Id,
					Name = x.Name
				})
				.ToList();

			return new MobileEntranceTypeGetPursesResultBase
			{
				PurseId = result.First().Id,
				Name = result.First().Name,
				Data = result
			};
		}
		#endregion ExecuteAsync
	}
}
